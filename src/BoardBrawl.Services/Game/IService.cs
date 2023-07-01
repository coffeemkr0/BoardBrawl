
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(int id);
        void PassTurn(int gameId);
        void AddPlayerToGame(int gameId, PlayerInfo playerInfo);
        List<PlayerInfo> GetPlayers(int gameId);
        PlayerInfo GetPlayer(string userId);
        void UpdatePeerId(int playerId, Guid peerId);
        void UpdateFocusedPlayer(int playerId, int focusedPlayerId);
        PlayerInfo AdjustLifeTotal(int playerId, int amount);
        PlayerInfo DecreaseCommanderDamage(int gameId, string userId, int amount);
        PlayerInfo IncreaseCommanderDamage(int gameId, string userId, int amount);
        PlayerInfo DecreaseInfectDamage(int gameId, string userId, int amount);
        PlayerInfo IncreaseInfectDamage(int gameId, string userId, int amount);
        void UpdatePlayerInfo(PlayerInfo playerInfo);
    }
}