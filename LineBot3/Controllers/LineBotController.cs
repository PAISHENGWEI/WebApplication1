using isRock.LineBot;
using Microsoft.ProjectOxford.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace LineBot3.Controllers
{
    public class LineBotController : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "JSCQiUyzNhGMbsOQbuNFkHrUldn43xSB0HcDbM5cNSLLxXb4VzOIidgGHaEymJhfUBdgryRdNQl4hwlZc1ZkqtF3OnIamIGyQ4Bkxchz0Ou3Vgn3yguSHLj88EEYK0TUheSV0kRPsA+1ZtvbMutauAdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "U562246020d0f9a258369a63e090215e2";
        const string VisionAPIKey = "9750f3bee74b4bcb80c1f592a2ae5821";

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
                isRock.LineBot.Bot bot = new isRock.LineBot.Bot(channelAccessToken);
                var UserInfo = bot.GetUserInfo(LineEvent.source.userId);

                //配合Line verify 
                //
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                if (LineEvent.type == "postback")
                {
                    var data = LineEvent.postback.data;
                    var dt = LineEvent.postback.Params.time;
                    this.ReplyMessage(LineEvent.replyToken, $"觸發了 postback \n 資料為:{data}\n 選擇結果:{dt} ");
                }
                if (LineEvent.type == "message")
                {
                    //回覆訊息
                    //if (LineEvent.message.type == "sticker") //收到貼圖
                    //    this.ReplyMessage(LineEvent.replyToken, 1, 2);
                    if (LineEvent.message.type == "location") //GPS
                        this.ReplyMessage(LineEvent.replyToken, $"你的位置在\n{LineEvent.message.latitude},{LineEvent.message.longitude}\n{LineEvent.message.address}");

                    if (LineEvent.message.type == "text")
                    {
                        if (LineEvent.message.text == "Hello")
                            this.ReplyMessage(LineEvent.replyToken, UserInfo.displayName + "您好，今天適合穿短袖上衣");
                    }

                    if (LineEvent.message.text == "餓了嗎")
                    {
                        var bott = new Bot(channelAccessToken);
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
                        bott.PushMessage(AdminUserId, ConfirmTemplate);
                    }
                    if (LineEvent.message.text == "Yes")
                    {
                        var bot1 = new Bot(channelAccessToken);
                        //建立actions,作為ButtonTemplate的用戶回復行為
                        var actions = new List<isRock.LineBot.TemplateActionBase>();
                        actions.Add(new isRock.LineBot.MessageAction() { label = "標題-文字回復", text = "回復文字" });
                        actions.Add(new isRock.LineBot.UriAction() { label = "選擇餐廳", uri = new Uri("https://tgifridays.com.tw/locations") });
                       // actions.Add(new isRock.LineBot.PostbackAction() { label = "標題-發生postback", data = "abc=aaa&def=111" });
                        actions.Add(new isRock.LineBot.DateTimePickerAction() { label = "請選擇時間", mode = "date" });
                        var ButtonTemplateMsg = new isRock.LineBot.ButtonsTemplate()
                        {
                            title = "選項",
                            text = "請選擇其中之一",
                            altText = "請在手機上檢視",
                            thumbnailImageUrl = new Uri("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSQSfptbc-INs9IUyaBi7xU3_Hr52NbdIEOwOa_gX5xrrQXEd0m7w"),
                            actions = actions
                        };
                        bot1.PushMessage(AdminUserId, ButtonTemplateMsg);
                    }
                    if (LineEvent.message.type == "image")
                    {
                        //取得圖片Bytes
                        var bytes = this.GetUserUploadedContent(LineEvent.message.id);
                        //儲存為圖檔
                        var guid = Guid.NewGuid().ToString();
                        var filename = $"{guid}.png";
                        var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Temps/");

                        System.IO.File.WriteAllBytes(path + filename, bytes);

                        //取得base URL
                        var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                        //組出外部可以讀取的檔名
                        var url = $"{baseUrl}/Temps/{filename}";
                        // this.ReplyMessage(LineEvent.replyToken, $"你的圖片位於\n{url}");
                        var fs1 = new FileStream(path + filename, FileMode.Open);
                        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(fs1);
                        Graphics g = Graphics.FromImage(bmp);
                        fs1.Close();

                        var visionClient = new Microsoft.ProjectOxford.Vision.VisionServiceClient(VisionAPIKey, 
                            "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0");

                        //分析圖片(從FileUpload1.PostedFile.InputStream取得影像)
                        //分析 Faces & Description 

                        var Results = visionClient.AnalyzeImageAsync(url,
                            new VisualFeature[] { VisualFeature.Faces, VisualFeature.Description }).Result;
                        
                        int isM = 0, isF = 0;
                        foreach (var Face in Results.Faces)
                        {
                            //取得人臉位置
                            var faceRect = Face.FaceRectangle;
                            //繪製人臉紅框 
                            g.DrawRectangle(
                                        new Pen(Brushes.Red, 10),
                                        new Rectangle(faceRect.Left, faceRect.Top,
                                            faceRect.Width, faceRect.Height));

                            Font drawFont = new Font("Arial", 40);
                            SolidBrush drawBrush = new SolidBrush(Color.Red);
                            String drawString = Face.Age.ToString();

                            g.DrawString(drawString + "歲", drawFont, drawBrush, new Point(faceRect.Left - 30, faceRect.Top - 50));

                            //計算幾男幾女
                            if (Face.Gender.StartsWith("M"))
                                isM += 1;
                            else
                                isF += 1;
                        }
                        var path1 = System.Web.Hosting.HostingEnvironment.MapPath("~/Temps/");
                        var newfilename = Guid.NewGuid().ToString() + ".png";
                        bmp.Save(path1 + newfilename);

                        this.ReplyMessage(LineEvent.replyToken,new Uri($"{baseUrl}/Temps/{newfilename}"));

                    }

                }

                    return Ok();

                //this.ReplyMessage(LineEvent.replyToken, "Hello,你的UserId是:" + LineEvent.source.userId);
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
