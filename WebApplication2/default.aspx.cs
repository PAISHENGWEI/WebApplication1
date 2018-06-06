using isRock.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class _default : System.Web.UI.Page
    {
        const string channelAccessToken = "JSCQiUyzNhGMbsOQbuNFkHrUldn43xSB0HcDbM5cNSLLxXb4VzOIidgGHaEymJhfUBdgryRdNQl4hwlZc1ZkqtF3OnIamIGyQ4Bkxchz0Ou3Vgn3yguSHLj88EEYK0TUheSV0kRPsA+1ZtvbMutauAdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId= "U562246020d0f9a258369a63e090215e2";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, $"測試 {DateTime.Now.ToString()} ! ");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, 1,2);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            //建立actions,作為ButtonTemplate的用戶回復行為
            var actions = new List<isRock.LineBot.TemplateActionBase>();
            actions.Add(new isRock.LineBot.MessageAction() { label = "標題-文字回復", text = "回復文字" });
            actions.Add(new isRock.LineBot.UriAction() { label = "標題-開啟URL", uri = new Uri("http://www.google.com") });
            actions.Add(new isRock.LineBot.PostbackAction() { label = "標題-發生postback", data = "abc=aaa&def=111" });
            
            var ButtonTemplateMsg = new isRock.LineBot.ButtonsTemplate()
            {
                title = "選項",
                text = "請選擇其中之一",
                altText = "請在手機上檢視",
                thumbnailImageUrl = new Uri("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSQSfptbc-INs9IUyaBi7xU3_Hr52NbdIEOwOa_gX5xrrQXEd0m7w"),
                actions = actions
            };
            bot.PushMessage(AdminUserId, ButtonTemplateMsg);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            //建立actions,作為ButtonTemplate的用戶回復行為
            var actions = new List<isRock.LineBot.TemplateActionBase>();
            actions.Add(new isRock.LineBot.MessageAction() { label = "Yes", text = "Yes" });
            actions.Add(new isRock.LineBot.MessageAction() { label = "No", text = "No" });

            var ConfirmTemplate = new isRock.LineBot.ConfirmTemplate()
            {
                
                text = "請選擇其中之一",
                altText = "請在手機上檢視",
  
                actions = actions
            };
            bot.PushMessage(AdminUserId, ConfirmTemplate);
        }
    }
}