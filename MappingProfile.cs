﻿using AutoMapper;
using GhiblipediaAPI.Models;

namespace GhiblipediaAPI
{
    public class MappingProfile : Profile
    {
        //This is where I have mapped between the model dto classes.
        public MappingProfile()
        {
            CreateMap<MovieDtoGet, MovieGet>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Movie_id))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Created_at))
                .ForMember(dest => dest.EnglishTitle, opt => opt.MapFrom(src => src.English_title))
                .ForMember(dest => dest.JapaneseTitle, opt => opt.MapFrom(src => src.Japanese_title))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Release_date))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image_url))
                .ForMember(dest => dest.TrailerUrl, opt => opt.MapFrom(src => src.Trailer_url))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Plot, opt => opt.MapFrom(src => src.Plot))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.RunningTime, opt => opt.MapFrom(src => src.Running_time))
                .ForMember(dest => dest.Tags, opt => 
                {
                    opt.PreCondition(src => src.Tags != null);
                    opt.MapFrom(src => src.Tags);
                });

            CreateMap<MovieGet, MovieDtoGet>()
                .ForMember(dest => dest.Movie_id, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.Created_at, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.English_title, opt => opt.MapFrom(src => src.EnglishTitle))
                .ForMember(dest => dest.Japanese_title, opt => opt.MapFrom(src => src.JapaneseTitle))
                .ForMember(dest => dest.Release_date, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.Image_url, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Trailer_url, opt => opt.MapFrom(src => src.TrailerUrl))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Plot, opt => opt.MapFrom(src => src.Plot))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Running_time, opt => opt.MapFrom(src => src.RunningTime))
                .ForMember(dest => dest.Tags, opt =>
                {
                    opt.PreCondition(src => src.Tags != null);
                    opt.MapFrom(src => src.Tags);
                });

            CreateMap<MovieDtoPostPut, MoviePostPut>()                
                .ForMember(dest => dest.EnglishTitle, opt => opt.MapFrom(src => src.English_title))
                .ForMember(dest => dest.JapaneseTitle, opt => opt.MapFrom(src => src.Japanese_title))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Release_date))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image_url))
                .ForMember(dest => dest.TrailerUrl, opt => opt.MapFrom(src => src.Trailer_url))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Plot, opt => opt.MapFrom(src => src.Plot))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.RunningTime, opt => opt.MapFrom(src => src.Running_time))
                .ForMember(dest => dest.Tags, opt =>
                {
                    opt.PreCondition(src => src.Tags != null);
                    opt.MapFrom(src => src.Tags);
                });

            CreateMap<MoviePostPut, MovieDtoPostPut>()                
                .ForMember(dest => dest.English_title, opt => opt.MapFrom(src => src.EnglishTitle))
                .ForMember(dest => dest.Japanese_title, opt => opt.MapFrom(src => src.JapaneseTitle))
                .ForMember(dest => dest.Release_date, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.Image_url, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Trailer_url, opt => opt.MapFrom(src => src.TrailerUrl))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Plot, opt => opt.MapFrom(src => src.Plot))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Running_time, opt => opt.MapFrom(src => src.RunningTime))
                .ForMember(dest => dest.Tags, opt =>
                {
                    opt.PreCondition(src => src.Tags != null);
                    opt.MapFrom(src => src.Tags);
                });

            CreateMap<MovieGet, MoviePostPut>()                
                .ForSourceMember(src => src.MovieId, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.CreatedAt, opt => opt.DoNotValidate())
                .ForMember(dest => dest.Tags, opt =>
                {
                    opt.PreCondition(src => src.Tags != null);
                    opt.MapFrom(src => src.Tags);
                });

            CreateMap<OmdbMovie, MoviePostPut>()                
                .ForMember(dest => dest.EnglishTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Released))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Poster))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Plot))
                .ForMember(dest => dest.Plot, opt => opt.MapFrom(src => src.FullPlot))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.RunningTime, opt => opt.MapFrom(src => src.Runtime))
                ;
        }
    }
}
