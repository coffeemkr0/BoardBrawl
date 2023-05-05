using AutoMapper;
using IMapper = SpellTable2.Core.AutoMapping.IMapper;

namespace SpellTable2.WebApp.MVC.AutoMapping
{
    public class AutoMapperMapper : IMapper
    {
        private readonly AutoMapper.IMapper _mapper;

        public AutoMapperMapper()
        {
            var configurationExpression = new MapperConfigurationExpression();

            configurationExpression.AddProfile<Services.Lobby.AutoMapperProfile>();
            configurationExpression.AddProfile<Services.Game.AutoMapperProfile>();
            configurationExpression.AddProfile<Areas.Game.AutoMapperProfile>();

            _mapper = new Mapper(new MapperConfiguration(configurationExpression));
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
