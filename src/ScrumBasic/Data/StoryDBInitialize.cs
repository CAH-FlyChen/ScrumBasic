using ScrumBasic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.Data
{
    public static class StoryDBInitialize
    {
        public static void EnsureSeedData(this ApplicationDbContext context)
        {
            if (!context.StoryStatus.Any())
            {

                string[] codes = new string[] { "Unstarted", "Started", "Finished", "Delivered", "Accepted", "Rejected" };
                string[] text = new string[] { "未开始", "进行中", "已完成", "审批中", "已通过","已拒绝" };
                string[] buttonDisplayName = new string[] { "开始", "完成", "送审" , "通过,拒绝","完成","完成" };
                int[] order = new int[] {0,1,2,3,4,5,6 };
                for(int i=0;i<codes.Length;i++)
                    context.StoryStatus.Add(new StoryStatus { Code = codes[i], Text = text[i], Order = order[i],ButtonDisplayName=buttonDisplayName[i] });

                context.SaveChanges();
            }
        }
    }
}
