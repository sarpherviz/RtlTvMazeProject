using RtlTvMaze.Domain;

namespace RtlTvMaze.BackgroundService.Model
{
    public static class TvMazeModelMapping
    {
        public static TvShows toTvShows(TvMazeModel model)
        {
            TvShows tvShows = new TvShows();
            tvShows.ShowId = model.id;
            tvShows.ShowName = model.name;
            if (model._embedded.cast != null)
            {
                tvShows.Cast = new List<Domain.Cast>();
                foreach (Model.Cast cast in model._embedded.cast.DistinctBy(y => y.person.id).OrderByDescending(x => x.person.birthday))
                {
                    Domain.Cast c = new Domain.Cast();
                    c.Id = cast.person.id;
                    c.Name = cast.person.name;
                    c.Birthday = Convert.ToDateTime(cast.person.birthday);
                    tvShows.Cast.Add(c);
                }
            }
            return tvShows;
        }
    }

}