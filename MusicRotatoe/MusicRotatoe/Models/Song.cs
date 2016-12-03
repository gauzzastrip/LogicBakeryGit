using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRotatoe.Models
{
    public class Song
    {
        public string File { get; set; }
        public string Title { get; set; }
        public string YoutubeId { get; set; }
        public bool Keep { get; set; }
        public string SpotifyArtistId { get; set; }
        public string SpotifyTrackId { get; set; }
        public string Artist { get; set; }
    }
}
