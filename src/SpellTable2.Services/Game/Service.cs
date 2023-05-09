using SpellTable2.Core.AutoMapping;
using SpellTable2.Repositories.Game;
using SpellTable2.Services.Game.Models;

namespace SpellTable2.Services.Game
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

        public void AddPlayerToGame(Guid gameId, PlayerInfo playerInfo)
        {
            _repository.AddPlayerToGame(gameId, _mapper.Map<Repositories.Game.Models.PlayerInfo>(playerInfo));
        }

        public PlayerInfo DecreaseLifeTotal(Guid gameId, Guid userId, int amount)
        {
            _repository.DecreaseLifeTotal(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public GameInfo? GetGameInfo(Guid id)
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

        public List<PlayerInfo> GetPlayers(Guid gameId)
        {
            var repoPlayers = _repository.GetPlayers(gameId);
            return _mapper.Map<List<PlayerInfo>>(repoPlayers);
        }

        public PlayerInfo IncreaseLifeTotal(Guid gameId, Guid userId, int amount)
        {
            _repository.IncreaseLifeTotal(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public void UpdatePeerId(Guid gameId, Guid userId, Guid peerId)
        {
            _repository.UpdatePeerId(gameId, userId, peerId);
        }
    }
}