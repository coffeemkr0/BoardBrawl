using BoardBrawl.Services.Lobby.Models;

namespace BoardBrawl.Services.Lobby
{
    public interface IService
    {
        List<GameInfo> GetPublicGames();
        List<GameInfo> GetGames(Guid userId);
        void CreateGame(GameInfo gameInfo);
        void DeleteGame(Guid gameId);
    }
}