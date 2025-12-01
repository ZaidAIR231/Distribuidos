import os
from dataclasses import dataclass


@dataclass
class Settings:
    """
    Configuración principal del microservicio NintendoApi.

    Se alimenta principalmente desde variables de entorno,
    con valores por defecto razonables para ambiente local.
    """
    # MongoDB
    mongo_uri: str = os.getenv("MONGO_URI", "mongodb://localhost:27017")
    mongo_db_name: str = os.getenv("MONGO_DB_NAME", "nintendo_db")
    mongo_consoles_collection: str = os.getenv("MONGO_CONSOLES_COLLECTION", "consoles")

    # Servidor gRPC
    grpc_host: str = os.getenv("GRPC_HOST", "0.0.0.0")
    grpc_port: int = int(os.getenv("GRPC_PORT", "50051"))

    # Otros
    log_level: str = os.getenv("LOG_LEVEL", "INFO")


# Instancia global sencilla. En proyectos más grandes se podría usar un patrón
# de inyección de dependencias o un contenedor, pero para este microservicio
# esto suele ser suficiente.
settings = Settings()
