using SpellTable2.Repositories.Game.Models;

namespace SpellTable2.Repositories.Game
{
    public interface IRepository
    {
        List<GameInfo> GetGames();
        void CreateGame(GameInfo gameInfo);
        void CloseGame(GameInfo gameInfo);
        string GetPlayerName();
        void SetPlayerName(string playerName);
    }
}