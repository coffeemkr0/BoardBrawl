using AutoMapper;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Services.Game.Models.PlayerInfo, PlayerInfo>()
                .ForMember(
                    dest => dest.PlayerName,
                    opt => opt.MapFrom(src => src.Name)
                );

            CreateMap<Services.Game.Models.CommanderDamage, CommanderDamage>();
            CreateMap<Services.Game.Models.CardHistoryEntry, CardHistoryEntry>();
        }
    }
}