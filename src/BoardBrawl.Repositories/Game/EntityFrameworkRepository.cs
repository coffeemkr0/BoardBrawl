using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Data.Application;
using BoardBrawl.Repositories.Game.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardBrawl.Repositories.Game
{
    public class EntityFrameworkRepository : IRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public EntityFrameworkRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public void AddPlayerToGame(int gameId, PlayerInfo playerInfo)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.UserId == playerInfo.UserId);

            if (playerEntity == null)
            {
                playerEntity = _mapper.Map<Data.Application.Models.Player>(playerInfo);
                _applicationDbContext.Players.Add(playerEntity);
            }

            playerEntity.GameId = gameId;
            _applicationDbContext.SaveChanges();

            playerInfo.Id = playerEntity.Id;
            playerInfo.GameId = gameId;
        }

        public GameInfo GetGameInfo(int id)
        {
            var gameEntity = _applicationDbContext.Games
                .Include(i => i.Players)
                .Include(i => i.CommanderDamages)
                .Include(i => i.CardHistory)
                .First(i => i.Id == id);

            gameEntity.Players = gameEntity.Players.OrderBy(i => i.TurnOrder).ToList();
            gameEntity.CardHistory = gameEntity.CardHistory.OrderByDescending(i => i.DateTimeAdded).ToList();
            return _mapper.Map<GameInfo>(gameEntity);
        }

        public List<PlayerInfo> GetPlayers(int gameId)
        {
            var playerEntities = _applicationDbContext.Players.Where(i => i.GameId == gameId);

            return _mapper.Map<List<PlayerInfo>>(playerEntities);
        }

        public void UpdateGameInfo(GameInfo gameInfo)
        {
            var gameInfoEntity = _applicationDbContext.Games.First(i => i.Id == gameInfo.Id);

            _mapper.Map(gameInfo, gameInfoEntity);

            _applicationDbContext.SaveChanges();
        }

        public void UpdateFocusedPlayer(int playerId, int focusedPlayerId)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.Id == playerId);

            if (playerEntity != null)
            {
                playerEntity.FocusedPlayerId = focusedPlayerId;
                _applicationDbContext.SaveChanges();
            }
        }

        public PlayerInfo? GetPlayer(string userId)
        {
            var playerInfoEntity = _applicationDbContext.Players.FirstOrDefault(i => i.UserId == userId);

            if (playerInfoEntity != null)
            {
                return _mapper.Map<PlayerInfo>(playerInfoEntity);
            }

            return null;
        }

        public void AdjustLifeTotal(int playerId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.First(i => i.Id == playerId);

            playerEntity.LifeTotal += amount;
            _applicationDbContext.SaveChanges();
        }

        public void UpdatePeerId(int playerId, Guid peerId)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.Id == playerId);

            if(playerEntity != null)
            {
                playerEntity.PeerId = peerId;
                _applicationDbContext.SaveChanges();
            }
        }

        public void UpdateCommander(int playerId, int slot, string cardId)
        {
            var playerEntity = _applicationDbContext.Players.First(i => i.Id == playerId);

            switch (slot)
            {
                case 1:
                    playerEntity.Commander1Id = cardId;
                    break;
                case 2:
                    playerEntity.Commander2Id = cardId;
                    break;
            }

            _applicationDbContext.SaveChanges();
        }

        public void AdjustInfectCount(int playerId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.First(i => i.Id == playerId);

            playerEntity.InfectCount += amount;
            _applicationDbContext.SaveChanges();
        }

        public void AdjustCommanderDamage(int gameId, int playerId, int ownerPlayerId, string cardId, int amount)
        {
            var commanderDamageEntity = _applicationDbContext.CommanderDamages.FirstOrDefault(i =>
                i.GameId == gameId && i.PlayerId == playerId && i.OwnerPlayerId == ownerPlayerId && i.CardId == cardId);

            if (commanderDamageEntity == null)
            {
                commanderDamageEntity = new Data.Application.Models.CommanderDamage
                {
                    GameId = gameId,
                    PlayerId = playerId,
                    OwnerPlayerId = ownerPlayerId,
                    CardId = cardId
                };

                _applicationDbContext.CommanderDamages.Add(commanderDamageEntity);
            }

            commanderDamageEntity.Damage += amount;

            _applicationDbContext.SaveChanges();
        }

        public void UpdatePlayerTurnOrder(int gameId, List<int> playerIds)
        {
            var playerEntities = _applicationDbContext.Players.Where(i => i.GameId == gameId);

            if (playerIds.Count != playerEntities.Count()) throw new ArgumentException(
                $"playerIds contains {playerIds.Count} while there were {playerEntities.Count()} players found for game Id {gameId}");

            for (int i = 0; i < playerIds.Count; i++)
            {
                var playerEntity = playerEntities.FirstOrDefault(p => p.Id == playerIds[i]);
                if (playerEntity != null)
                {
                    playerEntity.TurnOrder = i;
                }
                else
                {
                    throw new ArgumentException($"No player with Id {playerIds[i]} exists");
                }
            }
            _applicationDbContext.SaveChanges();
        }

        public void UpdateGameOwner(int gameid, string userId)
        {
            var gameInfoEntity = _applicationDbContext.Games.First(i => i.Id == gameid);
            gameInfoEntity.OwnerUserId = userId;
            _applicationDbContext.SaveChanges();
        }

        public void DeletePlayer(int playerId)
        {
            var playerEntity = _applicationDbContext.Players.First(i => i.Id == playerId);
            _applicationDbContext.Players.Remove(playerEntity);
            _applicationDbContext.SaveChanges();
        }

        public void DeleteGame(int gameId)
        {
            var gameEntity = _applicationDbContext.Games.FirstOrDefault(i => i.Id == gameId);

            if (gameEntity != null)
            {
                _applicationDbContext.Games.Remove(gameEntity);
                _applicationDbContext.SaveChanges();
            }
        }

        public void AddCardToCardHistory(int gameId, int playerId, DateTime dateTimeAdded, string cardId)
        {
            _applicationDbContext.CardHistory.Add(new Data.Application.Models.CardHistoryEntry
            {
                GameId = gameId,
                PlayerId = playerId,
                DateTimeAdded = dateTimeAdded,
                CardId = cardId
            });

            _applicationDbContext.SaveChanges();
        }

        public void RemoveCardFromCardHistory(int id)
        {
            var cardHistoryEntity = _applicationDbContext.CardHistory.First(i=>i.Id == id);
            _applicationDbContext.CardHistory.Remove(cardHistoryEntity);
            _applicationDbContext.SaveChanges();
        }
    }
}