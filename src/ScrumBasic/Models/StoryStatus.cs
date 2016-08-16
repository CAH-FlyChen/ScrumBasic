using Microsoft.EntityFrameworkCore;
using ScrumBasic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.Models
{
    [Table("DM_StoryStatus")]
    public class StoryStatus
    {
        [Key]
        public string Code { get; set; }
        public string Text { get; set; }

        public string ButtonDisplayName { get; set; }
        public int Order { get; set; }
    }





    public class StoryStatusList
    {
        private static List<StoryStatus> status = new List<StoryStatus>();


        public static List<StoryStatus> GetStatusList(ApplicationDbContext ctx)
        {
            if (!status.Any())
            {
                    status = ctx.StoryStatus.OrderBy(t=>t.Order).ToList();
            }
            return status;
        }

        public static string GetStatusText(string code, ApplicationDbContext ctx)
        {
            GetStatusList(ctx);
            var r = status.SingleOrDefault(x => x.Code==code);
            if(default(KeyValuePair<string, string>).Equals(r))
            {
                return ""; 
            }
            else
            {
                return r.Text;
            }
        }

        public static StoryStatus GetNextStatus(string statusCode, ApplicationDbContext ctx)
        {
            GetStatusList(ctx);
            for(int i=0;i< status.Count;i++)
            {
                if(status[i].Code==statusCode)
                {
                    if ((i + 1 )<= status.Count)
                        return status[i + 1];
                    else
                        return status[0];
                }
            }
            return null;
        }
        public static StoryStatus GetNextStatusButtonDisplay(string statusCode, ApplicationDbContext ctx)
        {
            GetStatusList(ctx);
            for (int i = 0; i < status.Count; i++)
            {
                if (status[i].Code == statusCode)
                {
                    if ((i + 1) <= status.Count)
                        return status[i + 1];
                    else
                        return status[0];
                }
            }
            return null;
        }
        public static StoryStatus GetStatusButtonDisplay(string statusCode, ApplicationDbContext ctx)
        {
            GetStatusList(ctx);

            for (int i = 0; i < status.Count; i++)
            {
                if (status[i].Code == statusCode)
                {
                        return status[i];
                }
            }
            return null;
        }
    }
}
