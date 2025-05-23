namespace GhiblipediaAPI.Models
{
    public class Movie
    {
        public int? Movie_id { get; set; } // Auto-increment by default.
        public string? English_title { get; set; }
        public string? Japanese_title { get; set; }        
        public int? Release_year { get; set; }
        public string? Image_url { get; set; }
        public string? Trailer_url { get; set; }
        public string? Summary { get; set; }
        public string? Plot { get; set; }
        public string? Director { get; set; }
        public string? Genre { get; set; }
        public int? Running_time_mins { get; set; }
    }
}
