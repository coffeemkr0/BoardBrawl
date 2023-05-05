using AutoMapper;

namespace SpellTable2.WebApp.MVC.Areas.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Services.Game.Models.PlayerInfo, Models.PlayerInfo>();
        }
    }
}