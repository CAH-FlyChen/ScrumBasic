using System.Linq;
using System.Threading.Tasks;
using ScrumBasic.Models;
using ScrumBasic.Models.SprintViewModels;
using System.Collections.Generic;
using AutoMapper;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ScrumBasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ScrumBasic.Controllers
{
    public class StoryStatusController : Controller
    {
        private string mystr;
        private IOptions<Startup.MyOptions> _optons { get; set; }
        private ApplicationDbContext _context;
        private IMapper map;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="context"></param>
        /// <param name="opt"></param>
        public StoryStatusController(ApplicationDbContext context, IOptions<Startup.MyOptions> opt, IMapper map)
        {
            _optons = opt;
            _context = context;
            this.map = map;
        }

        public IEnumerable<SelectListItem> GetStatusSelectList()
        {
            var selectList = StoryStatusList.GetStatusList(_context).OrderBy(t => t.Order).Select(a => new SelectListItem
            {
                Text = a.Text,
                Value = a.Code
            });
            return selectList;
        }



    }
}
