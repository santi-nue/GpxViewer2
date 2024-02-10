using Avalonia;
using Avalonia.Input;
using GpxViewer2.Views.RouteSelection;
using RolandK.AvaloniaExtensions.Mvvm.Controls;

namespace GpxViewer2.Views;

public partial class RouteSelectionView : MvvmUserControl
{
    public RouteSelectionView()
    {
        this.InitializeComponent();
    }

    private void OnTreeView_DoubleTabbed(object? sender, TappedEventArgs e)
    {
        if ((e.Source is not StyledElement styledElement) ||
            (styledElement.DataContext is not RouteSelectionNode node) ||
            (this.DataContext is not RouteSelectionViewModel viewModel))
        {
            return;
        }

        viewModel.NotifyDoubleTabbed(node);
    }
}