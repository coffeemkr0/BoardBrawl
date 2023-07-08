using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Data.Application;
using BoardBrawl.Data.Application.Models;
using BoardBrawl.Repositories.Game;
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public class Service : IService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public Service(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public GameInfo? GetGameInfo(int id, string userId)
        {
            var repoGameInfo = _repository.GetGameInfo(id);
            if (repoGameInfo != null)
            {
                var gameInfo = _mapper.Map<GameInfo>(repoGameInfo);

                LoadIsSelf(userId, gameInfo);
                LoadInfectPercentages(gameInfo);
                LoadCommanderDamages(gameInfo, repoGameInfo.CommanderDamages);

                return gameInfo;
            }
            else
            {
                return null;
            }
        }

        public void PassTurn(int gameId)
        {
            var repoGameInfo = _repository.GetGameInfo(gameId);

            if (repoGameInfo == null) throw new Exception($"Game not found with id {gameId}");
          
            var now = DateTime.Now;

            //Start game if it hasn't started
            if (repoGameInfo.GameStart == null)
            {
                repoGameInfo.GameStart = now;
            }

            //Set active player to next player in list (this assumes the players are already orded by TurnOrder from the repo)
            if (repoGameInfo.ActivePlayerId == null)
            {
                repoGameInfo.ActivePlayerId = repoGameInfo.Players.First().Id;
            }
            else
            {
                var currentPlayerIndex = repoGameInfo.Players.FindIndex(i => i.Id == repoGameInfo.ActivePlayerId);
                int nextPlayerIndex = (currentPlayerIndex + 1) % repoGameInfo.Players.Count;
                repoGameInfo.ActivePlayerId = repoGameInfo.Players[nextPlayerIndex].Id;
            }

            //Start the turn
            repoGameInfo.TurnStart = now;

            _repository.UpdateGameInfo(repoGameInfo);
        }

        public void UpdateFocusedPlayer(int playerId, int focusedPlayerId)
        {
            _repository.UpdateFocusedPlayer(playerId, focusedPlayerId);
        }

        public void AdjustLifeTotal(int playerId, int amount)
        {
            _repository.AdjustLifeTotal(playerId, amount);
        }

        public void UpdatePeerId(int playerId, Guid peerId)
        {
            _repository.UpdatePeerId(playerId, peerId);
        }

        public void JoinGame(int gameId, string userId)
        {
            var repoPlayerInfo = _repository.GetPlayer(userId);

            if (repoPlayerInfo == null)
            {
                //The user does not have a player in a game, create a new one
                var newPlayer = CreateNewPlayer(userId);
                repoPlayerInfo = _mapper.Map<Repositories.Game.Models.PlayerInfo>(newPlayer);
            }

            //Put the player at the end of the turn order
            var turnOrder = _repository.GetPlayers(gameId).OrderBy(i => i.TurnOrder).LastOrDefault()?.TurnOrder + 1;
            repoPlayerInfo.TurnOrder = turnOrder ?? 0;

            _repository.AddPlayerToGame(gameId, repoPlayerInfo);
        }

        public void UpdateCommander(int playerId, int slot, string cardId)
        {
            _repository.UpdateCommander(playerId, slot, cardId);
        }

        public void AdjustInfectCount(int playerId, int amount)
        {
            _repository.AdjustInfectCount(playerId, amount);
        }

        public void AdjustCommanderDamage(int gameId, int playerId, int ownerPlayerId, string cardId, int amount)
        {
            _repository.AdjustCommanderDamage(gameId, playerId, ownerPlayerId, cardId, amount);
        }

        private PlayerInfo CreateNewPlayer(string userId)
        {
            //TODO:Get default player name from user preferences
            return new PlayerInfo
            {
                UserId = userId,
                Name = $"Player {userId.ToString()[..5]}",
                LifeTotal = 40
            };
        }

        private void LoadIsSelf(string userId, GameInfo gameInfo)
        {
            var myPlayer = gameInfo.Players.FirstOrDefault(i => i.UserId == userId);
            if (myPlayer != null) { myPlayer.IsSelf = true; }
        }

        private void LoadInfectPercentages(GameInfo gameInfo)
        {
            foreach (var player in gameInfo.Players)
            {
                player.InfectPercentage = Math.Min(100, Convert.ToInt32(((float)player.InfectCount / 10.0f) * 100));
            }
        }

        private void LoadCommanderDamages(GameInfo gameInfo, List<Repositories.Game.Models.CommanderDamage> repoCommanderDamages)
        {
            //Get a collection of all commanders in the game, stored per player
            var commanderDictionary = new Dictionary<int, List<string>>();

            foreach (var ownerPlayer in gameInfo.Players)
            {
                commanderDictionary[ownerPlayer.Id] = new List<string>();

                if(!string.IsNullOrEmpty(ownerPlayer.Commander1Id)) 
                {
                    commanderDictionary[ownerPlayer.Id].Add(ownerPlayer.Commander1Id);
                }

                if (!string.IsNullOrEmpty(ownerPlayer.Commander2Id))
                {
                    commanderDictionary[ownerPlayer.Id].Add(ownerPlayer.Commander2Id);
                }
            }

            //Iterate each player and create a commander damage entry for each commander in the game
            foreach (var player in gameInfo.Players)
            {
                foreach (var ownerPlayerId in commanderDictionary.Keys)
                {
                    player.CommanderDamages.Add(ownerPlayerId, new List<Models.CommanderDamage>());

                    var ownerPlayer = gameInfo.Players.First(i => i.Id == ownerPlayerId);

                    foreach (var commanderId in commanderDictionary[ownerPlayerId])
                    {
                        var commanderDamage = new Models.CommanderDamage
                        {
                            PlayerId = player.Id,
                            OwnerPlayerId = ownerPlayerId,
                            OwnerPlayerName = ownerPlayer.Name,
                            CardId = commanderId
                        };

                        //If the repo contains an existing damage entry, assign the damage value to the entry
                        var repoCommanderDamage = repoCommanderDamages.FirstOrDefault(i => 
                            i.PlayerId == player.Id && i.OwnerPlayerId == ownerPlayerId && i.CardId == commanderId);

                        if(repoCommanderDamage != null )
                        {
                            commanderDamage.Damage = repoCommanderDamage.Damage;
                            commanderDamage.DamagePercentage = Convert.ToInt32(((float)commanderDamage.Damage / 20.0f) * 100);
                        }

                        player.CommanderDamages[ownerPlayerId].Add(commanderDamage);
                    }
                }
            }
        }

        public void UpdatePlayerTurnOrder(int gameId, List<int> playerIds)
        {
            _repository.UpdatePlayerTurnOrder(gameId, playerIds);
        }

        public bool IsPlayerInGame(int gameId, string userId)
        {
            return _repository.GetPlayers(gameId).Any(i => i.UserId == userId);
        }
    }
}