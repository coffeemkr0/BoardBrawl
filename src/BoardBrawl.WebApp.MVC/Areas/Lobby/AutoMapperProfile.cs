using AutoMapper;
using BoardBrawl.WebApp.MVC.Areas.Lobby.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Lobby
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Services.Lobby.Models.GameInfo, GameInfo>();
            CreateMap<GameInfo, Services.Lobby.Models.GameInfo>();
        }
    }
}