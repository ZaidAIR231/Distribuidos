# nintendo_api/infrastructure/repositories.py

from __future__ import annotations

from dataclasses import dataclass
from typing import List, Optional, Tuple

from bson import ObjectId
from motor.motor_asyncio import AsyncIOMotorCollection

from nintendo_api.domain.models import Console
from nintendo_api.infrastructure.mongo_client import get_consoles_collection


def _parse_object_id(id_str: str) -> ObjectId:
    """
    Convierte un string a ObjectId o lanza ValueError si es inválido.
    """
    try:
        return ObjectId(id_str)
    except Exception as exc:  # bson.errors.InvalidId u otros
        raise ValueError(f"Invalid ObjectId: {id_str}") from exc


@dataclass
class ListConsolesResult:
    consoles: List[Console]
    next_page_token: str
    total_count: int


class MongoConsoleRepository:
    """
    Implementación de repositorio para la entidad Console usando MongoDB (Motor).

    Esta clase no hace validaciones de negocio, solo persiste y consulta datos.
    """

    def __init__(self, collection: Optional[AsyncIOMotorCollection] = None) -> None:
        self._collection = collection or get_consoles_collection()

    # ==============
    # CREATE
    # ==============

    async def create_console(self, console: Console) -> Console:
        """
        Inserta una nueva consola en la colección.

        Asume que las validaciones de negocio (duplicados, etc.)
        ya fueron hechas en la capa de servicio.
        """
        doc = console.to_mongo()
        result = await self._collection.insert_one(doc)
        doc["_id"] = result.inserted_id
        return Console.from_mongo(doc)

    # ==============
    # READ
    # ==============

    async def get_console_by_id(self, id_str: str) -> Optional[Console]:
        oid = _parse_object_id(id_str)
        doc = await self._collection.find_one({"_id": oid})
        if not doc:
            return None
        return Console.from_mongo(doc)

    async def exists_by_name(self, name: str) -> bool:
        """
        Devuelve True si existe una consola con el mismo nombre (case-sensitive).
        Se podría ajustar a case-insensitive con un índice collation si se requiere.
        """
        doc = await self._collection.find_one({"name": name})
        return doc is not None

    async def exists_by_name_excluding_id(self, name: str, id_str: str) -> bool:
        """
        Devuelve True si existe una consola con el mismo nombre,
        excluyendo el id proporcionado (para updates).
        """
        oid = _parse_object_id(id_str)
        doc = await self._collection.find_one(
            {
                "name": name,
                "_id": {"$ne": oid},
            }
        )
        return doc is not None

    async def list_consoles(
        self,
        page_size: int,
        page_token: str,
        generation: Optional[int],
        name_query: Optional[str],
    ) -> ListConsolesResult:
        """
        Lista consolas con paginación por offset (page_token como entero en string).

        page_token:
            - "" o valor no convertible a int => se toma como 0 (inicio).
            - Representa el número de documentos a omitir (skip).

        Retorna:
            - lista de Console,
            - next_page_token (string) o "" si no hay más páginas,
            - total_count (int) de documentos que cumplen el filtro.
        """
        # Sanitizar page_size
        if page_size <= 0:
            page_size = 10
        page_size = min(page_size, 100)  # límite de seguridad

        # Parsear page_token como offset
        try:
            offset = int(page_token) if page_token else 0
            if offset < 0:
                offset = 0
        except ValueError:
            offset = 0

        # Construir filtro
        query: dict = {}
        if generation and generation > 0:
            query["generation"] = generation
        if name_query:
            # Búsqueda básica "contains" case-insensitive
            query["name"] = {"$regex": name_query, "$options": "i"}

        total_count = await self._collection.count_documents(query)

        cursor = (
            self._collection.find(query)
            .sort("name", 1)
            .skip(offset)
            .limit(page_size)
        )

        results: List[Console] = []
        async for doc in cursor:
            results.append(Console.from_mongo(doc))

        new_offset = offset + len(results)
        next_page_token = str(new_offset) if new_offset < total_count else ""

        return ListConsolesResult(
            consoles=results,
            next_page_token=next_page_token,
            total_count=total_count,
        )

    # ==============
    # UPDATE
    # ==============

    async def update_console(self, id_str: str, console: Console) -> Optional[Console]:
        """
        Reemplaza los campos de la consola (update set) con los datos del modelo.

        Retorna:
            - Console actualizada si se encontró el documento.
            - None si no se encontró.
        """
        oid = _parse_object_id(id_str)
        update_doc = {"$set": console.to_mongo()}

        result = await self._collection.find_one_and_update(
            {"_id": oid},
            update_doc,
            return_document=True,  # Devuelve el documento actualizado
        )

        if not result:
            return None

        return Console.from_mongo(result)

    # ==============
    # DELETE
    # ==============

    async def delete_console(self, id_str: str) -> bool:
        """
        Elimina una consola por id. Devuelve True si se eliminó un documento.
        """
        oid = _parse_object_id(id_str)
        result = await self._collection.delete_one({"_id": oid})
        return result.deleted_count > 0
