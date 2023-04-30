using SpellTable2.Repositories.Game;
using SpellTable2.Repositories.Game.Models;

namespace SpellTable2.Services.Game
{
    public class Service : IService
    {
        private readonly IRepository _repository;

        public Service(IRepository repository)
        {
            _repository = repository;
        }

        public GameInfo? GetGameInfo(Guid id)
        {
            var repoGameInfo = _repository.GetGameInfo(id);
            if (repoGameInfo != null)
            {
                return new GameInfo
                {
                    GameId = id,
                    Name = repoGameInfo.Name,
                    Description = repoGameInfo.Description,
                    IsPublic = repoGameInfo.IsPublic,
                };
            }
            else
            {
                return null;
            }
        }
    }
}