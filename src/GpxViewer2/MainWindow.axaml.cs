using GpxViewer2.ViewServices;
using RolandK.AvaloniaExtensions.Mvvm.Controls;

namespace GpxViewer2;

public partial class MainWindow : MvvmWindow
{
    public MainWindow()
    {
        this.InitializeComponent();
        
        this.ViewServices.Add(new ServiceProviderViewService(this));
    }
}