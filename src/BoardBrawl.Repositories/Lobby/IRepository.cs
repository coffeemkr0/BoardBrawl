using BoardBrawl.Repositories.Lobby.Models;

namespace BoardBrawl.Repositories.Lobby
{
    public interface IRepository
    {
        List<GameInfo> GetGames();
        void CreateGame(GameInfo gameInfo);
        void DeleteGame(int gameId);
        void AddPlayerToGame(int gameId, Guid playerId);
    }
}