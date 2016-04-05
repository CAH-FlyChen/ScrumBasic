using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumBasic.Models
{
    public class StoryStatus
    {
        private static Dictionary<string, string> status;
        public static string 未开始 { get { return "NotStarted"; } }
        public static string 进行中 { get { return "InProgress"; } }
        public static string 已完成 { get { return "Complete"; } }
        public static string 提交审批 { get { return "Approve"; } }
        public static string 审批通过 { get { return "Approval"; } }
        public static string 审批拒绝 { get { return "Rejected"; } }
        public static string 关闭 { get { return "Close"; } }
        public static string 已关闭 { get { return "Closed"; } }

        static StoryStatus()
        {
            if(status==null)
            {
                Dictionary<string, string> kvs = new Dictionary<string, string>();
                kvs.Add("未开始", "NotStarted");
                kvs.Add("进行中", "InProgress");
                kvs.Add("已完成", "Complete");
                kvs.Add("提交审批", "Approve");
                kvs.Add("审批通过", "Approval");
                kvs.Add("审批拒绝", "Rejected");
                kvs.Add("关闭", "Close");
                kvs.Add("已关闭", "Closed");
                status = kvs;
            }
        }


        public static Dictionary<string,string> GetStatusList()
        {
            return status;
        }

        public static string GetStatusText(string code)
        {
            KeyValuePair<string,string> r = status.SingleOrDefault(x => x.Value == code);
            if(default(KeyValuePair<string, string>).Equals(r))
            {
                return ""; 
            }
            else
            {
                return r.Key;
            }
        }

    }
}
