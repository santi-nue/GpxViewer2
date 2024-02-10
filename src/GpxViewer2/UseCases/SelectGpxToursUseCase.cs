using System.Collections.Generic;
using GpxViewer2.Messages;
using GpxViewer2.Model;
using RolandK.InProcessMessaging;

namespace GpxViewer2.UseCases;

public class SelectGpxToursUseCase(
    IInProcessMessagePublisher srvMessagePublisher)
{
    public void SelectGpxTours(IReadOnlyList<LoadedGpxFileTourInfo> gpxTours)
    {
        srvMessagePublisher.Publish(new GpxFilesSelectedMessage(gpxTours));
    }
}