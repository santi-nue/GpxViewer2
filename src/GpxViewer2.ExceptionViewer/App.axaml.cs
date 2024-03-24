using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using RolandK.AvaloniaExtensions.ExceptionHandling;
using RolandK.AvaloniaExtensions.ExceptionHandling.Data;

namespace GpxViewer2.ExceptionViewer;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ExceptionInfo? exceptionInfo = null;
            try
            {
                var filePath = desktop!.Args![0];
            
                using var inStream = File.OpenRead(filePath);
                exceptionInfo = JsonSerializer.Deserialize<ExceptionInfo>(inStream);
            }
            catch (Exception)
            {
                // Nothing we can do here
            }
            
            if (exceptionInfo == null)
            {
                // We need to wait some time. Otherwise, an exception is thrown after Shutdown()
                Task.Delay(100).ContinueWith(_ =>
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        desktop.Shutdown();
                    });
                });
                base.OnFrameworkInitializationCompleted();
                return;
            }
            
            var dialog = new UnexpectedErrorDialog();
            dialog.DataContext = exceptionInfo;
            desktop.MainWindow = dialog;
        }

        base.OnFrameworkInitializationCompleted();
    }
}