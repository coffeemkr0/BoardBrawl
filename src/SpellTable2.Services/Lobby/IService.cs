using SpellTable2.Services.Lobby.Models;

namespace SpellTable2.Services.Lobby
{
    public interface IService
    {
        List<GameInfo> GetPublicGames();
        void CreateGame(GameInfo gameInfo);
    }
}