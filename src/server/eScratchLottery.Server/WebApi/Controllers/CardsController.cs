using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using eScratchLottery.Server.WebApi.Messages;
using Microsoft.AspNetCore.SignalR;
using System;

namespace eScratchLottery.Server.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardService _cardService;
        private readonly IHubContext<PlayerHub, IPlayerHub> _playerHub;

        public CardsController(CardService cardService, IHubContext<PlayerHub, IPlayerHub> playerHub)
        {
            _cardService = cardService;
            _playerHub = playerHub;
        }

        [HttpGet]
        public async Task<CardDto[]> GetCards() 
        {
            return await _cardService.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> RevealCard(RevealCardRequest request)
        {
            if (!await _cardService.CardExist(request.CardId))
            {
                return NotFound();
            }

            var validate = await ValidateRequest(request);
            if (!validate.IsValid)
            {
                return BadRequest(new
                {
                    Message = validate.ErrorMessage
                });
            }

            Price price = await _cardService.RevealCard(request.CardId, request.PlayerName);

            await _playerHub.Clients.All.RevealedCard(new CardDto
            {
                Id = request.CardId,
                PlayerName = request.PlayerName,
                WonPrice = price.Value
            });

            return Ok(price);
        }

        private async Task<(bool IsValid, string ErrorMessage)> ValidateRequest(RevealCardRequest request) 
        {
            if (string.IsNullOrEmpty(request.PlayerName)) 
            {
                return (IsValid: false, ErrorMessage: "Player name is required");
            }
            
            if (await _cardService.IsRevealed(request.CardId))
            {
                return (IsValid: false, ErrorMessage: "The card is already revealed");
            }

            if (await _cardService.HasRevealedBy(request.PlayerName))
            {
                return (IsValid: false, ErrorMessage: $"Player {request.PlayerName} already revealed card");
            }

            return (IsValid: true, ErrorMessage: string.Empty);
        }
    }
}
