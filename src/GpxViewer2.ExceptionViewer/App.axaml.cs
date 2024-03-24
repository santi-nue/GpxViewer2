using System;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using RolandK.AvaloniaExtensions.ExceptionHandling;

namespace GpxViewer2.ExceptionViewer;

public partial class App : ExceptionViewerApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
}