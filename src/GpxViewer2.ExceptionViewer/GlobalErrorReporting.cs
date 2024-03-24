using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using RolandK.AvaloniaExtensions.ExceptionHandling.Data;

namespace GpxViewer2.ExceptionViewer;

public static class GlobalErrorReporting
{
    /// <summary>
    /// Tries to show an error dialog with some exception details.
    /// If it is not possible for any reason, this method simply does nothing.
    /// </summary>
    /// <param name="exception">The exception to be shown to the user.</param>
    /// <param name="applicationName">The name of the application. It should be a technical name, the method uses it to create a directory in the filesystem.</param>
    public static void TryShowGlobalExceptionDialogInAnotherProcess(Exception exception, string applicationName)
    {
        try
        {
            // Write exception details to a temporary file
            var errorDirectoryPath = GetErrorFileDirectoryAndEnsureCreated(applicationName);
            var errorFilePath = GenerateErrorFilePath(errorDirectoryPath);
            
            WriteExceptionInfoToFile(exception, errorFilePath);
            try
            {
                if (!TryFindViewerExecutable(out var executablePath))
                {
                    return;
                }
                
                ShowGlobalException(errorFilePath, executablePath);
            }
            finally
            {
                // Delete the temporary file
                File.Delete(errorFilePath);
            }
        }
        catch(Exception)
        {
            // Nothing to do here..
        }
    }
    
    private static string GetErrorFileDirectoryAndEnsureCreated(string applicationName)
    {
        var errorDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            applicationName);
        if (!Directory.Exists(errorDirectoryPath))
        {
            Directory.CreateDirectory(errorDirectoryPath);
        }
        return errorDirectoryPath;
    }

    private static string GenerateErrorFilePath(string errorDirectoryPath)
    {
        string errorFilePath;
        do
        {
            var errorGuid = Guid.NewGuid();
            errorFilePath = Path.Combine(
                errorDirectoryPath,
                $"Error-{errorGuid}.err");
        } while (File.Exists(errorFilePath));

        return errorFilePath;
    }

    private static void WriteExceptionInfoToFile(Exception exception, string targetFileName)
    {
        using var outStream = File.Create(targetFileName);
        
        var exceptionInfo = new ExceptionInfo(exception);
        JsonSerializer.Serialize(
            outStream, 
            exceptionInfo, 
            new JsonSerializerOptions(JsonSerializerDefaults.General)
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
    }
    
    private static bool TryFindViewerExecutable(out string executablePath)
    {
        executablePath = string.Empty;

        var executingAssembly = Assembly.GetExecutingAssembly();
        var executingAssemblyDirectory = Path.GetDirectoryName(executingAssembly.Location);
        var executingAssemblyName = executingAssembly.GetName().Name;
        if (string.IsNullOrEmpty(executingAssemblyDirectory) ||
            string.IsNullOrEmpty(executingAssemblyName))
        {
            return false;
        }
        
        var executablePathCheck = Path.Combine(executingAssemblyDirectory, executingAssemblyName);
        if (!Path.Exists(executablePathCheck))
        {
            executablePathCheck += ".exe";
            if (!Path.Exists(executablePathCheck))
            {
                return false;
            }
        }

        executablePath = executablePathCheck;
        return true;
    }
    
    private static void ShowGlobalException(string exceptionDetailsFilePath, string executablePath)
    {
        var processStartInfo = new ProcessStartInfo(
            executablePath,
            $"\"{exceptionDetailsFilePath}\"");
        processStartInfo.ErrorDialog = false;
        processStartInfo.UseShellExecute = false;

        var childProcess = Process.Start(processStartInfo);
        if (childProcess != null)
        {
            childProcess.WaitForExit();
        }
    }
}