using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace GhiblipediaAPI.Models
{
    //Data model for HttpGet requests from frontend.
    public class MovieGet
    {
        [JsonProperty("movie_id")]
        public int? MovieId { get; private set; }
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
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
        [JsonProperty("tags")]
        public string[]? Tags { get; set; }
    }
}
