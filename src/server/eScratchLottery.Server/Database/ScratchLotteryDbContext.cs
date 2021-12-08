using eScratchLottery.Server.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace eScratchLottery.Server.Database
{
    public class ScratchLotteryDbContext : DbContext
    {
        public ScratchLotteryDbContext(DbContextOptions<ScratchLotteryDbContext> options)
            :base(options)
        {}

        public DbSet<Card> Cards { get; set; }
        public DbSet<RevealedCard> RevealedCards { get; set; }
    }
}
