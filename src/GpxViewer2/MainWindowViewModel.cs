using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using GpxViewer2.UseCases;
using GpxViewer2.Util;
using RolandK.AvaloniaExtensions.ViewServices;

namespace GpxViewer2;

public partial class MainWindowViewModel : OwnViewModelBase
{
    public static readonly MainWindowViewModel EmptyViewModel = new();

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
}