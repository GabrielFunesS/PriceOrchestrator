namespace PriceOrchestrator.Api.Entities.Enums
{
    public enum PromotionStatus
    {
        Scheduled = 1,  // Programada, aún no empieza
        Active = 2,     // Activa ahora
        Paused = 3,     // Pausada manualmente
        Expired = 4,    // Terminó por fecha
        Cancelled = 5   // Cancelada manualmente
    }
}
