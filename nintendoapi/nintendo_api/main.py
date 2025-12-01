# nintendo_api/main.py

import asyncio
import logging

import grpc

from nintendo_api.config import settings


async def serve() -> None:
    """
    Punto de entrada asíncrono del servidor gRPC.
    Crea el server, registra el servicio y se queda a la escucha.
    """
    # Import diferido para evitar posibles ciclos de importación durante el desarrollo.
    from nintendo_api.presentation.grpc_handlers import add_nintendo_service

    server = grpc.aio.server()

    # Registrar nuestro servicio NintendoService en el servidor.
    add_nintendo_service(server)

    listen_addr = f"{settings.grpc_host}:{settings.grpc_port}"
    server.add_insecure_port(listen_addr)

    logging.info("NintendoApi gRPC server escuchando en %s", listen_addr)

    await server.start()

    try:
        # Espera hasta que el proceso se termine (Ctrl+C, señal, etc.)
        await server.wait_for_termination()
    except KeyboardInterrupt:
        logging.info("Deteniendo servidor gRPC (KeyboardInterrupt)...")
        # Detención con un tiempo de gracia de 5 segundos
        await server.stop(grace=5)


def main() -> None:
    """
    Función main síncrona que configura logging y ejecuta el loop async.
    Permite ejecutar: `python -m nintendo_api.main`.
    """
    numeric_level = getattr(logging, settings.log_level.upper(), logging.INFO)
    logging.basicConfig(level=numeric_level)

    asyncio.run(serve())


if __name__ == "__main__":
    main()
