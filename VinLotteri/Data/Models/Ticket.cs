namespace VinLotteri.Data.Models
{

    public class Ticket
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Available;
        public string? BuyerName { get; set; }
    }

    public enum TicketStatus
    {
        Available,
        Sold,
        Drawn
    }
}

