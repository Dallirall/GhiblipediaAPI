﻿namespace GhiblipediaAPI.Models
{
    //Data model for objects retrieved from OMDb API.
    public class OmdbMovie
    {
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? Rated { get; set; }
        public string? Released { get; set; }
        public string? Runtime { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public string? Writer { get; set; }
        public string? Actors { get; set; }
        public string? Plot { get; set; }
        public string? FullPlot { get; set; }
        public string? Language { get; set; }
        public string? Country { get; set; }
        public string? Awards { get; set; }
        public string? Poster { get; set; }
        public List<Rating>? Ratings { get; set; }
        public string? Metascore { get; set; }
        public string? ImdbRating { get; set; }
        public string? ImdbVotes { get; set; }
        public string? ImdbID { get; set; }
        public string? Type { get; set; }
        public string? DVD { get; set; }
        public string? BoxOffice { get; set; }
        public string? Production { get; set; }
        public string? Website { get; set; }
        public string? Response { get; set; }
    }

    public class Rating
    {
        public string? Source { get; set; }
        public string? Value { get; set; }
    }
}
