using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using GpxViewer2.Views.RouteSelection;
using RolandK.AvaloniaExtensions.Mvvm.Controls;

namespace GpxViewer2.Views;

public partial class RouteSelectionView : MvvmUserControl, IRouteSelectionViewService
{
    /// <inheritdoc />
    public event EventHandler? NodeSelectionChanged;
    
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

    /// <inheritdoc />
    public IReadOnlyList<RouteSelectionNode> GetSelectedNodes()
    {
        return this.CtrlNodeTree.SelectedItems
            .Cast<RouteSelectionNode>()
            .ToArray();
    }

    private void OnCtrlNodeTree_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        this.NodeSelectionChanged?.Invoke(this, EventArgs.Empty);
    }
}