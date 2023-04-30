using SpellTable2.Repositories.Lobby;
using SpellTable2.Services.Lobby.Models;

namespace SpellTable2.Services.Lobby
{
    public class Service : IService
    {
        private readonly IRepository _repository;

        public Service(IRepository repository)
        {
            _repository = repository;
        }

        public void CreateGame(GameInfo gameInfo)
        {
            var repoGameInfo = new Repositories.Game.Models.GameInfo
            {
                GameId = gameInfo.GameId,
                Name = gameInfo.Name,
                Description = gameInfo.Description,
                IsPublic = gameInfo.IsPublic
            };

            _repository.CreateGame(repoGameInfo);
        }
    }
}