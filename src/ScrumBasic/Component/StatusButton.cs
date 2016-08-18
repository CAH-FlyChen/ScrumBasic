using Microsoft.AspNetCore.Mvc;
using ScrumBasic.Models.SprintViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.Component
{
    [ViewComponent(Name = "StatusButton")]
    public class StatusButton: ViewComponent
    {
 
        public StatusButton()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(UserStoryViewModel model)
        {
            return View(model);
        }
    }
}
