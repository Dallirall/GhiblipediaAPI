using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GhiblipediaAPI.Models
{
    //Data model for HttpPost requests (where english title is required).
    public class MovieCreate
    {
        private string[]? _tags;

        [Required]
        [JsonPropertyName("english_title")]
        public string? EnglishTitle { get; set; }
        [JsonPropertyName("japanese_title")]
        public string? JapaneseTitle { get; set; }
        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }
        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }
        [JsonPropertyName("trailer_url")]
        public string? TrailerUrl { get; set; }
        [JsonPropertyName("summary")]
        public string? Summary { get; set; }
        [JsonPropertyName("plot")]
        public string? Plot { get; set; }
        [JsonPropertyName("director")]
        public string? Director { get; set; }
        [JsonPropertyName("genre")]
        public string? Genre { get; set; }
        [JsonPropertyName("running_time")]
        public string? RunningTime { get; set; }

        /*When mapping from the JSON body, an unassigned 'Tags' field is stored as an empty array in the Tags property,
        which is then stored in the database, instead of leaving the column as null. 
        To reduce complications I therefore null the value in these cases.*/
        [JsonPropertyName("tags")]
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

