namespace Launcher.Desktop.Models
{
    public class News
    {
        public News()
        {
        }

        public News(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public string Title { get; set; }
        public string Content { get; set; }
    }
}