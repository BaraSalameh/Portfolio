using Application.Owner.Commands.BlogPostCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class BlogPostMappingProfiles : Profile
    {
        public BlogPostMappingProfiles()
        {
            CreateMap<AddEditBlogPostCommand, BlogPost>();
        }
    }
}
