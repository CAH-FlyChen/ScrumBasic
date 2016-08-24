using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.Models.SprintViewModels
{
    public class ChangeStoryStatusViewModel
    {
        public string ItemID { get; set; }
        public string CurrentStatusCode { get; set; }
        public string ApprovalResult { get; set; }
    }
}
