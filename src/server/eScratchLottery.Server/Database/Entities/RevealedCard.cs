using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eScratchLottery.Server.Database.Entities
{
    public class RevealedCard
    {
        [Key]
        public int Id { get; set; }
        public int CardId { get; set; }
        public string PlayerName { get; set; }
        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(CardId))]
        public virtual Card Card { get; set; }
    }
}
