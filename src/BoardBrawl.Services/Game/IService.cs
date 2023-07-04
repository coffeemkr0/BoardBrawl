﻿
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(int id, string userId);

        void PassTurn(int gameId);
        void JoinGame(int gameId, string userId);
        List<PlayerInfo> GetPlayers(int gameId);
        PlayerInfo GetPlayer(string userId);
        void UpdatePeerId(int playerId, Guid peerId);
        void UpdateFocusedPlayer(int playerId, int focusedPlayerId);
        PlayerInfo AdjustLifeTotal(int playerId, int amount);
        PlayerInfo DecreaseInfectDamage(int gameId, string userId, int amount);
        PlayerInfo IncreaseInfectDamage(int gameId, string userId, int amount);
        void UpdatePlayerInfo(PlayerInfo playerInfo);
        void UpdateCommander(int playerId, int slot, string cardId);
    }
}