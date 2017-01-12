using FluentValidation.Results;
using NzbDrone.Core.MediaFiles;
using NzbDrone.Core.Tv;
using NzbDrone.Core.Validation;
using NzbDrone.Core.Rest;
using RestSharp;
using NzbDrone.Core.Qualities;
using NzbDrone.Core.Parser.Model;
using System.Collections.Generic;

namespace NzbDrone.Core.Notifications.Webhook
{
    public interface IWebhookService
    {
        void OnDownload(Movie movie, WebhookSettings settings);
        void OnRename(Movie movie, WebhookSettings settings);
        void OnGrab(Movie movie, QualityModel quality, WebhookSettings settings);
        ValidationFailure Test(WebhookSettings settings);
    }

    public class WebhookService : IWebhookService
    {
        public void OnDownload(Movie movie, WebhookSettings settings)
        {
            var payload = new WebhookPayload
            {
                EventType = "Download",
                Movie = new WebhookMovie(movie)
                //Episodes = episodeFile.Episodes.Value.ConvertAll(x => new WebhookEpisode(x) {
                //    Quality = episodeFile.Quality.Quality.Name,
                //    QualityVersion = episodeFile.Quality.Revision.Version,
                //    ReleaseGroup = episodeFile.ReleaseGroup,
                //    SceneName = episodeFile.SceneName
                //})
            };

            NotifyWebhook(payload, settings);
        }

        public void OnRename(Movie movie, WebhookSettings settings)
        {
            var payload = new WebhookPayload
            {
                EventType = "Rename",
                Movie = new WebhookMovie(movie)
            };

            NotifyWebhook(payload, settings);
        }

        public void OnGrab(Movie movie, QualityModel quality, WebhookSettings settings)
        {
            var payload = new WebhookPayload
            {
                EventType = "Grab",
                Movie = new WebhookMovie(movie)
                //Episodes = episode.Episodes.ConvertAll(x => new WebhookEpisode(x)
                //{
                //    Quality = quality.Quality.Name,
                //    QualityVersion = quality.Revision.Version,
                //    ReleaseGroup = episode.ParsedEpisodeInfo.ReleaseGroup
                //})
            };
            NotifyWebhook(payload, settings);
        }

        public void NotifyWebhook(WebhookPayload body, WebhookSettings settings)
        {
            try {
                var client = RestClientFactory.BuildClient(settings.Url);
                var request = new RestRequest((Method) settings.Method);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(body);
                client.ExecuteAndValidate(request);
            }
            catch (RestException ex)
            {
                throw new WebhookException("Unable to post to webhook: {0}", ex, ex.Message);
            }
        }

        public ValidationFailure Test(WebhookSettings settings)
        {
            try
            {
                NotifyWebhook(
                    new WebhookPayload
                    {
                        EventType = "Test",
                        Movie = new WebhookMovie()
                        {
                            Id = 1,
                            Title = "Test Title",
                            Path = "C:\\testpath",
                            TmdbId = 1234
                        }
                        //Episodes = new List<WebhookEpisode>() {
                        //    new WebhookEpisode()
                        //    {
                        //        Id = 123,
                        //        EpisodeNumber = 1,
                        //        SeasonNumber = 1,
                        //        Title = "Test title"
                        //    }
                        //}
                    },
                    settings
                );
            }
            catch (WebhookException ex)
            {
                return new NzbDroneValidationFailure("Url", ex.Message);
            }

            return null;
        }
    }
}
