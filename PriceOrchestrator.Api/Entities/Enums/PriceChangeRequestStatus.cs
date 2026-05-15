namespace PriceOrchestrator.Api.Entities.Enums
{
    public enum PriceChangeRequestStatus
    {
        Pending = 1,    // Esperando ser aplicado
        Applied = 2,    // Ya se aplicó al precio actual
        Expired = 3,    // Venció sin aplicarse
        Cancelled = 4   // Cancelado manualmente
    }
}
