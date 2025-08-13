FROM python:3.12-slim
WORKDIR /app
ENV PYTHONDONTWRITEBYTECODE=1 PYTHONUNBUFFERED=1 MINOMBRE_ES=Zaid
EXPOSE 8080
COPY requirements.txt .
RUN pip install --upgrade pip && pip install --no-cache-dir -r requirements.txt
COPY app.py .
ENTRYPOINT ["python","-m","uvicorn","app:app","--host","0.0.0.0","--port","8080"]
