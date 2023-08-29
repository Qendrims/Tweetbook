using AutoMapper;
using Swashbuckle.Swagger.Model;
using TwitterBook.Contracts.V1.Response;
using TwitterBook.Domain;

namespace TwitterBook.MappingProfiles;

public class DomainToResponseProfile : Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<Post, PostResponse>()
            .ForMember(dest => dest.PostTags,
                opt => opt.MapFrom(
                    src => src.PostTags.Select(x => new TagResponse { TagName = x.TagName })));

        CreateMap<Tags, TagResponse>();
    }
}