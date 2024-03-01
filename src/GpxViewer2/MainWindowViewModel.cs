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
        Substitute.For<IRecentlyOpenedService>(),
        Substitute.For<IGpxFileRepositoryService>());

    private readonly IRecentlyOpenedService _srvRecentlyOpened;
    private readonly IGpxFileRepositoryService _srvGpxFileRepository;

    private bool _closeAllowed = false;

    [ObservableProperty]
    private IReadOnlyList<RecentlyOpenedFileOrDirectoryModel> _recentlyOpenedEntries = [];

    public MainWindowViewModel(
        IRecentlyOpenedService srvRecentlyOpened,
        IGpxFileRepositoryService srvGpxFileRepository)
    {
        _srvRecentlyOpened = srvRecentlyOpened;
        _srvGpxFileRepository = srvGpxFileRepository;
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
    private async Task LoadRecentlyOpenedAsync(RecentlyOpenedFileOrDirectoryModel recentlyOpenedEntry)
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

    [RelayCommand]
    private async Task SaveAllAsync()
    {
        await this.WrapWithErrorHandlingAsync(async () =>
        {
            using var scope = this.GetScopedService(out SaveNodeChangesUseCase useCase);

            var allNodes = _srvGpxFileRepository.GetAllLoadedNodes();
            await useCase.SaveChangesAsync(allNodes);
        });
    }

    public bool NotifyWindowClosing()
    {
        if (_closeAllowed) { return true; }
        
        var allNodes = _srvGpxFileRepository.GetAllLoadedNodes();
        var anyContentsChanged = allNodes.Any(x => x.ContentsChanged);
        if (!anyContentsChanged)
        {
            return true;
        }
        
        this.TriggerSaveBeforeExit();

        return false;
    }

    private async void TriggerSaveBeforeExit()
    {
        await this.WrapWithErrorHandlingAsync(async () =>
        {
            var srvMessageBox = base.GetViewService<IMessageBoxViewService>();
            var result = await srvMessageBox.ShowAsync(
                "Unsaved Changes",
                "There are unsaved changes. Do you want to save them before closing?",
                MessageBoxButtons.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    await SaveAllAsync();

                    _closeAllowed = true;
                    this.CloseHostWindow();
                    break;

                case MessageBoxResult.No:
                    _closeAllowed = true;
                    this.CloseHostWindow();
                    break;

                case MessageBoxResult.Cancel:
                    return;
            }
        });
    }

    private async void OnMessageReceived(GpxFileRepositoryNodesLoadedMessage message)
    {
        await this.WrapWithErrorHandlingAsync(async () =>
        {
            this.RecentlyOpenedEntries = 
                await _srvRecentlyOpened.GetAllRecentlyOpenedAsync();
        });
    }

    private void OnMessageReceived(TourConfigurationStateChangedMessage message)
    {
        if (message.DataChanged)
        {
            _closeAllowed = false;
        }
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