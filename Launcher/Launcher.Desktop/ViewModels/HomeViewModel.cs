using System.Collections.Generic;
using Launcher.Desktop.Contracts;
using Launcher.Desktop.Models;
using Launcher.Desktop.Extensions;
using MahApps.Metro.IconPacks;

namespace Launcher.Desktop.ViewModels
{
    public sealed class HomeViewModel : TabBase
    {
        private readonly IAccountService accountService;
        private readonly INewsService newsService;
        private readonly IMetroWindowManager windowManager;
        private string email;
        private IEnumerable<News> news;

        public HomeViewModel(IAccountService accountService, INewsService newsService, IMetroWindowManager windowManager)
        {
            this.accountService = accountService;
            this.newsService = newsService;
            this.windowManager = windowManager;
            DisplayName = "Home";
            DisplayIcon = PackIconMaterialKind.HomeOutline;
            DisplayOrder = 1;
            IsHomeTab = true;
        }

        public string Email
        {
            get => email;
            set => this.Set(out email, value);
        }

        public IEnumerable<News> News
        {
            get => news;
            set => this.Set(out news, value);
        }

        public async void ShowNews(News news)
        {
            if (news != null)
            {
                await windowManager.ShowMessageAsync(news.Title, news.Content);
            }
        }

        protected override async void OnInitialize()
        {
            await windowManager.ShowProgressAndDoAsync(async () =>
            {
                Email = (await accountService.GetUserInfoAsync()).Email;
                News = await newsService.GetNewsAsync();
            });
        }
    }
}