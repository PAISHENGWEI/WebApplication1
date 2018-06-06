using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class text01 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            isRock.LineBot.Bot bot = new isRock.LineBot.Bot("JSCQiUyzNhGMbsOQbuNFkHrUldn43xSB0HcDbM5cNSLLxXb4VzOIidgGHaEymJhfUBdgryRdNQl4hwlZc1ZkqtF3OnIamIGyQ4Bkxchz0Ou3Vgn3yguSHLj88EEYK0TUheSV0kRPsA+1ZtvbMutauAdB04t89/1O/w1cDnyilFU=");
            bot.PushMessage("U562246020d0f9a258369a63e090215e2", "text");
            bot.PushMessage("U562246020d0f9a258369a63e090215e2",1 , 1);
            bot.PushMessage("U562246020d0f9a258369a63e090215e2", new Uri("https://i2.kknews.cc/SIG=4ru29q/31oq00035prn11o60rq7.jpg"));
        }
    }
}