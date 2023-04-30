using SpellTable2.Services.Lobby.Models;

namespace SpellTable2.Services.Lobby
{
    public interface IService
    {
        void CreateGame(GameInfo gameInfo);
    }
}