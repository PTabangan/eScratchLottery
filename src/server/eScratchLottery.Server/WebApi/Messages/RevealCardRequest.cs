namespace eScratchLottery.Server.WebApi.Messages
{
    public class RevealCardRequest
    {
        public int CardId { get; set; }
        public string PlayerName { get; set; }
    }
}
