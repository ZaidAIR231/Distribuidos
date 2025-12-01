# nintendo_api/domain/models.py

from typing import Optional

from bson import ObjectId
from pydantic import BaseModel, EmailStr, Field, ValidationInfo, field_validator


class Console(BaseModel):
    """
    Modelo de dominio para una consola de Nintendo.

    Este modelo se usa en la capa de aplicación y repositorios.
    La capa de presentación (gRPC) se encargará de mapear a/desde
    los mensajes protobuf generados.
    """

    id: Optional[str] = Field(
        default=None,
        description="Identificador de la consola (ObjectId en string).",
    )
    name: str = Field(..., description="Nombre comercial de la consola, p.ej. 'Nintendo Switch'.")
    code_name: Optional[str] = Field(
        default=None,
        description="Nombre clave interno, p.ej. 'NX'.",
    )
    generation: int = Field(
        ...,
        gt=0,
        description="Número de generación, debe ser > 0.",
    )
    release_year: int = Field(
        ...,
        description="Año de lanzamiento (entre 1980 y 2100).",
    )
    discontinued_year: Optional[int] = Field(
        default=None,
        description="Año de descontinuación, si aplica.",
    )
    description: Optional[str] = Field(
        default=None,
        description="Descripción libre de la consola.",
    )
    regions: list[str] = Field(
        default_factory=list,
        description="Lista de regiones principales, p.ej. ['JP', 'NA', 'EU'].",
    )
    portable: bool = Field(
        default=False,
        description="True si es portátil o híbrida.",
    )
    support_email: Optional[EmailStr] = Field(
        default=None,
        description="Correo de soporte opcional.",
    )

    # =========================
    # VALIDACIONES DE NEGOCIO
    # =========================

    @field_validator("name")
    @classmethod
    def name_not_blank(cls, v: str) -> str:
        if not v or not v.strip():
            raise ValueError("name must not be empty")
        return v.strip()

    @field_validator("release_year")
    @classmethod
    def release_year_in_range(cls, v: int) -> int:
        if v < 1980 or v > 2100:
            raise ValueError("release_year must be between 1980 and 2100")
        return v

    @field_validator("discontinued_year")
    @classmethod
    def discontinued_not_before_release(
        cls,
        v: Optional[int],
        info: ValidationInfo,
    ) -> Optional[int]:
        """
        Valida que, si existe discontinued_year, no sea anterior a release_year.
        """
        if v is None:
            return v

        release_year = info.data.get("release_year")
        if release_year is not None and v < release_year:
            raise ValueError("discontinued_year cannot be earlier than release_year")
        return v

    # =========================
    # HELPERS PARA MONGO
    # =========================

    def to_mongo(self) -> dict:
        """
        Convierte el modelo a un dict listo para insertar/actualizar en MongoDB.

        - No incluye el campo 'id'; el repositorio se encargará de mapearlo a '_id'.
        - Excluye campos None para no guardarlos explícitamente si no es necesario.
        """
        data = self.model_dump(exclude={"id"}, exclude_none=True)
        return data

    @classmethod
    def from_mongo(cls, doc: dict) -> "Console":
        """
        Crea un modelo Console a partir de un documento MongoDB.
        Espera que el documento tenga un campo '_id'.
        """
        if not doc:
            raise ValueError("Cannot create Console from empty Mongo document")

        data = dict(doc)
        _id = data.pop("_id", None)

        if isinstance(_id, ObjectId):
            id_str = str(_id)
        else:
            id_str = _id if _id is not None else None

        return cls(id=id_str, **data)
