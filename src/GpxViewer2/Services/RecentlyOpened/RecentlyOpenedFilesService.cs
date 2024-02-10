using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GpxViewer2.Services.RecentlyOpened;

internal class RecentlyOpenedFilesService : IRecentlyOpenedFilesService
{
    private const string FILE_NAME = "recentlyOpened.json";

    private readonly string _applicationDirectoryName;
    private readonly int _maxFileCount;

    public RecentlyOpenedFilesService(string applicationDirectoryName, int maxFileCount)
    {
        if (maxFileCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxFileCount));
        }
        if (string.IsNullOrEmpty(applicationDirectoryName))
        {
            throw new ArgumentException(nameof(applicationDirectoryName));
        }

        _applicationDirectoryName = applicationDirectoryName;
        _maxFileCount = maxFileCount;
    }

    public async Task AddOpenedFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException(nameof(filePath));
        }

        var model = await ReadRecentlyOpenedFilesAsync();
        model.Files.RemoveAll(
            x => x.FullFilePath.Equals(filePath, StringComparison.InvariantCultureIgnoreCase));
        model.Files.Insert(0, new RecentlyOpenedFileModel(){ FullFilePath = filePath });

        while (model.Files.Count > _maxFileCount)
        {
            model.Files.RemoveAt(model.Files.Count - 1);
        }

        await WriteRecentlyOpenedFilesAsync(model);
    }

    public async Task<string> TryGetLastOpenedFileAsync()
    {
        var model = await ReadRecentlyOpenedFilesAsync();

        if (model.Files.Count > 0)
        {
            return model.Files[0].FullFilePath;
        }

        return string.Empty;
    }

    public async Task<IReadOnlyList<string>> GetAllRecentlyOpenedFilesAsync()
    {
        var model = await ReadRecentlyOpenedFilesAsync();
        return model.Files
            .Select(x => x.FullFilePath)
            .ToArray();
    }

    private async Task WriteRecentlyOpenedFilesAsync(RecentlyOpenedModel model)
    {
        var directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var appPath = Path.Combine(directory, _applicationDirectoryName);

        if (!Directory.Exists(appPath))
        {
            Directory.CreateDirectory(appPath);
        }

        var filePath = Path.Combine(appPath, FILE_NAME);
        await using var fileStream = File.Create(filePath);

        await JsonSerializer.SerializeAsync(
            fileStream, model,
            new JsonSerializerOptions(JsonSerializerDefaults.General)
            {
                WriteIndented = true
            });
    }

    private async Task<RecentlyOpenedModel> ReadRecentlyOpenedFilesAsync()
    {
        var directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var appPath = Path.Combine(directory, _applicationDirectoryName);

        if (!Directory.Exists(appPath))
        {
            return new RecentlyOpenedModel();
        }

        var filePath = Path.Combine(appPath, FILE_NAME);
        if (!File.Exists(filePath))
        {
            return new RecentlyOpenedModel();
        }

        try
        {
            await using var fileStream = File.OpenRead(filePath);
            var result = await JsonSerializer.DeserializeAsync<RecentlyOpenedModel>(fileStream);

            result ??= new RecentlyOpenedModel();
            return result;
        }
        catch (Exception)
        {
            return new RecentlyOpenedModel();
        }
    }
}