namespace ScrollsOfSealsApi.Infrastructure.Documents;

public enum MissionRankMongo
{
    Unspecified = 0,  
    D = 1,            // Misiones simples (entregar pergaminos, limpiar ríos)
    C = 2,            // Misiones de bajo riesgo
    B = 3,            // Misiones de riesgo medio
    A = 4,            // Misiones de alto riesgo
    S = 5             // Misiones clase S (nivel Kage o superior)
}
