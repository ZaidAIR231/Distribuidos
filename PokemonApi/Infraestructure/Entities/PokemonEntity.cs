namespace PokemonApi.Entities
{
    public class PokemonEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        // NUEVA 
        public int PS { get; set; }
    }
}

