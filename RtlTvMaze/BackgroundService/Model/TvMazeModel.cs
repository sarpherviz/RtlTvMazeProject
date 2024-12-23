namespace RtlTvMaze.BackgroundService.Model
{
    public class TvMazeModel
    {
        public int id { get; set; }
        public string? name { get; set; }
        public Embedded? _embedded { get; set; }
    }
    public class Cast
    {
        public Person? person { get; set; }
    }

    public class Embedded
    {
        public List<Cast>? cast { get; set; }
    }

    public class Person
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? birthday { get; set; }
    }

}