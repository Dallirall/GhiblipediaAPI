namespace GhiblipediaAPI.Models
{
    public class MovieDtoPostPut
    {        
        public string? English_title { get; set; }
        public string? Japanese_title { get; set; }
        public string? Release_date { get; set; }
        public string? Image_url { get; set; }
        public string? Trailer_url { get; set; }
        public string? Summary { get; set; }
        public string? Plot { get; set; }
        public string? Director { get; set; }
        public string? Genre { get; set; }
        public string? Running_time { get; set; }
        public string[]? Tags { get; set; }
    }
}
