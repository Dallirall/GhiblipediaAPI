namespace GhiblipediaAPI.Models
{
    public class MoviePostPut
    {
        private string[]? _tags;

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
        public string[]? Tags 
        { 
            get 
            { 
                return _tags; 
            } 
            set 
            {
                if (value == null || value is Array arr && arr.Length == 0)
                    _tags = null;
            }
        } 
    }
}

