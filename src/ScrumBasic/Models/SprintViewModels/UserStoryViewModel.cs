﻿using Microsoft.AspNetCore.Mvc.Rendering;
using ScrumBasic.Data;
using ScrumBasic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.Models.SprintViewModels
{
    public class UserStoryViewModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Point { get; set; }
        /// <summary>
        /// 用户故事 新功能 bug 技术故事
        /// </summary>
        public int ItemTypeCode { get; set; }
        /// <summary>
        /// backlog todo doing done
        /// </summary>
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string ListID { get; set; }
        public int Order { get; set; }
        public DateTime CreateTime { get; set; }

        public string DefaultStoryCode
        {
            get
            {
                return "Unstarted";
            }
        }
        public string ButtonDisplayName
        {
            get;set;
        }
        public IEnumerable<SelectListItem> StatusList { get; internal set; }
    }
}
