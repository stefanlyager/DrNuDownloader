namespace DrNuDownloader.Mappers
{
    public interface IMapper<in TFrom, out TTo>
    {
        TTo Map(TFrom input);
    }
}