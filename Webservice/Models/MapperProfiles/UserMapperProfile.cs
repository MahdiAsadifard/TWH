using AutoMapper;
using Models.DTOs.User;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserRecord, UserResponseDTO>();
            CreateMap<UserResponseDTO, UserRecord>();

            CreateMap<UserRecord, UserRequestDTO>()
                .ForMember(dest => dest.Password, src => src.MapFrom(x => x.HashPassword));

            CreateMap<UserRequestDTO, UserRecord>();
        }
    }
}
