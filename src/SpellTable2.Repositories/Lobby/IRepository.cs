using SpellTable2.Repositories.Lobby.Models;

namespace SpellTable2.Repositories.Lobby
{
    public interface IRepository
    {
        List<GameInfo> GetGames();
        void CreateGame(GameInfo gameInfo);
    }
}