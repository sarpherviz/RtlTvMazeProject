using Microsoft.Extensions.Options;
using RtlTvMaze.Data.Infrastructure;
using RtlTvMaze.Domain;
using RtlTvMaze.Repository.Interface;

namespace RtlTvMaze.Repository
{
    public class TvShowsRepository : RepositoryBase<TvShows>, ITvShowsRepository
    {
        public TvShowsRepository(IOptions<MongoDbSettings> options) : base(options)
        {
        }
    }
}