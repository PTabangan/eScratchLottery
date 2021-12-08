using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

using eScratchLottery.Server.Test.Fixture;
using eScratchLottery.Server.WebApi.Messages;
using System.Net;

namespace eScratchLottery.Server.Test
{
    [Collection(nameof(ScratchLotteryApiTestCollection))]
    public class TheCardApi
    {
        private readonly TestServerFixture _fixture;

        public TheCardApi(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Can_get_the_list_of_cards() 
        {
            using var httpClient = _fixture.CreateHttpClient();
            
            var response = await httpClient.GetAsync("api/cards");
            CardDto[] cards = await response.Content.ReadAsync<CardDto[]>();
            
            cards.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task When_revealing_card_that_does_not_exist_it_should_return_NotFound()
        {
            int unknownCardId = 100000000;

            using var httpClient = _fixture.CreateHttpClient();

            var response = await httpClient.PostAsync("api/cards", new RevealCardRequest 
            { 
                CardId = unknownCardId,
                PlayerName = "SomePlayerName"
            });

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_revealing_card_that_is_already_revealed_it_should_return_BadRequest()
        {
            int cardIdToReveal = 1;

            using var httpClient = _fixture.CreateHttpClient();

            var response = await httpClient.RevealCard(cardIdToReveal, "SomePlayerName");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await httpClient.RevealCard(cardIdToReveal, "SomePlayerName");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            var error = await response.Content.ReadAsync<BadRequestMessage>();

            error.Message.Should().Be("The card is already revealed");

        }

        [Fact]
        public async Task When_revealing_card_without_PlayerName_it_should_return_BadRequest()
        {
            int cardIdToReveal = 2;

            using var httpClient = _fixture.CreateHttpClient();

            var response = await httpClient.RevealCard(cardIdToReveal, playerName:string.Empty) ;
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadAsync<BadRequestMessage>();

            error.Message.Should().Be("Player name is required");

        }

        [Fact]
        public async Task When_revealing_card_it_should_return_the_Price()
        {
            int cardIdToReveal = 2;
            string expectedPrice = "200.00";

            using var httpClient = _fixture.CreateHttpClient();

            var response = await httpClient.RevealCard(cardIdToReveal, "SomePlayerName");
            
            var price = await response.Content.ReadAsync<Price>();

            price.Value.Should().Be(expectedPrice);
        }

        class BadRequestMessage 
        {
            public string Message { get; set; }
        }
    }
}
