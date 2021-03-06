﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.ViewModels.Sprint
{
    public class UserStoryListViewModel
    {
        public int BacklogItemCount { get; set; }
        public List<UserStoryViewModel> BacklogItems { get; set; }
        public int CurrentItemCount { get; set; }
        public List<UserStoryViewModel> CurrentItems { get; set; }

        public UserStoryListViewModel()
        {
            BacklogItems = new List<UserStoryViewModel>();
            CurrentItems = new List<UserStoryViewModel>();
        }
    }
}
