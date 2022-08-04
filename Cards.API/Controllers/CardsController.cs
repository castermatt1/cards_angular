using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cards.API.Data;
using Cards.API.Models;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext cardsDbContext;

        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }

        // GET: All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        // GET: single Cards
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await cardsDbContext.Cards.FirstOrDefaultAsync(m => m.id == id);
            if (card == null)
            {
                return NotFound("Card not found");
            }

            return Ok(card);
        }

        // POST: Add Card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.id = Guid.NewGuid();
            await cardsDbContext.AddAsync(card);
            await cardsDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCard), new { id = card.id }, card);
        }

        // GET: Update a Card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var exitingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.id == id);
            if (exitingCard != null)
            {
                exitingCard.CardholderName = card.CardholderName;
                exitingCard.CardNumber = card.CardNumber;
                exitingCard.ExpiryMonth = card.ExpiryMonth;
                exitingCard.ExpiryYear = card.ExpiryYear;
                exitingCard.CVC = card.CVC;
                await cardsDbContext.SaveChangesAsync();
                return Ok(exitingCard);
            }
            return NotFound("Card not found");
        }


        // GET: Update a Card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var exitingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.id == id);
            if (exitingCard != null)
            {
                cardsDbContext.Remove(exitingCard);
                await cardsDbContext.SaveChangesAsync();
                return Ok(exitingCard);
            }
            return NotFound("Card not found");
        }
    }
}
