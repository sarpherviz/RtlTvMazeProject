using System.Net;
using System.Text.Json;
using Polly;
using RtlTvMaze.BackgroundService.Interface;
using RtlTvMaze.BackgroundService.Model;
using RtlTvMaze.Domain;
using RtlTvMaze.Repository.Interface;

namespace RtlTvMaze.BackgroundService
{
    public class TvMazeBackgroundService : ITvMazeBackgroundService
    {
        private readonly ITvShowsRepository _tvShowRepository;
        private static readonly HttpClient httpClient = new HttpClient();


        public TvMazeBackgroundService(ITvShowsRepository tvShowRepository)
        {
            _tvShowRepository = tvShowRepository;
        }

        public async void AddTvMazeData()
        {
            TvShows latestShow = (await _tvShowRepository.GetListAsync(x=>x.IsDeleted==false)).OrderByDescending(x=>x.CreatedAt).FirstOrDefault();
            var retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(7));

            var allShowsResponse = await httpClient.GetAsync("https://api.tvmaze.com/updates/shows");
            var keys = new List<int>();
            if (allShowsResponse.IsSuccessStatusCode)
            {
                string responseBody = await allShowsResponse.Content.ReadAsStringAsync();
                var dictionary = JsonSerializer.Deserialize<Dictionary<string, long>>(responseBody);
                foreach (var key in dictionary.Keys)
                {
                    keys.Add(int.Parse(key));
                }
            }

            const int maxDegreeOfParallelism = 5;

            await Parallel.ForEachAsync(keys.Where(x=>x > latestShow.ShowId), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (key, cancellationToken) =>
            {
                var existingData = await _tvShowRepository.GetAsync(x => x.ShowId == key);
                if (existingData == null)
                {
                    var response = await retryPolicy.ExecuteAsync(() => httpClient.GetAsync($"https://api.tvmaze.com/shows/{key}?embed=cast"));
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        TvShows tvShow = TvMazeModelMapping.toTvShows(JsonSerializer.Deserialize<TvMazeModel>(responseBody));
                        await _tvShowRepository.AddAsync(tvShow);
                    }
                }
            });
        }
    }
}