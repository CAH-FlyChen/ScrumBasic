using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.Models
{
    public class StoryStatus
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
    }
    public class StoryStatusList
    {
        private static List<StoryStatus> status = new List<StoryStatus>();
        public static string 未开始 { get { return "NotStarted"; } }
        public static string 完成 { get { return "InProgress"; } }
        public static string 已完成 { get { return "Complete"; } }
        public static string 提交审批 { get { return "Approve"; } }
        public static string 审批通过 { get { return "Approval"; } }
        public static string 审批拒绝 { get { return "Rejected"; } }
        public static string 关闭 { get { return "Close"; } }
        public static string 已关闭 { get { return "Closed"; } }

        static StoryStatusList()
        {
            string[] namesValues = new string[] {
                "未开始", "NotStarted",
                "完成", "InProgress",
                "已完成", "Complete",
                "提交审批", "Approve",
                "审批通过", "Approval",
                "审批拒绝", "Rejected",
                "关闭", "Close",
                "已关闭", "Closed"
            };
            for (int i = 0; i < namesValues.Length; i += 2)
            {
                StoryStatus ss = new StoryStatus();
                ss.Name = namesValues[i];
                ss.Value = namesValues[i + 1];
                ss.Order = i;
                status.Add(ss);
            }
        }


        public static List<StoryStatus> GetStatusList()
        {
            return status;
        }

        public static string GetStatusText(string code)
        {
            var r = status.SingleOrDefault(x => x.Value==code);
            if(default(KeyValuePair<string, string>).Equals(r))
            {
                return ""; 
            }
            else
            {
                return r.Name;
            }
        }

        public static StoryStatus GetNextStatus(string statusCode)
        {
            for(int i=0;i< status.Count;i++)
            {
                if(status[i].Value==statusCode)
                {
                    if ((i + 1 )<= status.Count)
                        return status[i + 1];
                    else
                        return status[0];
                }
            }
            return null;
        }
    }
}
