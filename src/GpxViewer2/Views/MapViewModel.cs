using Avalonia.Controls;
using GpxViewer2.Messages;
using GpxViewer2.Services;
using GpxViewer2.UseCases;
using GpxViewer2.Util;
using GpxViewer2.Views.Maps;
using NSubstitute;
using RKMediaGallery.Controls;

namespace GpxViewer2.Views;

public partial class MapViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly MapViewModel EmptyViewModel = new(
        Substitute.For<IGpxFileRepositoryService>());

    private readonly IGpxFileRepositoryService _srvGpxFileRepository;

    private IMapsViewService? _attachedMapsViewService;
    
    /// <inheritdoc />
    public string Title { get; } = "Map";

    /// <inheritdoc />
    public Control CreateViewInstance()
    {
        return new MapView();
    }

    public MapViewModel(IGpxFileRepositoryService srvGpxFileRepository)
    {
        _srvGpxFileRepository = srvGpxFileRepository;
    }
    
    private void OnAttachedMapsViewService_RouteClicked(object? sender, RouteClickedEventArgs e)
    {
        using var scope = this.GetScopedService(out SelectGpxFilesUseCase useCase);

        if (e.ClickedGpxFile == null)
        {
            useCase.SelectGpxFiles([]);
            return;
        }
        
        useCase.SelectGpxFiles([e.ClickedGpxFile]);
    }
    
    public void OnMessageReceived(GpxFileRepositoryNodesLoadedMessage message)
    {
        var srvMaps = this.GetViewService<IMapsViewService>();
        srvMaps.AddAvailableGpxFiles(
            _srvGpxFileRepository.QueryAllLoadedFiles());
    }
    
    public void OnMessageReceived(GpxFilesSelectedMessage message)
    {
        var srvMaps = this.GetViewService<IMapsViewService>();
        srvMaps.SetSelectedGpxFile(message.GpxFiles);
    }

    /// <inheritdoc />
    protected override void OnAssociatedViewChanged(object? associatedView)
    {
        base.OnAssociatedViewChanged(associatedView);

        if (_attachedMapsViewService != null)
        {
            _attachedMapsViewService.RouteClicked -= OnAttachedMapsViewService_RouteClicked;
            _attachedMapsViewService = null;
        }
        
        if (associatedView is IMapsViewService attachedMapsViewService)
        {
            _attachedMapsViewService = attachedMapsViewService;
            _attachedMapsViewService.RouteClicked += OnAttachedMapsViewService_RouteClicked;
        }
    }
}