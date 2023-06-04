
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(Guid id);
        void AddPlayerToGame(Guid gameId, PlayerInfo playerInfo);
        List<PlayerInfo> GetPlayers(Guid gameId);
        void UpdatePeerId(Guid gameId, Guid userId, Guid peerId);
        PlayerInfo DecreaseLifeTotal(Guid gameId, Guid userId, int amount);
        PlayerInfo IncreaseLifeTotal(Guid gameId, Guid userId, int amount);
        void ClearPlayers();
        PlayerInfo DecreaseCommanderDamage(Guid gameId, Guid userId, int amount);
        PlayerInfo IncreaseCommanderDamage(Guid gameId, Guid userId, int amount);
        PlayerInfo DecreaseInfectDamage(Guid gameId, Guid userId, int amount);
        PlayerInfo IncreaseInfectDamage(Guid gameId, Guid userId, int amount);
    }
}