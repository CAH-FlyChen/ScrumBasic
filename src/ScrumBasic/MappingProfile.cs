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
            CreateMap<UserStory, UserStoryViewModel>();
        }
    }
}
