﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.Models.SprintViewModels
{
    public class ChangeOrderViewModel
    {
        public string ItemID { get; set; }
        public int OldIndex { get; set; }
        public int NewIndex { get; set; }
        public string OldListID { get; set; }
        public string NewListID { get; set; }
    }
}
