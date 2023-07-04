﻿using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Data.Application.Models;
using BoardBrawl.Repositories.Game;
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public class Service : IService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public Service(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public GameInfo? GetGameInfo(int id, string userId)
        {
            var repoGameInfo = _repository.GetGameInfo(id);
            if (repoGameInfo != null)
            {
                var gameInfo = _mapper.Map<GameInfo>(repoGameInfo);

                var myPlayer = gameInfo.Players.FirstOrDefault(i => i.UserId == userId);
                if(myPlayer != null) { myPlayer.IsSelf = true; }

                return gameInfo;
            }
            else
            {
                return null;
            }
        }

        public PlayerInfo IncreaseInfectDamage(int gameId, string userId, int amount)
        {
            _repository.IncreaseInfectDamage(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public PlayerInfo DecreaseInfectDamage(int gameId, string userId, int amount)
        {
            _repository.DecreaseInfectDamage(gameId, userId, amount);
            return _mapper.Map<PlayerInfo>(_repository.GetPlayers(gameId).First(i => i.UserId == userId));
        }

        public void PassTurn(int gameId)
        {
            var repoGameInfo = _repository.GetGameInfo(gameId);

            if (repoGameInfo == null) throw new Exception($"Game not found with id {gameId}");
          
            var now = DateTime.Now;

            //Start game if it hasn't started
            if (repoGameInfo.GameStart == null)
            {
                repoGameInfo.GameStart = now;
            }

            //Set active player to next player in list
            if (repoGameInfo.ActivePlayerId == null)
            {
                repoGameInfo.ActivePlayerId = repoGameInfo.Players.First().Id;
            }
            else
            {
                var currentPlayerIndex = repoGameInfo.Players.FindIndex(i => i.Id == repoGameInfo.ActivePlayerId);
                int nextPlayerIndex = (currentPlayerIndex + 1) % repoGameInfo.Players.Count;
                repoGameInfo.ActivePlayerId = repoGameInfo.Players[nextPlayerIndex].Id;
            }

            //Start the turn
            repoGameInfo.TurnStart = now;

            _repository.UpdateGameInfo(repoGameInfo);
        }

        public void UpdateFocusedPlayer(int playerId, int focusedPlayerId)
        {
            _repository.UpdateFocusedPlayer(playerId, focusedPlayerId);
        }

        public PlayerInfo AdjustLifeTotal(int playerId, int amount)
        {
            return _mapper.Map<PlayerInfo>(_repository.AdjustLifeTotal(playerId, amount));
        }

        public void UpdatePeerId(int playerId, Guid peerId)
        {
            _repository.UpdatePeerId(playerId, peerId);
        }

        public void JoinGame(int gameId, string userId)
        {
            var repoPlayerInfo = _repository.GetPlayer(userId);

            if (repoPlayerInfo == null)
            {
                //The user does not have a player in a game, create a new one
                var newPlayer = CreateNewPlayer(userId);
                repoPlayerInfo = _mapper.Map<Repositories.Game.Models.PlayerInfo>(newPlayer);
            }

            _repository.AddPlayerToGame(gameId, repoPlayerInfo);
        }

        private PlayerInfo CreateNewPlayer(string userId)
        {
            //TODO:Get default player name from user preferences
            return new PlayerInfo
            {
                UserId = userId,
                Name = $"Player {userId.ToString()[..5]}",
                LifeTotal = 40
            };
        }

        public void UpdateCommander(int playerId, int slot, string cardId)
        {
            _repository.UpdateCommander(playerId, slot, cardId);
        }
    }
}