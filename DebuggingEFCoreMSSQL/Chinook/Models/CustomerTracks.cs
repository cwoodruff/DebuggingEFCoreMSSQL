using System;

namespace Chinook.Models
{
    public class CustomerTrack
    {
        public int CustomerId { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}