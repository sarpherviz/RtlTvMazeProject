using RtlTvMaze.Domain;
using RtlTvMaze.Model;

namespace RtlTvMaze.Services
{
    public interface ITvShowService
    {
        Task<PagedList<TvShowModel>> Get(string? searchTerm, int page, int size);
    }
}