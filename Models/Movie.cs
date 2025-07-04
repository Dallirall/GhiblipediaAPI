namespace GhiblipediaAPI.Models
{
    public class Movie
    {
        public int? MovieId { get; set; } // Auto-increment by default.
        public DateTime? CreatedAt { get; set; } //Do not set value manually. A timestamp is set automatically in the database.
        public string? EnglishTitle { get; set; }
        public string? JapaneseTitle { get; set; }        
        public string? ReleaseDate { get; set; }
        public string? ImageUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public string? Summary { get; set; }
        public string? Plot { get; set; }
        public string? Director { get; set; }
        public string? Genre { get; set; }
        public string? RunningTime { get; set; }
        public string[]? Tags { get; set; }
    }
}
