import os
from fastapi import FastAPI

app = FastAPI()

@app.get("/hello-world")
def hello_world():
    nombre = os.getenv("MINOMBRE_ES", "Zaid")
    return {"Hola :D soy zaid"}
