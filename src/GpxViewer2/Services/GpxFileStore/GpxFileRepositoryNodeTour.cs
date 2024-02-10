using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer2.Model;

namespace GpxViewer2.Services.GpxFileStore
{
    internal class GpxFileRepositoryNodeTour(LoadedGpxFile parentFile, LoadedGpxFileTourInfo tour)
        : GpxFileRepositoryNode
    {
        /// <inheritdoc />
        public override bool CanSave => false;

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