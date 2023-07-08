using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Repositories.Lobby;
using BoardBrawl.Services.Lobby.Models;

namespace BoardBrawl.Services.Lobby
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

        public void CreateGame(GameInfo gameInfo)
        {
            var repoGameInfo = _mapper.Map<Repositories.Lobby.Models.GameInfo>(gameInfo);

            _repository.CreateGame(repoGameInfo);

            gameInfo.Id = repoGameInfo.Id;
        }

        public void DeleteGame(int gameId)
        {
            _repository.DeleteGame(gameId);
        }

        public List<GameInfo> GetGames(string userId)
        {
            var games = new List<GameInfo>();

            foreach (var repoGame in _repository.GetGames().Where(x => x.OwnerUserId == userId))
            {
                var game = _mapper.Map<GameInfo>(repoGame);

                games.Add(game);
            }

            return games;
        }
    }
}