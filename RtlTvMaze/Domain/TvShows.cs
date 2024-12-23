namespace RtlTvMaze.Domain
{
    public class TvShows : BaseEntity
    {
        public int ShowId { get; set; }
        public string? ShowName { get; set; }
        public List<Cast>? Cast { get; set; }
    }

    public class Cast
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Birthday { get; set; }
    }
}