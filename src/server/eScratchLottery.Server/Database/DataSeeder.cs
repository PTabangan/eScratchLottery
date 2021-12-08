using System.Collections.Generic;
using System.Linq;
using eScratchLottery.Server.Database.Entities;

namespace eScratchLottery.Server.Database
{
    public class DataSeeder
    {
        static readonly Dictionary<int, string> DemoPrices = new()
        {
            { 2, "200.00" },
            { 4, "300.00" },
            { 6, "200.00" }, 
            { 7, "200.00" }, 
            { 9, "300.00" }, 
            { 12, "200.00" },
            { 13, "200.00" },
            { 16, "200.00" },
            { 18, "200.00" },
            { 20, "200.00" },
            { 41, "300.00" },
            { 65, "200.00" }, 
            { 75, "200.00" }, 
            { 91, "300.00" }, 
            { 100, "FIETS" },
            { 21, "200.00" },
            { 42, "300.00" },
            { 61, "200.00" },
            { 74, "200.00" },
            { 99, "300.00" },
            { 421, "300.00" },
            { 612, "FREE DINER" },
            { 743, "200.00" },
            { 994, "300.00" },
            { 122, "200.00" },
            { 134, "200.00" },
            { 168, "200.00" },
            { 180, "200.00" },
            { 209, "200.00" },
            { 418, "300.00" },
            { 656, "200.00" },
            { 757, "200.00" },
            { 918, "300.00" },
            { 925, "25,0000.00" },
        };

        public static void SeedDemo(ScratchLotteryDbContext dbContext)
        {
            IEnumerable<Card> cards = Enumerable.Range(1, 1000)
                .Select((id) =>
                {
                    DemoPrices.TryGetValue(id, out string price);

                    return new Card
                    {
                        Name = $"Card - {id}",
                        Price = price
                    };
                });

            dbContext.AddRange(cards);
            dbContext.SaveChanges();
        }

        static readonly Dictionary<int, string> TestPrices = new()
        {
            { 2, "200.00" },
            { 4, "300.00" }
        };

        public static void SeedTest(ScratchLotteryDbContext dbContext)
        {
            IEnumerable<Card> cards = Enumerable.Range(1, 10)
                .Select((id) =>
                {
                    TestPrices.TryGetValue(id, out string price);

                    return new Card
                    {
                        Name = $"Card - {id}",
                        Price = price
                    };
                });

            dbContext.AddRange(cards);
            dbContext.SaveChanges();
        }
    }
}
