using AutoMapper;
using ScrumBasic.Models;
using ScrumBasic.Models.SprintViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic
{
    public class MappingProfile: Profile
    {
        public MappingProfile():base()
        {
            CreateMap<UserStoryViewModel, UserStory>();
            CreateMap<UserStory, UserStoryViewModel>()
                .ForMember(dest => dest.CreatorID, opt => opt.MapFrom(src => src.Creator.Id))
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.UserName))
                .ForMember(dest => dest.AssignToID, opt => opt.MapFrom(src => src.AssignTo.Id))
                .ForMember(dest => dest.AssignToName, opt => opt.MapFrom(src => src.AssignTo.UserName))
                ;
        }
    }
}
