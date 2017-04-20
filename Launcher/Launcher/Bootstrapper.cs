using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using Launcher.Contracts;
using Launcher.Models;
using Launcher.Properties;
using Launcher.ViewModels;
using Launcher.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RestSharp;

namespace Launcher
{
    /// <summary>
    /// The application bootstrapper. Here's the application configuration.
    /// </summary>
    public class Bootstrapper : BootstrapperBase
    {
        private IContainer container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            //Preferable json serialization in every RestClient
            SimpleJson.CurrentJsonSerializerStrategy = new CamelCaseJsonSerializerStrategy();

            //Generate if null client token (unique GUID)
            EnsureClientTokenExists();

            var builder = new ContainerBuilder();
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            //Register the shell
            builder.RegisterInstance(new ShellView()).As<MetroWindow>().AsSelf().SingleInstance();
            builder.RegisterType<ShellViewModel>().As<IShell>().SingleInstance();

            //Register tabs
            builder
                .RegisterAssemblyTypes(currentAssembly)
                .Where(type => type.IsAssignableTo<ITab>())
                .AsSelf()
                .As<ITab>()
                .SingleInstance();

            //Register services
            builder
                .RegisterAssemblyTypes(currentAssembly)
                .Where(x => x.FullName.Contains("Services"))
                .AsImplementedInterfaces()
                .SingleInstance();

            //Register others
            builder.RegisterType<RestClient>().As<IRestClient>();
            builder.Register(x => new TokenPayload(Settings.Default.AccessToken, Settings.Default.ClientToken)).AsSelf();
            builder.Register(x => DialogCoordinator.Instance).As<IDialogCoordinator>();

            container = builder.Build();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }

        protected override void BuildUp(object instance)
        {
            container.InjectProperties(instance);
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrEmpty(key) ? container.Resolve(service) : container.ResolveKeyed(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            Type enumerable = typeof(IEnumerable<>).MakeGenericType(service);
            return (IEnumerable<object>)container.Resolve(enumerable);
        }

        private static void EnsureClientTokenExists()
        {
            if (string.IsNullOrEmpty(Settings.Default.ClientToken))
            {
                Settings.Default.ClientToken = Guid.NewGuid().ToString();
                Settings.Default.Save();
            }
        }
    }
}