﻿using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Repositories.Lobby;
using BoardBrawl.Services.Lobby.Models;

namespace BoardBrawl.Services.Lobby
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

        public void CreateGame(GameInfo gameInfo)
        {
            var repoGameInfo = _mapper.Map<Repositories.Lobby.Models.GameInfo>(gameInfo);

            _repository.CreateGame(repoGameInfo);

            gameInfo.Id = repoGameInfo.Id;
        }

        public void DeleteGame(int gameId)
        {
            _repository.DeleteGame(gameId);
        }

        public List<GameInfo> GetGames(Guid userId)
        {
            var games = new List<GameInfo>();

            foreach (var repoGame in _repository.GetGames().Where(x => x.CreatedByUserId == userId))
            {
                var game = _mapper.Map<GameInfo>(repoGame);

                games.Add(game);
            }

            return games;
        }

        public void JoinGame(int gameId, Guid userId )
        {
            var playerInfo = new Repositories.Models.PlayerInfo
            {
                UserId = userId,
                //TODO:Get player info from repo
                PlayerName = "Hard coded player name",
                LifeTotal = 40
            };

            _repository.AddPlayerToGame(gameId, playerInfo.UserId);
        }
    }
}