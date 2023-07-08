using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using Flurl.Http;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
{
    public static class LoadCardInfoCommand
    {
        private static Dictionary<string, CardInfo> _cardInfoCache = new Dictionary<string, CardInfo>();

        public static async Task Execute(Model model)
        {
            var players = model.PlayerBoard.Players;

            foreach (var player in players)
            {
                await LoadCommandersCardInfo(player);
                LoadCombinedCommanderColors(player);
                await LoadCommanderDamageCardInfo(player);
            }
        }

        private static async Task LoadCommandersCardInfo(PlayerInfo player)
        {
            player.Commander1 = await GetCardInfo(player.Commander1Id);
            player.Commander2 = await GetCardInfo(player.Commander2Id);
        }

        private static async Task LoadCommanderDamageCardInfo(PlayerInfo player)
        {
            foreach (var ownerPlayerId in player.CommanderDamages.Keys)
            {
                foreach (var commanderDamage in player.CommanderDamages[ownerPlayerId])
                {
                    var cardInfo = await GetCardInfo(commanderDamage.CardId);
                    commanderDamage.CommanderName = cardInfo == null ? "" : cardInfo.Name;
                }
            }
        }

        private static void LoadCombinedCommanderColors(PlayerInfo player)
        {
            player.CommanderColors.Clear();
            if (player.Commander1 != null)
            {
                player.CommanderColors.AddRange(player.Commander1.Colors);
            }
            if (player.Commander2 != null)
            {
                player.CommanderColors.AddRange(player.Commander2.Colors.Where(i => !player.CommanderColors.Contains(i)));
            }
        }

        private static async Task<CardInfo?> GetCardInfo(string? cardId)
        {
            if (string.IsNullOrEmpty(cardId)) return null;

            if (_cardInfoCache.ContainsKey(cardId)) return _cardInfoCache[cardId];

            var cardInfo = new CardInfo
            {
                Identifier = cardId
            };

            var jsonResponse = await $"https://api.scryfall.com/cards/{cardId}"
                    .GetJsonAsync();

            LoadName(cardInfo, jsonResponse);
            LoadImages(cardInfo, jsonResponse);
            LoadColors(cardInfo, jsonResponse);

            _cardInfoCache.Add(cardId, cardInfo);

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
