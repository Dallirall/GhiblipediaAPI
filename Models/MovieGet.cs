using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GhiblipediaAPI.Models
{
    //Data model for HttpGet requests from frontend.
    public class MovieGet
    {
        [JsonPropertyName("movie_id")]
        public int? MovieId { get; private set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
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
        [JsonPropertyName("tags")]
        public string[]? Tags { get; set; }
    }
}
