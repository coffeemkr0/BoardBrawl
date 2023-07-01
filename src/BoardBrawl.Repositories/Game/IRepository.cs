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
        PlayerInfo GetPlayer(string userId);
        void UpdateFocusedPlayer(int playerId, int focusedPlayerId);
        void UpdatePeerId(int gameId, string userId, Guid peerId);
        PlayerInfo AdjustLifeTotal(int playerId, int amount);
        void DecreaseCommanderDamage(int gameId, string userId, int amount);
        void IncreaseCommanderDamage(int gameId, string userId, int amount);
        void DecreaseInfectDamage(int gameId, string userId, int amount);
        void IncreaseInfectDamage(int gameId, string userId, int amount);
        void UpdatePlayerInfo(PlayerInfo playerInfo);
    }
}