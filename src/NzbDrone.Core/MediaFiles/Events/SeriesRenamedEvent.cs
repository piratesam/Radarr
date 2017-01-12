using NzbDrone.Common.Messaging;
using NzbDrone.Core.Tv;

namespace NzbDrone.Core.MediaFiles.Events
{
    public class SeriesRenamedEvent : IEvent
    {
        public Movie Movie { get; private set; }

        public SeriesRenamedEvent(Movie movie)
        {
            Movie = movie;
        }
    }
}