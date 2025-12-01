CREATE EXTENSION IF NOT EXISTS pgcrypto;

INSERT INTO pikmins (id, captain_name, color, onion_count, habitat) VALUES
  (gen_random_uuid(), 'Olimar',  'Red',    5, 'Forest of Hope'),
  (gen_random_uuid(), 'Louie',   'Blue',   3, 'Awakening Wood'),
  (gen_random_uuid(), 'Alph',    'Yellow', 4, 'Tropical Wilds'),
  (gen_random_uuid(), 'Brittany','Pink',   2, 'Garden of Hope');
