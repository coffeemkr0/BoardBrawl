
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(int id);
        void AddPlayerToGame(int gameId, PlayerInfo playerInfo);
        List<PlayerInfo> GetPlayers(int gameId);
        void UpdatePeerId(int gameId, Guid userId, Guid peerId);
        PlayerInfo DecreaseLifeTotal(int gameId, Guid userId, int amount);
        PlayerInfo IncreaseLifeTotal(int gameId, Guid userId, int amount);
        void ClearPlayers();
        PlayerInfo DecreaseCommanderDamage(int gameId, Guid userId, int amount);
        PlayerInfo IncreaseCommanderDamage(int gameId, Guid userId, int amount);
        PlayerInfo DecreaseInfectDamage(int gameId, Guid userId, int amount);
        PlayerInfo IncreaseInfectDamage(int gameId, Guid userId, int amount);
    }
}