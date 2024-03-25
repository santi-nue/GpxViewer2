using System;
using System.IO;
using System.Linq;
using System.Web;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GpxViewer2.UseCases;
using Microsoft.Extensions.DependencyInjection;
using RolandK.AvaloniaExtensions.DependencyInjection;

namespace GpxViewer2;

public partial class App : Application
{
    public App()
    {
        this.UrlsOpened += this.OnUrlsOpened;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async void OnUrlsOpened(object? sender, UrlOpenedEventArgs e)
    {
        if (e.Urls.Length == 0)
        {
            return;
        }

        try
        {
            var fileUrl = new Uri(e.Urls.First(), UriKind.Absolute);
            var filePath = HttpUtility.UrlDecode(fileUrl.AbsolutePath);
            if (!File.Exists(filePath))
            {
                return;
            }

            var serviceProvider = this.GetServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var useCaseLoadFile = scope.ServiceProvider.GetRequiredService<LoadGpxFileUseCase>();
            await useCaseLoadFile.LoadGpxFileAsync(filePath);
        }
        catch
        {
            // TODO: Show error within a dialog
        }
    }
}
