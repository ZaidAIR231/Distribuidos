# nintendo_api/infrastructure/mongo_client.py

from typing import Optional

from motor.motor_asyncio import (
    AsyncIOMotorClient,
    AsyncIOMotorDatabase,
    AsyncIOMotorCollection,
)

from nintendo_api.config import settings

_client: Optional[AsyncIOMotorClient] = None


def get_client() -> AsyncIOMotorClient:
    """
    Devuelve un cliente global de MongoDB (Motor).

    Se inicializa de forma perezosa (lazy) la primera vez.
    Motor está pensado para compartir una sola instancia de client.
    """
    global _client
    if _client is None:
        _client = AsyncIOMotorClient(settings.mongo_uri)
    return _client


def get_database() -> AsyncIOMotorDatabase:
    """
    Devuelve la base de datos configurada en MONGO_DB_NAME.
    """
    client = get_client()
    return client[settings.mongo_db_name]


def get_consoles_collection() -> AsyncIOMotorCollection:
    """
    Devuelve la colección principal de consolas (por defecto 'consoles').
    """
    db = get_database()
    return db[settings.mongo_consoles_collection]


async def ping_database() -> None:
    """
    Hace un ping a la base de datos para verificar conectividad.
    Útil para health checks o pruebas de conexión.
    """
    client = get_client()
    await client.admin.command("ping")
