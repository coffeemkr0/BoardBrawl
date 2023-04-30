using SpellTable2.Repositories.Game.Models;

namespace SpellTable2.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(Guid id);
        string GetPlayerName();
        void SetPlayerName(string playerName);
    }
}