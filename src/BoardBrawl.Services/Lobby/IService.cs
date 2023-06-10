using BoardBrawl.Services.Lobby.Models;

namespace BoardBrawl.Services.Lobby
{
    public interface IService
    {
        List<GameInfo> GetGames(Guid userId);
        void CreateGame(GameInfo gameInfo);
        void DeleteGame(int gameId);
        void JoinGame(int gameId, Guid userId);
    }
}