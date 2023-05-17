using AutoMapper;

namespace BoardBrawl.WebApp.MVC.Areas.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Services.Game.Models.PlayerInfo, Models.PlayerInfo>();
        }
    }
}