using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using ScrumBasic.Models;
using ScrumBasic.ViewModels.Sprint;
using System.Collections.Generic;
using Microsoft.Extensions.OptionsModel;
using AutoMapper;
using System;
using System.Collections;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace ScrumBasic.Controllers
{
    public class UserStoryController : Controller
    {
        private string mystr;
        private IOptions<Startup.MyOptions> _optons { get; set; }
        public UserStoryListViewModel models = new UserStoryListViewModel();
        private ApplicationDbContext _context;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="context"></param>
        /// <param name="opt"></param>
        public UserStoryController(ApplicationDbContext context, IOptions<Startup.MyOptions> opt)
        {
            _optons = opt;
            _context = context;
            Mapper.CreateMap(typeof(UserStoryViewModel), typeof(UserStory));
            Mapper.CreateMap(typeof(UserStory), typeof(UserStoryViewModel));
        }


        public double ForTest(int a, int b)
        {
            return a / b;
        }

        // GET: UserStoryViewModels
        public async Task<IActionResult> Index()
        {
            List<UserStory> userStories = await _context.UserStories.OrderBy(t=>t.Order).ToListAsync<UserStory>();
            foreach (var us in userStories)
            {
                UserStoryViewModel m = Mapper.Map<UserStoryViewModel>(us);
                m.StatusName = StoryStatusList.GetStatusText(m.Status);
                if (m.ListID == "backlog")
                {
                    models.BacklogItemCount += 1;
                    models.BacklogItems.Add(m);
                } 
                else if (m.ListID == "current")
                {
                    models.CurrentItemCount += 1;
                    models.CurrentItems.Add(m);
                }
            }

            return View(models);
        }

        //// GET: UserStoryViewModels/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    UserStory us = await _context.UserStory.SingleAsync(m => m.ID == id);
        //    if (us == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(us);
        //}

        //// GET: UserStoryViewModels/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        public IActionResult Create()
        {
            UserStoryViewModel userStoryViewModel = new UserStoryViewModel();
            userStoryViewModel.Content = "";
            userStoryViewModel.ID = Guid.NewGuid().ToString("N");
            userStoryViewModel.Point = 0;
            userStoryViewModel.Status = StoryStatusList.未开始;
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
                UserStory usNew = Mapper.Map<UserStory>(userStoryViewModel);
                usNew.ID = Guid.NewGuid().ToString("N");
                usNew.CreateTime = DateTime.Now;
                usNew.Order = _context.UserStories.Max(t => t.Order)+1;
                usNew.ListID = "Backlog";
                _context.UserStories.Add(usNew);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("OK");
        }
        public class Pa
        {
            public string ItemId { get; set; }
            public string TargetStatus { get; set; }
        }

        // GET: UserStoryViewModels/Edit/5
        [HttpPost]
        public async Task<IActionResult> Move([FromBody] Pa p)
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

            us.Status = p.TargetStatus;
            await _context.SaveChangesAsync();
            return Json(true);
            //return View();
        }

        public class ChangeOrderParam
        {
            public string ItemID { get; set; }
            public int OldIndex { get; set; }
            public int NewIndex { get; set; }
            public string OldListID { get; set; }
            public string NewListID { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> ChangeOrder([FromBody] ChangeOrderParam p)
        {
            UserStory story = await _context.UserStories.SingleAsync(m => m.ID == p.ItemID);
            if (story == null)
            {
                return HttpNotFound();
            }
            if(p.OldIndex==-1)
            {
                story.Order = p.NewIndex;
                story.ListID = p.NewListID;

                var items = _context.UserStories.Where(t => t.Order >= p.NewIndex && t.ListID == p.OldListID).ToList();
                foreach (var t in items)
                    t.Order = t.Order - 1;

                var itemsx = _context.UserStories.Where(t => t.Order >= p.NewIndex && t.ListID == p.NewListID).ToList();
                foreach (var t in itemsx)
                    t.Order = t.Order + 1;

            }
            else
            {
                if (p.NewIndex > p.OldIndex)
                {
                    //向下
                    var items = _context.UserStories.Where(t => t.Order <= p.NewIndex && t.Order > p.OldIndex && t.ListID == p.OldListID).ToList();
                    foreach (var t in items)
                        t.Order = t.Order - 1;
                }
                else
                {
                    var items = _context.UserStories.Where(t => t.Order >= p.NewIndex && t.Order < p.OldIndex && t.ListID==p.OldListID).ToList();
                    foreach (var t in items)
                        t.Order = t.Order + 1;
                }
                story.Order = p.NewIndex;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return HttpNotFound();
            }
            UserStory story = await _context.UserStories.SingleAsync(m => m.ID == itemId);
            if (story == null)
            {
                return HttpNotFound();
            }
            return PartialView(story);
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
            story.Status = vm.Status;
            _context.SaveChanges();

            return RedirectToAction("Index");
            //return View(story);
        }


        public class ChangeStatusParam
        {
            public string ItemID { get; set; }
            public string CurrentStatusCode { get; set; }
        }


        [HttpPost]
        public async Task<IActionResult> ChangeStatus(ChangeStatusParam p)
        {
            if(ModelState.IsValid)
            {
                _context.
            }
        }


        //// POST: UserStoryViewModels/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(UserStoryViewModel userStoryViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Update(userStoryViewModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(userStoryViewModel);
        //}

        //// GET: UserStoryViewModels/Delete/5
        //[ActionName("Delete")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    UserStoryViewModel userStoryViewModel = await _context.UserStoryViewModel.SingleAsync(m => m.ID == id);
        //    if (userStoryViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(userStoryViewModel);
        //}

        //// POST: UserStoryViewModels/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    UserStoryViewModel userStoryViewModel = await _context.UserStoryViewModel.SingleAsync(m => m.ID == id);
        //    _context.UserStoryViewModel.Remove(userStoryViewModel);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
    }


}
