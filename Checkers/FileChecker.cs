using System.Text;

namespace CheckLocalization;

internal class FileChecker
{
    private readonly EncodingChecker _encodingChecker = new();
    private readonly LineEndingChecker _lineEndingChecker = new();
    private readonly BalanceChecker _balanceChecker = new();

    public FileResult CheckFile(string file)
    {
        var errors = CheckCore(file).ToList();

        var hasErrors = errors.Any();
        return new FileResult(!hasErrors, errors);
    }

    private IEnumerable<FileError> CheckCore(string file)
    {
        
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        var hasGoodEncoding = _encodingChecker.CheckEncoding(fileStream);
        if (!hasGoodEncoding)
        {
            yield return new FileError("Wrong encoding: Not UTF-16LE / UCS-2.");
            yield break;
        }

        var lines = ReadLines(file);

        var lineNumber = 1;
        foreach (var line in lines)
        {
            var hasGoodLineEnd = _lineEndingChecker.CheckLineEnds(line);
            if (!hasGoodLineEnd)
            {
                yield return new FileError($"Wrong line end, check line ends in \\r\\n: Line {lineNumber}");
            }

            var hasGoodBalance = _balanceChecker.CheckBalance(line);
            if (!hasGoodBalance)
            {
                yield return new FileError($"Unbalanced delimiters, not correctly surrounding localized text: Line {lineNumber}");
            }

            lineNumber++;
        }
    }

    static IEnumerable<string> ReadLines(string file)
    {
        using var reader = new StreamReader(file);
        var lineBuilder = new StringBuilder();
        int currentChar;
        while ((currentChar = reader.Read()) != -1)
        {
            var c = (char)currentChar;
            lineBuilder.Append(c);
            if (c != '\n')
            {
                continue;
            }
            yield return lineBuilder.ToString();
            lineBuilder.Clear();
        }
        // Handle the last line if it doesn't end with a newline
        if (lineBuilder.Length > 0)
        {
            yield return lineBuilder.ToString();
        }
    }
}

internal record struct FileError(string Error);
internal record struct FileResult(bool Success, IReadOnlyCollection<FileError> Errors);