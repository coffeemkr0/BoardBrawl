using SpellTable2.Core.AutoMapping;
using SpellTable2.Repositories.Lobby;
using SpellTable2.Services.Lobby.Models;

namespace SpellTable2.Services.Lobby
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
    }
}