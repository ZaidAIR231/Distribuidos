CREATE TABLE IF NOT EXISTS pikmins (
    id UUID PRIMARY KEY,
    captain_name VARCHAR(100) NOT NULL,
    color VARCHAR(40) NOT NULL,
    onion_count INTEGER NOT NULL CHECK (onion_count >= 0),
    habitat VARCHAR(100) NOT NULL
);