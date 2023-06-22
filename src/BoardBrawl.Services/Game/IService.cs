
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(int id);
        void PassTurn(int gameId);
        void AddPlayerToGame(int gameId, PlayerInfo playerInfo);
        List<PlayerInfo> GetPlayers(int gameId);
        void UpdatePeerId(int gameId, string userId, Guid peerId);
        void UpdateFocusedPlayer(int playerId, int focusedPlayerId);
        PlayerInfo DecreaseLifeTotal(int gameId, string userId, int amount);
        PlayerInfo IncreaseLifeTotal(int gameId, string userId, int amount);
        PlayerInfo DecreaseCommanderDamage(int gameId, string userId, int amount);
        PlayerInfo IncreaseCommanderDamage(int gameId, string userId, int amount);
        PlayerInfo DecreaseInfectDamage(int gameId, string userId, int amount);
        PlayerInfo IncreaseInfectDamage(int gameId, string userId, int amount);
    }
}