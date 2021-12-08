namespace eScratchLottery.Server.WebApi.Messages
{
    public class CardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlayerName { get; set; }

        public string WonPrice { get; set; }
    }
}
