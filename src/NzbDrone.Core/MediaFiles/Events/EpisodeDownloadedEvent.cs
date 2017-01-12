using System.Collections.Generic;
using NzbDrone.Common.Messaging;
using NzbDrone.Core.Parser.Model;

namespace NzbDrone.Core.MediaFiles.Events
{
    public class EpisodeDownloadedEvent : IEvent
    {
        public LocalMovie Movie { get; private set; }
        public MovieFile MovieFile { get; private set; }
        public List<MovieFile> OldFiles { get; private set; }

        public EpisodeDownloadedEvent(LocalMovie movie, MovieFile movieFile, List<MovieFile> oldFiles)
        {
            Movie = movie;
            MovieFile = movieFile;
            OldFiles = oldFiles;
        }
    }
}