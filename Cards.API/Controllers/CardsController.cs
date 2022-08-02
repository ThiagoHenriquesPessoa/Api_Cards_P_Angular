using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext _context;

        public CardsController(CardsDbContext cardsDbContext)
        {
            _context = cardsDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await _context.Cards.ToListAsync();
            return Ok(cards);
        }

        [HttpGet]
        [Route("id:guid")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card != null) return Ok(card);
            return NotFound("Card not found!");
        }

        [HttpPost]
        public async Task<IActionResult> AddCards([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();
            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCard), card.Id, card);
        }

        [HttpPut]
        [Route("id:guid")]
        public async Task<IActionResult> UpdsteCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var _card = await _context.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if(_card != null)
            {
                _card.CardHolderName = card.CardHolderName;
                _card.CardNumber = card.CardNumber;
                _card.ExpiryMonth = card.ExpiryMonth;
                _card.ExpiryYear = card.ExpiryYear;
                _card.CVC = card.CVC;
                await _context.SaveChangesAsync();
                return Ok(_card);
            }

            return NotFound("Card not found!");
        }

        [HttpDelete]
        [Route("id:guid")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card != null)
            {
                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();
                return Ok(card);
            }

            return NotFound("Card not found!");
        }
    }
}