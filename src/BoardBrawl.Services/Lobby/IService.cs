using BoardBrawl.Services.Lobby.Models;

namespace BoardBrawl.Services.Lobby
{
    public interface IService
    {
        List<GameInfo> GetPublicGames();
        void CreateGame(GameInfo gameInfo);
    }
}