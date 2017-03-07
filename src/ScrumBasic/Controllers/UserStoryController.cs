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
using Microsoft.AspNetCore.Identity;

namespace ScrumBasic.Controllers
{
    public class UserStoryController : Controller
    {
        private string mystr;
        private IOptions<Startup.MyOptions> _optons { get; set; }
        public UserStoryListViewModel models = new UserStoryListViewModel();
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IMapper map;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="context"></param>
        /// <param name="opt"></param>
        public UserStoryController(ApplicationDbContext context, IOptions<Startup.MyOptions> opt, IMapper map, UserManager<ApplicationUser> userManager)
        {
            _optons = opt;
            _context = context;
            this.map = map;
            _userManager = userManager;
        }

        // GET: UserStoryViewModels
        public async Task<IActionResult> Index(string projID)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(x => x.ID == projID);
            List<UserStory> userStories = await _context.UserStories.Include(x=>x.Creator).Where(t=>t.Project.ID== projID).OrderBy(t=>t.Order).ToListAsync<UserStory>();
            foreach (var us in userStories)
            {
                UserStoryViewModel m = map.Map<UserStoryViewModel>(us);
                m.ButtonDisplayName = StoryStatusList.GetStatusButtonDisplay(m.StatusCode, _context).ButtonDisplayName;
                if (m.ListID == "Backlog")
                {
                    models.BacklogItemCount += 1;
                    models.BacklogItems.Add(m);
                } 
                else if (m.ListID == "Current")
                {
                    models.CurrentItemCount += 1;
                    models.CurrentItems.Add(m);
                }
                else if (m.ListID == "ICEBox")
                {
                    models.ICEItemCount += 1;
                    models.ICEItems.Add(m);
                }
                else if (m.ListID == "Done")
                {
                    models.DoneItemsCount += 1;
                    models.DoneItems.Add(m);
                }
            }
            ViewBag.ProjectName = project.Name;
                
            return View(models);
        }

        public IActionResult Create(string listId)
        {
            UserStoryViewModel userStoryViewModel = new UserStoryViewModel();
            userStoryViewModel.Content = "";
            userStoryViewModel.ID = Guid.NewGuid().ToString("N");
            userStoryViewModel.Point = 0;
            userStoryViewModel.ListID = listId;
            var assigntoSelectList = _userManager.Users.ToList().Select(a => new SelectListItem
            {
                Text = a.UserName,
                Value = a.Id
            });

            userStoryViewModel.AssignToList = assigntoSelectList;

            var selectList = StoryStatusList.GetStatusList(_context).OrderBy(t => t.Order).Select(a => new SelectListItem
            {
                Text = a.Text,
                Value = a.Code
            });

            userStoryViewModel.StatusList = selectList;
            return PartialView(userStoryViewModel);
        }

