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

        public PlayerInfo DecreaseLifeTotal(int gameId, string userId, int amount)
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

        public PlayerInfo IncreaseLifeTotal(int gameId, string userId, int amount)
        {
            _repository.IncreaseLifeTotal(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public void UpdatePeerId(int gameId, string userId, Guid peerId)
        {
            _repository.UpdatePeerId(gameId, userId, peerId);
        }

        public PlayerInfo IncreaseCommanderDamage(int gameId, string userId, int amount)
        {
            _repository.IncreaseCommanderDamage(gameId, userId, amount);
            var repoPlayer = _repository.GetPlayers(gameId).First(i => i.UserId == userId);
            var servicePlayer = _mapper.Map<PlayerInfo>(repoPlayer);

            return servicePlayer;
        }

        public PlayerInfo IncreaseInfectDamage(int gameId, string userId, int amount)
        {
            _repository.IncreaseInfectDamage(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public PlayerInfo DecreaseCommanderDamage(int gameId, string userId, int amount)
        {
            _repository.DecreaseCommanderDamage(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public PlayerInfo DecreaseInfectDamage(int gameId, string userId, int amount)
        {
            _repository.DecreaseInfectDamage(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
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

            //Set active player to next player in list
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
    }
}