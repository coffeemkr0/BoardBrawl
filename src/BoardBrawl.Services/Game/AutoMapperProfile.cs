using AutoMapper;
using BoardBrawl.Repositories.Game.Models;

namespace BoardBrawl.Services.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.GameInfo, Repositories.Game.Models.GameInfo>();
            CreateMap<Repositories.Game.Models.GameInfo, Models.GameInfo>();

            CreateMap<Repositories.Game.Models.PlayerInfo, Models.PlayerInfo>();
            CreateMap<Models.PlayerInfo, Repositories.Game.Models.PlayerInfo>();

            CreateMap<Repositories.Game.Models.Commander, Models.Commander>()
                .ForMember(d => d.Colors,
                s => s.MapFrom(o => MapColorsFromString(o.Colors)));
            CreateMap<Models.Commander, Repositories.Game.Models.Commander>()
                .ForMember(d => d.Colors,
                s => s.MapFrom(o => MapColorsToString(o.Colors)));
        }

        public static List<Colors> MapColorsFromString(string colorString)
        {
            var colors = new List<Colors>();

            foreach (var item in colorString.Split(','))
            {
                colors.Add((Colors)Enum.Parse(typeof(Colors), item));
            }

            return colors;
        }

        public static string MapColorsToString(List<Colors> colors)
        {
            return string.Join(",", colors.Select(c => (int)c));
        }
    }
}