using BoardBrawl.Repositories.Game.Models;

namespace BoardBrawl.Repositories.Game
{
    public interface IRepository
    {
        GameInfo GetGameInfo(int id);
        void UpdateGameInfo(GameInfo gameInfo);
        void CloseGame(GameInfo gameInfo);
        void AddPlayerToGame(int gameId, PlayerInfo playerInfo);            
        List<PlayerInfo> GetPlayers(int gameId);
        PlayerInfo? GetPlayer(string userId);
        void UpdateFocusedPlayer(int playerId, int focusedPlayerId);
        void UpdatePeerId(int playerId, Guid peerId);
        void AdjustLifeTotal(int playerId, int amount);
        void AdjustInfectCount(int playerId, int amount);
        void AdjustCommanderDamage(int gameId, int playerId, int ownerPlayerId, string cardId, int amount);

        void UpdateCommander(int playerId, int slot, string cardId);
        void UpdatePlayerTurnOrder(int gameId, List<int> playerIds);
        void UpdateGameOwner(int gameid, string userId);
    }
}