# Dockerfile para NintendoApi (Python gRPC + MongoDB)

FROM python:3.12-slim

# Evitar archivos .pyc y forzar stdout sin buffer
ENV PYTHONDONTWRITEBYTECODE=1 \
    PYTHONUNBUFFERED=1

WORKDIR /app

# Instalar dependencias del sistema mínimas (puedes ajustarlo si necesitas más)
RUN apt-get update && apt-get install -y --no-install-recommends \
    build-essential \
 && rm -rf /var/lib/apt/lists/*

# Copiamos sólo requirements primero para aprovechar cache de Docker
COPY requirements.txt ./requirements.txt

RUN pip install --upgrade pip && \
    pip install -r requirements.txt

# Copiar el resto del código
COPY . .

# Asegurarnos de que exista el directorio de salida para los stubs gRPC
RUN mkdir -p nintendo_api/presentation

# Generar los stubs gRPC dentro de la imagen
RUN python -m grpc_tools.protoc \
    -I proto \
    --python_out=nintendo_api/presentation \
    --grpc_python_out=nintendo_api/presentation \
    proto/nintendo.proto

# Puerto gRPC
EXPOSE 50051

# Comando por defecto: arrancar el servidor gRPC
CMD ["python", "-m", "nintendo_api.main"]
