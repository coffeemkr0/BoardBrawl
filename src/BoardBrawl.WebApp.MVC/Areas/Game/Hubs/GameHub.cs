﻿using Microsoft.AspNetCore.SignalR;
using BoardBrawl.Services.Game;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Hubs
{
    public class GameHub : Hub
    {
        private readonly IService _service;

        public GameHub(IService service)
        {
            _service = service;
        }

        public async Task JoinGameHub(Guid gameId, Guid userId, Guid peerId)
        {
            _service.UpdatePeerId(gameId, userId, peerId);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
            await Clients.GroupExcept(gameId.ToString(), Context.ConnectionId).SendAsync("OnPlayerJoined", userId, peerId);
        }
    }
}