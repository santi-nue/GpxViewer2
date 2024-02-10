using System.Collections.Generic;
using System.Threading.Tasks;

namespace GpxViewer2.Services;

public interface IRecentlyOpenedFilesService
{
    Task AddOpenedFileAsync(string filePath);

    Task<string> TryGetLastOpenedFileAsync();

    Task<IReadOnlyList<string>> GetAllRecentlyOpenedFilesAsync();
}