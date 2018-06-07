using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LineBot3.Controllers
{
    public class LineBotController : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "JSCQiUyzNhGMbsOQbuNFkHrUldn43xSB0HcDbM5cNSLLxXb4VzOIidgGHaEymJhfUBdgryRdNQl4hwlZc1ZkqtF3OnIamIGyQ4Bkxchz0Ou3Vgn3yguSHLj88EEYK0TUheSV0kRPsA+1ZtvbMutauAdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "U562246020d0f9a258369a63e090215e2";

        [Route("api/Linebot")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken;
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息
                if (LineEvent.type == "message")
                {
                    if (LineEvent.message.type == "text") //收到文字
                        this.ReplyMessage(LineEvent.replyToken, "你說了:" + LineEvent.message.text);
                    if (LineEvent.message.type == "sticker") //收到貼圖
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                    if (LineEvent.message.type == "location") //GPS
                        this.ReplyMessage(LineEvent.replyToken,$"你的位置在\n{LineEvent.message.latitude},{LineEvent.message.longitude}");
                   if(LineEvent.message.type=="image")
                    {
                        //取得圖片Bytes
                        var bytes = this.GetUserUploadedContent(LineEvent.message.id);
                        //儲存為圖檔
                        var guid = Guid.NewGuid().ToString();
                        var filename = $"{guid}.png";
                        var path = System.Web.Hosting.HostingEnvironment.MapPath("~/temp/");
                        System.IO.File.WriteAllBytes(path + filename, bytes);
                        //取得base URL
                        var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                        //組出外部可以讀取的檔名
                        var url = $"{baseUrl}/temp/{filename}";
                        this.ReplyMessage(LineEvent.replyToken, $"你的圖片位於\n{url}");
                    }
                    //this.ReplyMessage(LineEvent.replyToken, "Hello,你的UserId是:" + LineEvent.source.userId);
                }
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
    }
}
