using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GpxViewer2.Messages;
using GpxViewer2.Services;
using GpxViewer2.Services.RecentlyOpened;
using GpxViewer2.UseCases;
using GpxViewer2.Util;
using NSubstitute;
using RolandK.AvaloniaExtensions.ViewServices;

namespace GpxViewer2;

public partial class MainWindowViewModel : OwnViewModelBase
{
    public static readonly MainWindowViewModel EmptyViewModel = new(
        Substitute.For<IRecentlyOpenedService>());

    private readonly IRecentlyOpenedService _srvRecentlyOpened;

    [ObservableProperty]
    private IReadOnlyList<RecentlyOpenedFileOrDirectoryModel> _recentlyOpenedEntries = [];

    public MainWindowViewModel(IRecentlyOpenedService srvRecentlyOpened)
    {
        _srvRecentlyOpened = srvRecentlyOpened;
    }
    
    [RelayCommand]
    private async Task LoadFileAsync()
    {
        await this.WrapWithErrorHandlingAsync(async () =>
        {
            using var scope = this.GetScopedService(out LoadGpxFileUseCase useCase);
            await useCase.LoadGpxFileAsync(
                this.GetViewService<IOpenFileViewService>());
        });
    }
    
    [RelayCommand]
    private async Task LoadDirectoryAsync()
    {
        await this.WrapWithErrorHandlingAsync(async () =>
        {
            using var scope = this.GetScopedService(out LoadGpxDirectoryUseCase useCase);
            await useCase.LoadGpxDirectoryAsync(
                this.GetViewService<IOpenDirectoryViewService>());
        });
    }

    [RelayCommand]
    private async Task LoadRecentlyOpened(RecentlyOpenedFileOrDirectoryModel recentlyOpenedEntry)
    {
        switch (recentlyOpenedEntry.Type)
        {
            case RecentlyOpenedType.Directory:
                {
                    using var scope = this.GetScopedService(out LoadGpxDirectoryUseCase useCase);
                    await useCase.LoadGpxDirectoryAsync(recentlyOpenedEntry.FullPath);
                }
                break;
            
            case RecentlyOpenedType.File:
                {
                    using var scope = this.GetScopedService(out LoadGpxFileUseCase useCase);
                    await useCase.LoadGpxFileAsync(recentlyOpenedEntry.FullPath);
                }
                break;
        }
    }

    private async void OnMessageReceived(GpxFileRepositoryNodesLoadedMessage message)
    {
        await this.WrapWithErrorHandlingAsync(async () =>
        {
            this.RecentlyOpenedEntries = 
                await _srvRecentlyOpened.GetAllRecentlyOpenedAsync();
        });
    }

    /// <inheritdoc />
    protected override async void OnAssociatedViewChanged(object? associatedView)
    {
        base.OnAssociatedViewChanged(associatedView);

        if (associatedView != null)
        {
            await this.WrapWithErrorHandlingAsync(async () =>
            {
                this.RecentlyOpenedEntries = 
                    await _srvRecentlyOpened.GetAllRecentlyOpenedAsync();
            });
        }
    }
}