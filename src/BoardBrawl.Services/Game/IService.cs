
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(int id);
        void StartGame(int gameId);
        void AddPlayerToGame(int gameId, PlayerInfo playerInfo);
        List<PlayerInfo> GetPlayers(int gameId);
        void UpdatePeerId(int gameId, string userId, Guid peerId);
        PlayerInfo DecreaseLifeTotal(int gameId, string userId, int amount);
        PlayerInfo IncreaseLifeTotal(int gameId, string userId, int amount);
        void ClearPlayers();
        PlayerInfo DecreaseCommanderDamage(int gameId, string userId, int amount);
        PlayerInfo IncreaseCommanderDamage(int gameId, string userId, int amount);
        PlayerInfo DecreaseInfectDamage(int gameId, string userId, int amount);
        PlayerInfo IncreaseInfectDamage(int gameId, string userId, int amount);
    }
}