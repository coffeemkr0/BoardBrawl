using AutoMapper;

namespace BoardBrawl.WebApp.MVC.Areas.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Services.Game.Models.PlayerInfo, Models.PlayerInfo>()
                .ForMember(
                    dest => dest.PlayerName,
                    opt => opt.MapFrom(src => src.Name)
                );

            CreateMap<Services.Game.Models.CommanderDamage, Models.CommanderDamage>();
        }
    }
}