using RtlTvMaze.Domain;

namespace RtlTvMaze.Model
{
    public static class TvShowModelMapping
    {
        public static List<TvShowModel> toTvShowModelList(List<TvShows> input)
        {
            List<TvShowModel> output = new List<TvShowModel>();
            foreach (var show in input)
            {
                output.Add(toTvShowModel(show));
            }
            return output;
        }

        public static TvShowModel toTvShowModel(TvShows input)
        {
            TvShowModel output = new TvShowModel();
            output.ShowId = input.ShowId;
            output.ShowName = input.ShowName;
            foreach (var cast in input.Cast.OrderByDescending(x => x.Birthday))
            {
                if (output.Cast == null)
                {
                    output.Cast = new List<CastModel>();
                }
                CastModel castModel = new CastModel();
                castModel.Id = cast.Id;
                castModel.Name = cast.Name;
                castModel.Birthday = cast.Birthday;
                output.Cast.Add(castModel);
            }
            return output;
        }
    }
}