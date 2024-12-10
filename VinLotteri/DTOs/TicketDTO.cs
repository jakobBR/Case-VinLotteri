using VinLotteri.Data.Models;

namespace VinLotteri.DTOs
{
    public class TicketDTO
    {
        public int Number { get; set; }
        public string Status { get; set; } = TicketStatus.Available.ToString();
        public required string BuyerName { get; set; }
    }
}
