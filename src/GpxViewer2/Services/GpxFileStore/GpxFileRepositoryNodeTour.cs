using GpxViewer2.Model;
using GpxViewer2.ValueObjects;

namespace GpxViewer2.Services.GpxFileStore
{
    internal class GpxFileRepositoryNodeTour(LoadedGpxFile parentFile, LoadedGpxFileTourInfo tour)
        : GpxFileRepositoryNode
    {
        /// <inheritdoc />
        public override bool CanSave => false;

        public override bool IsDirectory => false;

        /// <inheritdoc />
        public override FileOrDirectoryPath Source => FileOrDirectoryPath.Empty;

        /// <inheritdoc />
        protected override bool HasThisNodesContentsChanged()
        {
            return parentFile.ContentsChanged;
        }

        /// <inheritdoc />
        protected override string GetNodeText()
        {
            return tour.RawTrackOrRoute.Name ?? "-";
        }

        /// <inheritdoc />
        public override LoadedGpxFile GetAssociatedGpxFile()
        {
            return parentFile;
        }

        /// <inheritdoc />
        public override LoadedGpxFileTourInfo GetAssociatedTour()
        {
            return tour;
        }
    }
}
