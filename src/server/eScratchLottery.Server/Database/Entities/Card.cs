using System.ComponentModel.DataAnnotations;

namespace eScratchLottery.Server.Database.Entities
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public virtual RevealedCard RevealedCard { get; set; }  
    }
}
