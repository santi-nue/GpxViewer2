using System;
using System.Threading.Tasks;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace GpxViewer2.ViewServices;

public interface IErrorReportingViewService : IViewService
{
    Task ShowErrorDialogAsync(Exception exception);
}