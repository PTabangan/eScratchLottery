using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using eScratchLottery.Server.Database;
using eScratchLottery.Server.Database.Entities;
using eScratchLottery.Server.WebApi.Messages;


namespace eScratchLottery.Server.WebApi
{
    public class CardService
    {
        readonly ScratchLotteryDbContext _dbContext;
        public CardService(ScratchLotteryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        internal async Task<CardDto[]> GetAll() 
        {
            var cards = await _dbContext
                .Cards
                .AsNoTracking()
                .Include(p => p.RevealedCard)
                .ToListAsync();

            return cards.Select(p => new CardDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    PlayerName = p.RevealedCard?.PlayerName,
                    WonPrice = p.RevealedCard != null ? p.Price : null
                })
                .ToArray();
        }

        internal async Task<bool> IsRevealed(int id)
        {
            return await _dbContext.RevealedCards.AnyAsync(p => p.CardId == id);
        }

        internal async Task<bool> HasRevealedBy(string playerName)
        {
            return await _dbContext.RevealedCards.AnyAsync(p => playerName.Equals(p.PlayerName,StringComparison.OrdinalIgnoreCase));
        }

        internal async Task<bool> CardExist(int id)
        {
            return await Get(id) != null;
        }

        internal async Task<Price> RevealCard(int id, string playerName)
        {
            var card = await Get(id);

            if (card == null) 
            {
                throw new Exception($"Card with Id of {id} does not exist");
            }

            _dbContext.RevealedCards.Add(new RevealedCard
                {
                    CardId = id,
                    PlayerName = playerName,
                    DateTime = DateTime.Now
                });

            await _dbContext.SaveChangesAsync();

            return new Price
            {
                Value = card.Price
            };
        }

        private async Task<Card> Get(int id) 
        {
            return await _dbContext.Cards.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
