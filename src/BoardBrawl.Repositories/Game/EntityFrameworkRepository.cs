using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Data.Application;
using BoardBrawl.Repositories.Game.Models;

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
        }

        public void CloseGame(GameInfo gameInfo)
        {
            var gameEntity = _applicationDbContext.Games.FirstOrDefault(i => i.Id == gameInfo.Id);

            if (gameEntity != null)
            {
                _applicationDbContext.Games.Remove(gameEntity);
                _applicationDbContext.SaveChanges();
            }
        }

        public void DecreaseLifeTotal(int gameId, string userId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.GameId == gameId && i.UserId == userId);

            if (playerEntity != null)
            {
                playerEntity.LifeTotal -= amount;
                _applicationDbContext.SaveChanges();
            }
        }

        public GameInfo? GetGameInfo(int id)
        {
            var gameEntity = _applicationDbContext.Games.FirstOrDefault(i => i.Id == id);

            if (gameEntity != null)
            {
                return _mapper.Map<GameInfo>(gameEntity);
            }

            return null;
        }

        public List<PlayerInfo> GetPlayers(int gameId)
        {
            var playerEntities = _applicationDbContext.Players.Where(i => i.GameId == gameId);

            return _mapper.Map<List<PlayerInfo>>(playerEntities);
        }

        public void IncreaseLifeTotal(int gameId, string userId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.GameId == gameId && i.UserId == userId);

            if (playerEntity != null)
            {
                playerEntity.LifeTotal += amount;
                _applicationDbContext.SaveChanges();
            }
        }

        public void UpdatePeerId(int gameId, string userId, Guid peerId)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.GameId == gameId && i.UserId == userId);

            if (playerEntity != null)
            {
                playerEntity.PeerId = peerId;
                _applicationDbContext.SaveChanges();
            }
        }


        public void ClearPlayers()
        {
            _applicationDbContext.Players.RemoveRange(_applicationDbContext.Players);
            _applicationDbContext.SaveChanges();
        }

        public void DecreaseCommanderDamage(int gameId, string userId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.GameId == gameId && i.UserId == userId);

            if (playerEntity != null)
            {
                playerEntity.CommanderDamage -= amount;
                _applicationDbContext.SaveChanges();
            }
        }

        public void IncreaseCommanderDamage(int gameId, string userId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.GameId == gameId && i.UserId == userId);

            if (playerEntity != null)
            {
                playerEntity.CommanderDamage += amount;
                _applicationDbContext.SaveChanges();
            }
        }

        public void DecreaseInfectDamage(int gameId, string userId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.GameId == gameId && i.UserId == userId);

            if (playerEntity != null)
            {
                playerEntity.InfectDamage -= amount;
                _applicationDbContext.SaveChanges();
            }
        }

        public void IncreaseInfectDamage(int gameId, string userId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.GameId == gameId && i.UserId == userId);

            if (playerEntity != null)
            {
                playerEntity.InfectDamage += amount;
                _applicationDbContext.SaveChanges();
            }
        }

        public void UpdateGameInfo(GameInfo gameInfo)
        {
            var gameInfoEntity = _applicationDbContext.Games.First(i => i.Id == gameInfo.Id);

            _mapper.Map(gameInfo, gameInfoEntity);

            _applicationDbContext.SaveChanges();
        }
    }
}