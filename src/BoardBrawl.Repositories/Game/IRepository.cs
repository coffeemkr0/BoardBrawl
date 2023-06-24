using BoardBrawl.Repositories.Game.Models;

namespace BoardBrawl.Repositories.Game
{
    public interface IRepository
    {
        GameInfo? GetGameInfo(int id);
        void UpdateGameInfo(GameInfo gameInfo);
        void CloseGame(GameInfo gameInfo);
        void AddPlayerToGame(int gameId, PlayerInfo playerInfo);            
        List<PlayerInfo> GetPlayers(int gameId);
        void UpdateFocusedPlayer(int playerId, int focusedPlayerId);
        void UpdatePeerId(int gameId, string userId, Guid peerId);
        void DecreaseLifeTotal(int gameId, string userId, int amount);
        void IncreaseLifeTotal(int gameId, string userId, int amount);
        void DecreaseCommanderDamage(int gameId, string userId, int amount);
        void IncreaseCommanderDamage(int gameId, string userId, int amount);
        void DecreaseInfectDamage(int gameId, string userId, int amount);
        void IncreaseInfectDamage(int gameId, string userId, int amount);
        void UpdateCommanders(int playerId, List<Commander> commanders);
    }
}