namespace BoardBrawl.Core.AutoMapping
{
    public interface IMapper
    {
        TDestination Map<TDestination>(object source);
        TDestination Map<TDestination>(object source, TDestination destination);
    }
}
