namespace RtlTvMaze.Model
{
    public class TvShowModel
    {
        public int ShowId { get; set; }
        public string? ShowName { get; set; }
        public List<CastModel>? Cast { get; set; }
    }
}
