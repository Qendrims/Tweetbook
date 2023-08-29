using AutoMapper;
using TwitterBook.Contracts.V1.Requests.Queries;
using TwitterBook.Domain;

namespace TwitterBook.MappingProfiles;

public class RequestToDomainProfile : Profile
{
    public RequestToDomainProfile()
    {
        CreateMap<PaginationQuery, PaginationFilter>();
    }
}