using AutoMapper;
using GhiblipediaAPI.Models;

namespace GhiblipediaAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieDto, Movie>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Movie_id))
                .ForMember(dest => dest.EnglishTitle, opt => opt.MapFrom(src => src.English_title))
                .ForMember(dest => dest.JapaneseTitle, opt => opt.MapFrom(src => src.Japanese_title))
                .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.Release_year))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image_url))
                .ForMember(dest => dest.TrailerUrl, opt => opt.MapFrom(src => src.Trailer_url))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Plot, opt => opt.MapFrom(src => src.Plot))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.RunningTimeMins, opt => opt.MapFrom(src => src.Running_time_mins))
                ;

            CreateMap<Movie, MovieDto>()  // Reverse mapping if needed
                .ForMember(dest => dest.Movie_id, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.English_title, opt => opt.MapFrom(src => src.EnglishTitle))
                .ForMember(dest => dest.Japanese_title, opt => opt.MapFrom(src => src.JapaneseTitle))
                .ForMember(dest => dest.Release_year, opt => opt.MapFrom(src => src.ReleaseYear))
                .ForMember(dest => dest.Image_url, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Trailer_url, opt => opt.MapFrom(src => src.TrailerUrl))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Plot, opt => opt.MapFrom(src => src.Plot))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Running_time_mins, opt => opt.MapFrom(src => src.RunningTimeMins))
                ;

        }
    }
}
