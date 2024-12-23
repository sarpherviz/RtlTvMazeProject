namespace RtlTvMaze.Model
{
    public class PagedList<T>
    {

        public PagedList(List<T> data, long count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            Data = data;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public List<T> Data { get; set; }

        public static async Task<PagedList<T>> CreateAsync(List<T> source, long totalCount, int pageNumber, int pageSize)
        {
            return new PagedList<T>(source, totalCount, pageNumber, pageSize);
        }
    }
}