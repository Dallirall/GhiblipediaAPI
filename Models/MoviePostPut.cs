using Newtonsoft.Json;

namespace GhiblipediaAPI.Models
{
    //Data model for HttpPost, HttpPut and HttpPatch requests from frontend.
    public class MoviePostPut
    {
        private string[]? _tags;

        [JsonProperty("english_title")]
        public string? EnglishTitle { get; set; }
        [JsonProperty("japanese_title")]
        public string? JapaneseTitle { get; set; }
        [JsonProperty("release_date")]
        public string? ReleaseDate { get; set; }
        [JsonProperty("image_url")]
        public string? ImageUrl { get; set; }
        [JsonProperty("trailer_url")]
        public string? TrailerUrl { get; set; }
        [JsonProperty("summary")]
        public string? Summary { get; set; }
        [JsonProperty("plot")]
        public string? Plot { get; set; }
        [JsonProperty("director")]
        public string? Director { get; set; }
        [JsonProperty("genre")]
        public string? Genre { get; set; }
        [JsonProperty("running_time")]
        public string? RunningTime { get; set; }

        /*When mapping from the JSON body, an empty 'Tags' field is stored as an empty array in the Tags property,
        which risks unintentional overwriting of existing values on HttpPut requests. 
        That's why I make sure to null the value in these cases.*/
        [JsonProperty("tags")]
        public string[]? Tags 
        { 
            get 
            { 
                return _tags; 
            } 
            set 
            {
                if (value == null || value is Array arr && arr.Length == 0)
                {
                    _tags = null;
                }
                else
                {
                    _tags = value;
                }
            }
        } 
    }
}

