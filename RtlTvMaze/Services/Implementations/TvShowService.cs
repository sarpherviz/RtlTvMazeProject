using RtlTvMaze.Domain;
using RtlTvMaze.Model;
using RtlTvMaze.Repository.Interface;

namespace RtlTvMaze.Services.Implementations
{
    public class TvShowService : ITvShowService
    {
        private readonly ITvShowsRepository _tvShowRepository;

        public TvShowService(ITvShowsRepository tvShowRepository)
        {
            _tvShowRepository = tvShowRepository;
        }

        public async Task<PagedList<TvShowModel>> Get(string? searchTerm, int page, int size)
        {
            System.Linq.Expressions.Expression<Func<TvShows, bool>> predicate = x => x.IsDeleted == false;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                predicate = x => x.IsDeleted == false && x.ShowName.ToLower().Contains(string.IsNullOrEmpty(searchTerm) ? x.ShowName : searchTerm.ToLower());
            }
            var totalCount = await _tvShowRepository.GetListCountAsync(predicate);
            List<TvShows> data = await _tvShowRepository.GetListWithPaginationAsync(predicate, page, size);
            return await PagedList<TvShowModel>.CreateAsync(TvShowModelMapping.toTvShowModelList(data), totalCount, page, size);
        }
    }
}