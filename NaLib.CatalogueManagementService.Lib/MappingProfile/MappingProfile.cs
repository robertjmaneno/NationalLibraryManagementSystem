using AutoMapper;
using NaLib.CatalogueManagementService.Lib.Dto;
using NaLib.CatalogueManagementService.Lib.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LibraryResource, LibraryResourceDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.ResourceType, opt => opt.MapFrom(src => src.ResourceType))
            .ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.Format))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres))
            .ForMember(dest => dest.IsBorrowable, opt => opt.MapFrom(src => src.IsBorrowable))
            .ForMember(dest => dest.BorrowLimitInDays, opt => opt.MapFrom(src => src.BorrowRules.BorrowLimitInDays))
            .ForMember(dest => dest.CatalogedBy, opt => opt.MapFrom(src => src.CatalogedBy))
            .ForMember(dest => dest.BorrowStatus, opt => opt.MapFrom(src => src.BorrowStatus.ToString()));

        CreateMap<CreateLibraryResourceDto, LibraryResource>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.ResourceType, opt => opt.MapFrom(src => src.ResourceType))
            .ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.Format))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres))
            .ForMember(dest => dest.CatalogedBy, opt => opt.MapFrom(src => src.CatalogedBy))
            .ForMember(dest => dest.BorrowStatus, opt => opt.MapFrom(src => BorrowStatus.Available))
            .ForMember(dest => dest.IsBorrowable, opt => opt.Ignore())
            .ForMember(dest => dest.BorrowRules, opt => opt.Ignore())
            .ReverseMap();


        CreateMap<UpdateLibraryResourceDto, LibraryResource>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdString, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.ResourceType, opt => opt.MapFrom(src => src.ResourceType))
            .ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.Format))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres))
            .ForMember(dest => dest.IsBorrowable, opt => opt.MapFrom(src => src.IsBorrowable))
            .ForMember(dest => dest.CatalogedBy, opt => opt.MapFrom(src => src.CatalogedBy))
            .ForMember(dest => dest.BorrowStatus, opt => opt.MapFrom<BorrowStatusResolver>());

    }
}
