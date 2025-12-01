# nintendo_api/presentation/grpc_handlers.py

from __future__ import annotations

from typing import Optional

import grpc

from nintendo_api.application.services import (
    ConsoleAlreadyExistsError,
    ConsoleNotFoundError,
    ConsoleService,
    ConsoleValidationError,
)
from nintendo_api.infrastructure.repositories import MongoConsoleRepository

# Import de los módulos generados por protoc (relativo al paquete)
from . import nintendo_pb2, nintendo_pb2_grpc


# ==========================
# MAPEOS PROTO <-> DOMINIO
# ==========================


def console_to_proto(console) -> nintendo_pb2.Console:
    """
    Convierte un modelo Console (dominio) a mensaje protobuf Console.
    """
    discontinued_year = console.discontinued_year or 0
    description = console.description or ""
    support_email = console.support_email or ""

    return nintendo_pb2.Console(
        id=console.id or "",
        name=console.name,
        code_name=console.code_name or "",
        generation=console.generation,
        release_year=console.release_year,
        discontinued_year=discontinued_year,
        description=description,
        regions=list(console.regions or []),
        portable=console.portable,
        support_email=support_email,
    )


def proto_to_console_dict(msg: nintendo_pb2.Console) -> dict:
    """
    Convierte un mensaje protobuf Console en un dict apto para crear
    un modelo de dominio Console.
    """
    data = {
        "name": msg.name,
        "code_name": msg.code_name or None,
        "generation": msg.generation,
        "release_year": msg.release_year,
        "discontinued_year": msg.discontinued_year or None,
        "description": msg.description or None,
        "regions": list(msg.regions),
        "portable": msg.portable,
        "support_email": msg.support_email or None,
    }
    return data


class NintendoServiceServicer(nintendo_pb2_grpc.NintendoServiceServicer):
    """
    Implementación gRPC del servicio NintendoService definido en nintendo.proto.
    """

    def __init__(self, console_service: Optional[ConsoleService] = None) -> None:
        self._service = console_service or ConsoleService(
            repository=MongoConsoleRepository()
        )

    # ---- CREATE ----

    async def CreateConsole(self, request, context):
        try:
            data = proto_to_console_dict(request.console)
            created = await self._service.create_console(data)
            return nintendo_pb2.ConsoleResponse(console=console_to_proto(created))

        except ConsoleValidationError as exc:
            await context.abort(
                grpc.StatusCode.INVALID_ARGUMENT,
                str(exc),
            )
        except ConsoleAlreadyExistsError as exc:
            await context.abort(
                grpc.StatusCode.ALREADY_EXISTS,
                str(exc),
            )
        except Exception:
            await context.abort(
                grpc.StatusCode.INTERNAL,
                "Internal server error",
            )

    # ---- GET BY ID ----

    async def GetConsoleById(self, request, context):
        try:
            console = await self._service.get_console_by_id(request.id)
            return nintendo_pb2.ConsoleResponse(console=console_to_proto(console))

        except ConsoleValidationError as exc:
            await context.abort(
                grpc.StatusCode.INVALID_ARGUMENT,
                str(exc),
            )
        except ConsoleNotFoundError as exc:
            await context.abort(
                grpc.StatusCode.NOT_FOUND,
                str(exc),
            )
        except Exception:
            await context.abort(
                grpc.StatusCode.INTERNAL,
                "Internal server error",
            )

    # ---- LIST ----

    async def ListConsoles(self, request, context):
        try:
            generation = request.generation if request.generation > 0 else None
            name_query = request.name_query or None

            result = await self._service.list_consoles(
                page_size=request.page_size,
                page_token=request.page_token,
                generation=generation,
                name_query=name_query,
            )

            consoles_pb = [console_to_proto(c) for c in result.consoles]

            return nintendo_pb2.ListConsolesResponse(
                consoles=consoles_pb,
                next_page_token=result.next_page_token,
                total_count=result.total_count,
            )

        except ConsoleValidationError as exc:
            await context.abort(
                grpc.StatusCode.INVALID_ARGUMENT,
                str(exc),
            )
        except Exception:
            await context.abort(
                grpc.StatusCode.INTERNAL,
                "Internal server error",
            )

    # ---- UPDATE ----

    async def UpdateConsole(self, request, context):
        try:
            data = proto_to_console_dict(request.console)
            updated = await self._service.update_console(request.id, data)
            return nintendo_pb2.ConsoleResponse(console=console_to_proto(updated))

        except ConsoleValidationError as exc:
            await context.abort(
                grpc.StatusCode.INVALID_ARGUMENT,
                str(exc),
            )
        except ConsoleAlreadyExistsError as exc:
            await context.abort(
                grpc.StatusCode.ALREADY_EXISTS,
                str(exc),
            )
        except ConsoleNotFoundError as exc:
            await context.abort(
                grpc.StatusCode.NOT_FOUND,
                str(exc),
            )
        except Exception:
            await context.abort(
                grpc.StatusCode.INTERNAL,
                "Internal server error",
            )

    # ---- DELETE ----

    async def DeleteConsole(self, request, context):
        try:
            await self._service.delete_console(request.id)
            return nintendo_pb2.DeleteConsoleResponse(success=True)

        except ConsoleValidationError as exc:
            await context.abort(
                grpc.StatusCode.INVALID_ARGUMENT,
                str(exc),
            )
        except ConsoleNotFoundError as exc:
            await context.abort(
                grpc.StatusCode.NOT_FOUND,
                str(exc),
            )
        except Exception:
            await context.abort(
                grpc.StatusCode.INTERNAL,
                "Internal server error",
            )


def add_nintendo_service(server: grpc.aio.Server) -> None:
    """
    Registra NintendoService en el servidor gRPC.
    """
    servicer = NintendoServiceServicer()
    nintendo_pb2_grpc.add_NintendoServiceServicer_to_server(servicer, server)
