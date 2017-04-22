using System.Linq;
using System.Threading.Tasks;
using Launcher.Desktop.Contracts;
using Launcher.Desktop.Models;
using Launcher.Desktop.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Launcher.Tests
{
    [TestClass]
    public class NewsServiceTests
    {
        //Used from https://mcupdate.tumblr.com/
        private const string HtmlPart = @"<body>
<div class=""ui_dialog_lock"" style=""display: none;""></div><table width=""100%"" height=""100%"">
<tbody>
<tr>
<td height=""100%"" style=""padding-left: 16px;padding-right: 16px;"">
<table width=""100%""><tbody><tr><td><h1>Minecraft News</h1></td><td align=""right""><a href=""http://tumblr.com/""><font color=""#555555"" size=""-2"">Powered by Tumblr</font></a></td></tr></tbody></table><div class=""container"">
<div id=""posts"">
<div class=""post text"">
<h3>
<a href=""https://mcupdate.tumblr.com/post/154722948989/minecraft-1112-released"">Minecraft 1.11.2 Released</a>
</h3>
<p>Surprise! Minecraft is now at version 1.11.2!</p>
<p>The main purpose of this update is to fix a number of bugs, but we also took the opportunity to introduce a number of other features. Just in time for the holidays!</p>
<p>+ Added rocket-propelled elytra flight<br>+ Added Iron Nuggets<br>+ Added Sweeping Edge enchantment for swords<br>* Changed the attack indicator to hint when you should attack</p>
<p>The update is available in your Minecraft launcher, enjoy!</p>
<p>Merry Christmas and Happy New Year from the Minecraft Team!</p>
<div class=""meta"">
<div class=""permaLink"">
<a title=""Permanent link for this post"" href=""https://mcupdate.tumblr.com/post/154722948989/minecraft-1112-released""></a>
</div>
</div>
</div>
<div style=""width: 100%; height: 16px;"">
</div>
<div class=""post text"">
<h3><a href=""https://mcupdate.tumblr.com/post/154071571459/become-a-code-wizard-in-just-60-minutes"">Become a CODE WIZARD in just 60 minutes!</a></h3><p>
Check out our new Hour of Code tutorial, designed to teach you the basics of coding in a mere 60 minutes. We introduce concepts of game design and computer science in easy-to-understand steps before giving insight into mob behavior and more. 
</p><p>
<br>Both the 2015 and 2016 tutorials are now available at <a href=""http://t.umblr.com/redirect?z=http%3A%2F%2Fcode.org%2Fminecraft&t=YzFmZjQyYzJjZmYwMjkwYjI4OWFmYTNhNGEzYjBjMjhjNTBlMDczNSxraXZsVXIwZQ%3D%3D&b=t%3Av62pV3ylqYAJw1DQbJXwpQ&p=https%3A%2F%2Fmcupdate.tumblr.com%2Fpost%2F154071571459%2Fbecome-a-code-wizard-in-just-60-minutes&";

        private const string LastNewsTitle = "Become a CODE WIZARD in just 60 minutes!";
        private const string LastNewsContent = "Check out our new Hour of Code tutorial, designed to teach you the basics of coding in a mere 60 minutes. We introduce concepts of game design and computer science in easy-to-understand steps before giving insight into mob behavior and more.\r\n\r\n";

        [TestMethod]
        public async Task ShouldGetNews()
        {
            var lastNews = new News(LastNewsTitle, LastNewsContent);
            var webServiceMock = new Mock<IWebService>();
            webServiceMock.Setup(x => x.DownloadString(It.IsAny<string>())).Returns(HtmlPart);
            var service = new NewsService(webServiceMock.Object);

            var news = (await service.GetNewsAsync()).ToList();

            Assert.AreEqual(2, news.Count);
            Assert.AreEqual(lastNews.Title, news.Last().Title);
            Assert.AreEqual(lastNews.Content, news.Last().Content);
        }
    }
}