        //// POST: UserStoryViewModels/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserStoryViewModel userStoryViewModel)
        {
            if (ModelState.IsValid)
            {
                //mapping   
                UserStory usNew = map.Map<UserStory>(userStoryViewModel);
                usNew.ID = Guid.NewGuid().ToString("N");
                usNew.StatusCode = "Unstarted";
                usNew.Creator = _userManager.FindByNameAsync(User.Identity.Name).Result;
                usNew.CreateTime = DateTime.Now;
                usNew.Order = _context.UserStories.Max(t => t.Order)+1;
                _context.UserStories.Add(usNew);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("OK");
        }


        // GET: UserStoryViewModels/Edit/5
        [HttpPost]
        public async Task<IActionResult> Move([FromBody] StoryMoveViewModel p)
        {
            if (p.ItemId == null)
            {
                return HttpNotFound();
            }

            UserStory us = await _context.UserStories.SingleAsync(m => m.ID == p.ItemId);
            if (us == null)
            {
                return HttpNotFound();
            }

            us.StatusCode = p.TargetStatus;
            await _context.SaveChangesAsync();
            return Json(true);
            //return View();
        }

        private IActionResult HttpNotFound()
        {
            Response.StatusCode = 404;
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOrder([FromBody] ChangeOrderViewModel p)
        {
            UserStory story = await _context.UserStories.SingleAsync(m => m.ID == p.ItemID);
            if (story == null) return HttpNotFound();

            //跨列表
            if (p.NewListID!=p.OldListID && p.NewListID!="" && p.OldListID!="")
            {
                story.Order = p.NewIndex;
                story.ListID = p.NewListID;
                //分割位置下面的向上移动
                //处理新列表。下移动新列表的分割条目后面的项目
                var downItems = _context.UserStories.Where(t => t.Order >= p.NewIndex && t.ListID == p.NewListID).OrderBy(t => t.Order).ToList();
                for (int i = 0; i < downItems.Count; i++)
                {
                    var item = downItems[i];
                    item.Order = i + 1 + p.NewIndex;
                }
                //处理源列表移出位置上移
                var oldListItems = _context.UserStories.Where(t => t.Order > p.OldIndex && t.ListID == p.OldListID).OrderBy(t => t.Order).ToList();
                for(int i=0;i<oldListItems.Count;i++)
                {
                    var item = oldListItems[i];
                    item.Order = p.OldIndex + i;
                }
                
            }
            else
            {
                //同列表
                if (p.NewIndex > p.OldIndex)
                {
                    //向下移动
                    //找到在旧列表中 分割的位置上边的内容
                    var upItems = _context.UserStories.Where(t => t.Order <= p.NewIndex && t.Order > p.OldIndex && t.ListID == p.OldListID && t.Order != p.OldIndex).OrderBy(t => t.Order).ToList();

                    story.Order = p.NewIndex;
                    for (int i = 0; i < upItems.Count; i++)
                    {
                        var item = upItems[i];
                        item.Order = i + p.OldIndex;
                    }
                }
                else
                {
                    //向上移动
                    //找到在旧列表中 分割的位置下边的内容
                    var downItems = _context.UserStories.Where(t => t.Order >= p.NewIndex && t.Order < p.OldIndex && t.ListID == p.OldListID && t.Order != p.OldIndex).OrderBy(t => t.Order).ToList();

                    story.Order = p.NewIndex;
                    for (int i = 0; i < downItems.Count; i++)
                    {
                        var item = downItems[i];
                        item.Order = i + 1 + p.NewIndex;
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string listID,string itemID)
        {
            if (string.IsNullOrEmpty(itemID))
            {
                return HttpNotFound();
            }
            UserStory story = await _context.UserStories.SingleAsync(m => m.ID == itemID);
            if (story == null)
            {
                return HttpNotFound();
            }
            UserStoryViewModel usvm = map.Map<UserStoryViewModel>(story);

            var assigntoSelectList = _userManager.Users.ToList().Select(a => new SelectListItem
            {
                Text = a.UserName,
                Value = a.Id
            });

            usvm.AssignToList = assigntoSelectList;

            var selectList = StoryStatusList.GetStatusList(_context).OrderBy(t => t.Order).Select(a => new SelectListItem
            {
                Text = a.Text,
                Value = a.Code
            });

            usvm.StatusList = selectList;
            return PartialView(usvm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserStoryViewModel vm)
        {
            if (vm.ID == null)
            {
                return HttpNotFound();
            }

            UserStory story = await _context.UserStories.SingleAsync(m => m.ID == vm.ID);
            if (story == null)
            {
                return HttpNotFound();
            }
            story.Content = vm.Content;
            story.ItemTypeCode = vm.ItemTypeCode;
            story.Point = vm.Point;
            story.StatusCode = vm.StatusCode;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody]ChangeStoryStatusViewModel p)
        {
            if (ModelState.IsValid)
            {
                var item = _context.UserStories.Single(t => t.ID == p.ItemID);
                var s = StoryStatusList.GetNextStatusButtonDisplay(item.StatusCode, _context);
                if (string.IsNullOrEmpty(p.ApprovalResult))
                    item.StatusCode = s.Code;
                else if (p.ApprovalResult == "Y")
                    item.StatusCode = "Accepted";
                else
                    item.StatusCode = "Rejected";
                await _context.SaveChangesAsync();
                
                UserStoryViewModel model = map.Map<UserStoryViewModel>(item);
                model.ButtonDisplayName = s.ButtonDisplayName;
                return ViewComponent("StatusButton", new { model = model });
            }

            return null;
        }

    }
}
