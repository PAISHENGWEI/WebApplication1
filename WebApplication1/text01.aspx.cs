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
            isRock.LineBot.Bot bot = new isRock.LineBot.Bot("LDLXosKMbT2s7IpWWISQYEfLKPq3WBdOWjxBw0qY/lFeDgi16Ji0nHh3Ss4pONI1UBdgryRdNQl4hwlZc1ZkqtF3OnIamIGyQ4Bkxchz0Oujmrh1dmjCRq5GERwGnLEySmheMC7b98Ims2yUdzblgAdB04t89/1O/w1cDnyilFU=");
            bot.PushMessage("U562246020d0f9a258369a63e090215e", "text");
            bot.PushMessage("U562246020d0f9a258369a63e090215e",1 , 1);
            bot.PushMessage("U562246020d0f9a258369a63e090215e", new Uri("https://i2.kknews.cc/SIG=4ru29q/31oq00035prn11o60rq7.jpg"));
        }
    }
}
