using GpxViewer2.Messages;
using GpxViewer2.Model;
using GpxViewer2.Model.GpxXmlExtensions;
using RolandK.InProcessMessaging;

namespace GpxViewer2.UseCases;

public class UpdateTourPropertyUseCase(IInProcessMessagePublisher messagePublisher)
{
    public void SetTourName(LoadedGpxFileTourInfo tour, string name)
    {
        if (tour.RawTrackOrRoute.Name != name)
        {
            tour.RawTrackOrRoute.Name = name;
            tour.File.ContentsChanged = true;

            messagePublisher.BeginPublish(
                new TourConfigurationStateChangedMessage([tour], true));
        }
    }

    public void SetTourDescription(LoadedGpxFileTourInfo tour, string description)
    {
        if (tour.RawTrackOrRoute.Description != description)
        {
            tour.RawTrackOrRoute.Description = description;
            tour.File.ContentsChanged = true;

            messagePublisher.BeginPublish(
                new TourConfigurationStateChangedMessage([tour], true));
        }
    }

    public void SetTourState(LoadedGpxFileTourInfo tour, GpxTrackState state)
    {
        if (tour.RawTourExtensionData.State != state)
        {
            tour.RawTourExtensionData.State = state;
            tour.File.ContentsChanged = true;

            messagePublisher.BeginPublish(
                new TourConfigurationStateChangedMessage([tour], true));
        }
    }
}