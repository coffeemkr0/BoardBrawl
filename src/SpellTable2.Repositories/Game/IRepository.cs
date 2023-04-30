using SpellTable2.Repositories.Game.Models;

namespace SpellTable2.Repositories.Game
{
    public interface IRepository
    {
        GameInfo? GetGameInfo(Guid id);
        void CloseGame(GameInfo gameInfo);
        string GetPlayerName();
        void SetPlayerName(string playerName);
    }
}