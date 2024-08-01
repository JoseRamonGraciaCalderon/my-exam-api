using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands;
using Application.Queries;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo bidireccional entre User y UserDto
            CreateMap<User, UserDto>().ReverseMap();

            // Mapeo de CreateUserCommand a User
            CreateMap<CreateUserCommand, User>();

            // Mapeo de UpdateUserCommand a User
            CreateMap<UpdateUserCommand, User>();

            // Mapeo de User a UserDto en GetUserQuery
            CreateMap<User, GetUserQuery>().ReverseMap();

            // Mapeo de User a UserDto en GetUsersQuery
            CreateMap<User, GetUsersQuery>().ReverseMap();
            CreateMap<TreeNode, TreeNodeDto>().ReverseMap();

            CreateMap<CreateNodeCommand, TreeNode>()
                       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Node.Id))
                       .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Node.Name))
                       .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Node.Children));

            CreateMap<UpdateNodeCommand, TreeNode>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Node.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Node.Name))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Node.Children));
        }
    }
}
