using System.Threading.Tasks;
using eScratchLottery.Server.WebApi.Messages;
using Microsoft.AspNetCore.SignalR;

namespace eScratchLottery.Server.WebApi
{
    public interface IPlayerHub 
    {
        Task RevealedCard(CardDto card);
    }

    public class PlayerHub: Hub<IPlayerHub>{}
}
