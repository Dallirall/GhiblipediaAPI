
﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace GhiblipediaAPI.Models
{
    //Data model for HttpGet requests.
    public class MovieResponse
    {           
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
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
