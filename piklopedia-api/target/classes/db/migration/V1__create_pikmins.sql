-- Extensión para UUID (tu usuario es superuser en este contenedor)
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Esquema base
CREATE TABLE IF NOT EXISTS pikmins (
    id UUID PRIMARY KEY,
    captain_name VARCHAR(100) NOT NULL,
    color VARCHAR(50) NOT NULL,
    onion_count INT NOT NULL,
    habitat VARCHAR(150)
);
