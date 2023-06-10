using BoardBrawl.Core.AutoMapping;
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

        public void AddPlayerToGame(int gameId, PlayerInfo playerInfo)
        {
            _repository.AddPlayerToGame(gameId, _mapper.Map< Repositories.Game.Models.PlayerInfo >(playerInfo));
        }

        public void ClearPlayers()
        {
            _repository.ClearPlayers();
        }

        public PlayerInfo DecreaseLifeTotal(int gameId, Guid userId, int amount)
        {
            _repository.DecreaseLifeTotal(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public GameInfo? GetGameInfo(int id)
        {
            var repoGameInfo = _repository.GetGameInfo(id);
            if (repoGameInfo != null)
            {
                return _mapper.Map<GameInfo>(repoGameInfo);
            }
            else
            {
                return null;
            }
        }

        public List<PlayerInfo> GetPlayers(int gameId)
        {
            var repoPlayers = _repository.GetPlayers(gameId);
            return _mapper.Map<List<PlayerInfo>>(repoPlayers);
        }

        public PlayerInfo IncreaseLifeTotal(int gameId, Guid userId, int amount)
        {
            _repository.IncreaseLifeTotal(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public void UpdatePeerId(int gameId, Guid userId, Guid peerId)
        {
            _repository.UpdatePeerId(gameId, userId, peerId);
        }

        public PlayerInfo IncreaseCommanderDamage(int gameId, Guid userId, int amount)
        {
            _repository.IncreaseCommanderDamage(gameId, userId, amount);
            var repoPlayer = _repository.GetPlayers(gameId).First(i => i.UserId == userId);
            var servicePlayer = _mapper.Map<PlayerInfo>(repoPlayer);

            return servicePlayer;
        }

        public PlayerInfo IncreaseInfectDamage(int gameId, Guid userId, int amount)
        {
            _repository.IncreaseInfectDamage(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public PlayerInfo DecreaseCommanderDamage(int gameId, Guid userId, int amount)
        {
            _repository.DecreaseCommanderDamage(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public PlayerInfo DecreaseInfectDamage(int gameId, Guid userId, int amount)
        {
            _repository.DecreaseInfectDamage(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }
    }
}