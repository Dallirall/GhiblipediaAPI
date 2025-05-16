namespace GhiblipediaAPI.Models
{
    public class Movie
    {
        public int Movie_id { get; set; }
        public string English_title { get; set; }
        public string? Japanese_title { get; set; }
        public string? Romaji_title { get; set; }
        public int? Release_year { get; set; }
        public string? Image_url { get; set; }
        public string? Trailer_url { get; set; }
        public string? Description_short { get; set; }
        public string? Description_long { get; set; }
        public string? Director { get; set; }
        public string? Genre { get; set; }
        public int? Running_time_mins { get; set; }
    }
}
