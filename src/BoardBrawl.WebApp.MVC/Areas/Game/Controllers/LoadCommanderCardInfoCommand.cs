using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using Flurl.Http;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
{
    public static class LoadCommanderCardInfoCommand
    {
        public static async Task Execute(PlayerInfo playerInfo)
        {
            playerInfo.Commander1 = await GetCardInfo(playerInfo.Commander1Id);
            playerInfo.Commander2 = await GetCardInfo(playerInfo.Commander2Id);
        }

        private static async Task<CardInfo?> GetCardInfo(string? cardId)
        {
            if (string.IsNullOrEmpty(cardId)) return null;

            CardInfo cardInfo = new CardInfo
            {
                Identifier = cardId
            };

            var jsonResponse = await $"https://api.scryfall.com/cards/{cardId}"
                    .GetJsonAsync();

            cardInfo.Name = jsonResponse.name;
            cardInfo.ImageUriSmall = new Uri(jsonResponse.image_uris.small);
            cardInfo.ImageUri = new Uri(jsonResponse.image_uris.normal);
            cardInfo.ImageUriSmall = new Uri(jsonResponse.image_uris.large);

            return cardInfo;
        }
    }
}
