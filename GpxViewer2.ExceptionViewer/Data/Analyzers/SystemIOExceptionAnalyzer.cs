using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GpxViewer2.ExceptionViewer.Data.Analyzers;

public class SystemIOExceptionAnalyzer : IExceptionAnalyzer
{
    /// <inheritdoc />
    public IEnumerable<ExceptionProperty>? ReadExceptionInfo(Exception ex)
    {
        switch (ex)
        {
            case FileLoadException fileLoadEx:
                yield return new ExceptionProperty("FileName", fileLoadEx.FileName ?? string.Empty);
                yield return new ExceptionProperty("FusionLog", fileLoadEx.FusionLog ?? string.Empty);
                break;

            case FileNotFoundException fileNotFoundEx:
                yield return new ExceptionProperty("FileName", fileNotFoundEx.FileName ?? string.Empty);
                yield return new ExceptionProperty("FusionLog", fileNotFoundEx.FusionLog ?? string.Empty);
                break;

            case DirectoryNotFoundException:
                break;
        }
    }

    /// <inheritdoc />
    public IEnumerable<Exception>? GetInnerExceptions(Exception ex)
    {
        return null;
    }
}