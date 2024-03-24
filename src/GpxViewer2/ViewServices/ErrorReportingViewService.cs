using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using GpxViewer2.ExceptionViewer;
using RolandK.AvaloniaExtensions.ExceptionHandling.Data;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace GpxViewer2.ViewServices;

public class ErrorReportingViewService(Window hostWindow) : ViewServiceBase, IErrorReportingViewService
{
    /// <inheritdoc />
    public async Task ShowErrorDialogAsync(Exception exception)
    {
        var exceptionInfo = new ExceptionInfo(exception);
        
        var dialog = new UnexpectedErrorDialog();
        dialog.DataContext = exceptionInfo;
        await dialog.ShowDialog(hostWindow);
    }
}