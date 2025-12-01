# nintendo_api/application/services.py

from __future__ import annotations

from dataclasses import dataclass
from typing import List, Optional

from pydantic import ValidationError as PydanticValidationError

from nintendo_api.domain.models import Console
from nintendo_api.infrastructure.repositories import (
    ListConsolesResult,
    MongoConsoleRepository,
)


# ==========================
# EXCEPCIONES DE DOMINIO
# ==========================


class DomainError(Exception):
    """Base para errores de dominio / negocio."""


class ConsoleNotFoundError(DomainError):
    """Se lanza cuando una consola no existe."""


class ConsoleAlreadyExistsError(DomainError):
    """Se lanza cuando se intenta crear/actualizar a un nombre duplicado."""


class ConsoleValidationError(DomainError):
    """Se lanza cuando los datos de entrada no pasan las validaciones."""


# ==========================
# SERVICIO DE APLICACIÓN
# ==========================


@dataclass
class ConsoleService:
    """
    Servicio de aplicación para la entidad Console.

    Orquesta validaciones, reglas de negocio y delega en el repositorio.
    """

    repository: MongoConsoleRepository

    # -------------
    # CREATE
    # -------------

    async def create_console(self, data: dict) -> Console:
        """
        Crea una nueva consola a partir de un dict de datos de entrada.

        - Valida usando el modelo Pydantic.
        - Verifica duplicados por nombre.
        """
        try:
            console = Console(**data)
        except PydanticValidationError as exc:
            raise ConsoleValidationError(str(exc)) from exc

        # Verificar duplicado por nombre
        if await self.repository.exists_by_name(console.name):
            raise ConsoleAlreadyExistsError(
                f"A console with name '{console.name}' already exists."
            )

        created = await self.repository.create_console(console)
        return created

    # -------------
    # READ
    # -------------

    async def get_console_by_id(self, console_id: str) -> Console:
        try:
            console = await self.repository.get_console_by_id(console_id)
        except ValueError as exc:  # id inválido (ObjectId)
            raise ConsoleValidationError(str(exc)) from exc

        if console is None:
            raise ConsoleNotFoundError(f"Console with id '{console_id}' not found.")
        return console

    async def list_consoles(
        self,
        page_size: int,
        page_token: str,
        generation: Optional[int],
        name_query: Optional[str],
    ) -> ListConsolesResult:
        result = await self.repository.list_consoles(
            page_size=page_size,
            page_token=page_token,
            generation=generation,
            name_query=name_query,
        )
        return result

    # -------------
    # UPDATE
    # -------------

    async def update_console(self, console_id: str, data: dict) -> Console:
        """
        Actualiza una consola existente con nuevos datos.

        - Valida usando Pydantic.
        - Verifica duplicado por nombre (excluyendo el propio id).
        - Si no existe, lanza ConsoleNotFoundError.
        """
        try:
            console = Console(**data)
        except PydanticValidationError as exc:
            raise ConsoleValidationError(str(exc)) from exc

        # Checar que no haya otra consola con el mismo nombre
        if await self.repository.exists_by_name_excluding_id(console.name, console_id):
            raise ConsoleAlreadyExistsError(
                f"Another console with name '{console.name}' already exists."
            )

        try:
            updated = await self.repository.update_console(console_id, console)
        except ValueError as exc:  # id inválido
            raise ConsoleValidationError(str(exc)) from exc

        if updated is None:
            raise ConsoleNotFoundError(f"Console with id '{console_id}' not found.")

        return updated

    # -------------
    # DELETE
    # -------------

    async def delete_console(self, console_id: str) -> bool:
        """
        Elimina una consola. Si no existe, lanza ConsoleNotFoundError.
        """
        try:
            deleted = await self.repository.delete_console(console_id)
        except ValueError as exc:  # id inválido
            raise ConsoleValidationError(str(exc)) from exc

        if not deleted:
            raise ConsoleNotFoundError(f"Console with id '{console_id}' not found.")
        return True
