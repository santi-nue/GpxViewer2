using System.Collections.Generic;
using GpxViewer2.Messages;
using GpxViewer2.Model;
using RolandK.InProcessMessaging;

namespace GpxViewer2.UseCases;

public class SelectGpxFilesUseCase(
    IInProcessMessagePublisher srvMessagePublisher)
{
    public void SelectGpxFiles(IReadOnlyList<LoadedGpxFile> gpxFiles)
    {
        srvMessagePublisher.Publish(new GpxFilesSelectedMessage(gpxFiles));
    }
}