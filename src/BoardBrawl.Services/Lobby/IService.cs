using BoardBrawl.Services.Lobby.Models;

namespace BoardBrawl.Services.Lobby
{
    public interface IService
    {
        List<GameInfo> GetGames(string userId);
        void CreateGame(GameInfo gameInfo);
        void DeleteGame(int gameId);
    }
}