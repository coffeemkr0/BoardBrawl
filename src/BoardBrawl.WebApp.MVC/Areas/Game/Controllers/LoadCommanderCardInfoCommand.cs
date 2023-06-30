using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using Flurl.Http;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
{
    public static class LoadCommanderCardInfoCommand
    {
        public static async Task Execute(PlayerInfo playerInfo)
        {
            await LoadCommandersCardInfo(playerInfo);
            LoadCombinedCommanderColors(playerInfo);
        }

        private static async Task LoadCommandersCardInfo(PlayerInfo playerInfo)
        {
            playerInfo.Commander1 = await GetCardInfo(playerInfo.Commander1Id);
            playerInfo.Commander2 = await GetCardInfo(playerInfo.Commander2Id);
        }

        private static void LoadCombinedCommanderColors(PlayerInfo playerInfo)
        {
            playerInfo.CommanderColors.Clear();
            if (playerInfo.Commander1 != null)
            {
                playerInfo.CommanderColors.AddRange(playerInfo.Commander1.Colors);
            }
            if (playerInfo.Commander2 != null)
            {
                playerInfo.CommanderColors.AddRange(playerInfo.Commander2.Colors.Where(i => !playerInfo.CommanderColors.Contains(i)));
            }
        }

        private static async Task<CardInfo?> GetCardInfo(string? cardId)
        {
            if (string.IsNullOrEmpty(cardId)) return null;

            var cardInfo = new CardInfo
            {
                Identifier = cardId
            };

            var jsonResponse = await $"https://api.scryfall.com/cards/{cardId}"
                    .GetJsonAsync();

            LoadName(cardInfo, jsonResponse);
            LoadImages(cardInfo, jsonResponse);
            LoadColors(cardInfo, jsonResponse);

            return cardInfo;
        }

        private static void LoadName(CardInfo cardInfo, dynamic jsonResponse)
        {
            cardInfo.Name = jsonResponse.name;
        }

        private static void LoadImages(CardInfo cardInfo, dynamic jsonResponse)
        {
            cardInfo.ImageUriSmall = new Uri(jsonResponse.image_uris.small);
            cardInfo.ImageUri = new Uri(jsonResponse.image_uris.normal);
            cardInfo.ImageUriLarge = new Uri(jsonResponse.image_uris.large);
        }

        private static void LoadColors(CardInfo cardInfo, dynamic jsonResponse)
        {
            cardInfo.Colors.Clear();

            foreach (var colorString in jsonResponse.colors)
            {
                switch (colorString)
                {
                    case "W":
                        cardInfo.Colors.Add(Colors.White);
                        break;
                    case "B":
                        cardInfo.Colors.Add(Colors.Black);
                        break;
                    case "G":
                        cardInfo.Colors.Add(Colors.Green);
                        break;
                    case "R":
                        cardInfo.Colors.Add(Colors.Red);
                        break;
                    case "U":
                        cardInfo.Colors.Add(Colors.Blue);
                        break;
                }
            }
        }
    }
}
