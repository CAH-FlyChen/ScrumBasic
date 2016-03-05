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
        private IOptions<Startup.MyOptions> _optons { get; set; }
        public static List<UserStoryViewModel> models = new List<UserStoryViewModel>();
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
            models.Clear();
            List<UserStory> userStories = await _context.UserStory.ToListAsync<UserStory>();
            foreach (var us in userStories)
            {
                UserStoryViewModel m = Mapper.Map<UserStoryViewModel>(us);
                models.Add(m);
            }
            //ViewData["story_items"] = models;
            return View((IEnumerable)models);
            //return View(await _context.UserStoryViewModel.ToListAsync());
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

        //// POST: UserStoryViewModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserStoryViewModel userStoryViewModel)
        {
            if (ModelState.IsValid)
            {
                //mapping   
                UserStory usNew = Mapper.Map<UserStory>(userStoryViewModel);
                usNew.ID = Guid.NewGuid().ToString("N");
                usNew.Status = 1;
                _context.UserStory.Add(usNew);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("OK");
        }
        public class Pa
        {
            public string ItemId { get; set; }
            public int TargetStatus { get; set; }
        }

        // GET: UserStoryViewModels/Edit/5
        [HttpPost]
        public async Task<IActionResult> Move([FromBody] Pa p)
        {
            if (p.ItemId == null)
            {
                return HttpNotFound();
            }

            UserStory us = await _context.UserStory.SingleAsync(m => m.ID == p.ItemId);
            if (us == null)
            {
                return HttpNotFound();
            }

            us.Status = p.TargetStatus;
            await _context.SaveChangesAsync();
            return Json(true);
            //return View();
        }




        public async Task<IActionResult> EditItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return HttpNotFound();
            }
            UserStory story = await _context.UserStory.SingleAsync(m => m.ID == itemId);
            if (story == null)
            {
                return HttpNotFound();
            }
            return PartialView(story);
        }


        //// GET: UserStoryViewModels/Edit/5
        public async Task<IActionResult> Edit(UserStoryViewModel vm)
        {
            if (vm.ID == null)
            {
                return HttpNotFound();
            }

            UserStory story = await _context.UserStory.SingleAsync(m => m.ID == vm.ID);
            if (story == null)
            {
                return HttpNotFound();
            }
            story.Content = vm.Content;
            story.ItemTypeCode = vm.ItemTypeCode;
            story.Point = vm.Point;

            return RedirectToAction("Index");
            //return View(story);
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
