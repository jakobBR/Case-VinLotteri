using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinLotteri.Data;
using VinLotteri.Data.Models;
using VinLotteri.DTOs;

namespace VinLotteri.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly LotteryDbContext _context;

        public TicketsController(LotteryDbContext context)
        {
            _context = context;
        }

        // POST: api/Tickets
        [HttpPost("buy")]
        public async Task<ActionResult<Ticket>> BuyTicket(BuyTicketDTO buyTicketDto)
        {
            var ticket = await _context.Tickets.SingleOrDefaultAsync(t => t.Number == buyTicketDto.Number);

            // Validate ticket
            if (ticket == null)
            {
                return NotFound($"Lodd nummer {buyTicketDto.Number} finnes ikke. Velg et nummer mellom 1 og 100!");
            }
            if (!(ticket.Status == TicketStatus.Available))
            {
                return BadRequest($"Lodd nummer {buyTicketDto.Number} er allerede kjøpt eller reservert");
            }

            // update bought ticket 
            ticket.Status = TicketStatus.Sold;
            ticket.BuyerName = buyTicketDto.BuyerName;

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }


        [HttpPost("draw")]
        public async Task<ActionResult<DrawResultDTO>> DrawWinner()
        {
            //Henter billetter som kan trekkes 
            var eligibleTickets = await _context.Tickets
              .Where(t => t.Status == TicketStatus.Sold)
              .ToListAsync();
            
            if (eligibleTickets.Count == 0)
            {
                return BadRequest("Ingen lodd å trekke! Ingen lodd er solgt eller så er de allerede trukket");
            }

            //trekker ut et tilfeldig winner lodd 
            var random = new Random();
            var winningTicket = eligibleTickets[random.Next(eligibleTickets.Count)];

            // Oppdater statusen til loddet for å indikere at det er trukket
            winningTicket.Status = TicketStatus.Drawn;
            await _context.SaveChangesAsync();

            var result = new DrawResultDTO
            {
                WinningTicketNumber = winningTicket.Number,
                BuyerName = winningTicket.BuyerName
            };

            return Ok(result);
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
        {
            var tickets = await _context.Tickets.ToListAsync();

            var ticketDtos = tickets.Select(ticket => new TicketDTO
            {
                Number = ticket.Number,
                Status = ticket.Status.ToString(), 
                BuyerName = ticket.BuyerName ?? ""
            });

            return Ok(ticketDtos);
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
