using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;

namespace TweetBook.MappingProfiles
{
    public class DomainToResponceProfile : Profile
    {
        public DomainToResponceProfile()
        {
            CreateMap<Post, PostResponse>();
        }
    }
}
