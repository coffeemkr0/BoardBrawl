using AutoMapper;

namespace BoardBrawl.WebApp.MVC.Areas.Lobby
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Services.Lobby.Models.GameInfo, Models.GameInfo>();
        }
    }
}