using SpellTable2.Repositories.Game;

namespace SpellTable2.Services.Game
{
    public class Service : IService
    {
        private readonly IRepository _repository;

        public Service(IRepository repository)
        {
            _repository = repository;
        }
    }
}