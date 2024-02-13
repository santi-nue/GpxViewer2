﻿using Avalonia;
using System;
using GpxViewer2.Model.GpxXmlExtensions;
using GpxViewer2.Services;
using GpxViewer2.Services.GpxFileStore;
using GpxViewer2.Services.RecentlyOpened;
using GpxViewer2.UseCases;
using GpxViewer2.Views;
using Microsoft.Extensions.DependencyInjection;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using RolandK.AvaloniaExtensions.DependencyInjection;
using RolandK.Formats.Gpx;
using RolandK.InProcessMessaging;

namespace GpxViewer2;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current
            .Register<FontAwesomeIconProvider>();
        
        // Register GpxFile extensions
        GpxFile.RegisterExtensionType(typeof(TrackExtension));
        GpxFile.RegisterExtensionType(typeof(RouteExtension));
        GpxFile.RegisterNamespace("rkgpxv", "http://gpxviewer.rolandk.net/");
        
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseDependencyInjection(services =>
            {
                // Services
                var inProcessMessenger = new InProcessMessenger();
                services.AddSingleton<IInProcessMessageSubscriber>(_ => inProcessMessenger);
                services.AddSingleton<IInProcessMessagePublisher>(_ => inProcessMessenger);
                services.AddSingleton<IRecentlyOpenedFilesService>(
                    _ => new RecentlyOpenedFilesService(".RKMediaGallery", 5));
                services.AddSingleton<IGpxFileRepositoryService, GpxFileRepositoryService>();

                // ViewModels
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<MapViewModel>();
                services.AddTransient<RouteDetailViewModel>();
                services.AddTransient<RouteSelectionViewModel>();

                // UseCases
                services.AddScoped<CloseFileOrDirectoryUseCase>();
                services.AddScoped<LoadGpxFileUseCase>();
                services.AddScoped<LoadGpxDirectoryUseCase>();
                services.AddScoped<SelectGpxToursUseCase>();
            });
    }
}