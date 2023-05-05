
using SpellTable2.Services.Game.Models;

namespace SpellTable2.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(Guid id);
        void AddPlayerToGame(Guid gameId, PlayerInfo playerInfo);
        List<PlayerInfo> GetPlayers(Guid gameId);
    }
}