using AutoMapper;
using NewZealandWalksAPI.Models.Domain;
using NewZealandWalksAPI.Models.DTO;

namespace NewZealandWalksAPI.Mappings
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();

        }
    }
}
