using NzbDrone.Core.Tv;

namespace NzbDrone.Core.Notifications.Webhook
{
    public class WebhookMovie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public int TmdbId { get; set; }

        public WebhookMovie() { }

        public WebhookMovie(Movie movie)
        {
            Id = movie.Id;
            Title = movie.Title;
            Path = movie.Path;
            TmdbId = movie.TmdbId;
        }
    }
}