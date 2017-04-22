using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Launcher.Desktop.Contracts;
using Launcher.Desktop.Models;

namespace Launcher.Desktop.Services
{
    public class NewsService : INewsService
    {
        private const string NewsUrl = "http://mcupdate.tumblr.com/";
        private readonly IWebService downloader;

        public NewsService(IWebService downloader)
        {
            this.downloader = downloader;
        }

        public async Task<IEnumerable<News>> GetNewsAsync()
        {
            string html = await GetHtmlAsync(NewsUrl);
            var document = new HtmlDocument();
            document.LoadHtml(html);
            HtmlNode body = document.DocumentNode.SelectSingleNode("//body");
            HtmlNodeCollection entries = body.SelectNodes("//div[@class='post text']");

            return entries.Select(ParseNode);
        }

        private async Task<string> GetHtmlAsync(string url)
        {
            return await Task.Run(() => DownloadString(url));
        }

        private string DownloadString(string url)
        {
            using (downloader)
            {
                return downloader.DownloadString(url);
            }
        }

        private News ParseNode(HtmlNode node)
        {
            HtmlNode header = node.SelectSingleNode("h3");
            HtmlNodeCollection pharagraps = node.SelectNodes("p");

            string title = header.InnerText.Trim();
            string content = pharagraps.Aggregate(string.Empty, (current, pharagraph) => current + ParsePharagraph(pharagraph));

            return new News(title, content);
        }

        private string ParsePharagraph(HtmlNode pharagraph)
        {
            string html = pharagraph.InnerHtml;
            var lines = html.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < lines.Length; i++)
            {
                lines[i] = FixLine(lines[i]);
            }

            return string.Join("\n", lines).Trim() + Environment.NewLine;
        }

        private string FixLine(string line)
        {
            if (line.First() == '\n')
                line = line.Remove(0, 1);

            if (line.Contains("<a"))
                line = ReplaceLinkWithShortcut(line);

            if (line.Contains("strike"))
                line = null;

            return HtmlEntity.DeEntitize(line);
        }

        private string ReplaceLinkWithShortcut(string text)
        {
            string[] linkCharacteristicParts = { "<a", "</a>", "\">" };
            var parts = text.Split(linkCharacteristicParts, StringSplitOptions.RemoveEmptyEntries);
            return parts.Where(part => !part.Contains("href")).Aggregate(string.Empty, (current, part) => current + part);
        }
    }
}