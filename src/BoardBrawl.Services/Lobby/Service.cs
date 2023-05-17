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
        }

        public List<GameInfo> GetPublicGames()
        {
            var repoGames = _repository.GetGames().Where(x => x.IsPublic);

            return _mapper.Map<List<GameInfo>>(repoGames);
        }
    }
}