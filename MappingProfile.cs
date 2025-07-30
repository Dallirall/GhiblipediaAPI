using AutoMapper;
using GhiblipediaAPI.Models;

namespace GhiblipediaAPI
{
    public class MappingProfile : Profile
    {
        //For mapping between movie model classes using AutoMapper.
        public MappingProfile()
        {
            CreateMap<OmdbMovie, MovieCreate>()                
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
