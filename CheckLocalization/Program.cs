using CheckLocalization;

var noPause = args.Length == 1 && string.Equals(args[0], "-noPause", StringComparison.OrdinalIgnoreCase);

var result = CheckFiles();

if (!noPause)
{
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

return result;

int CheckFiles()
{
    var files = Directory.GetFiles(Environment.CurrentDirectory, "*.utxt", SearchOption.AllDirectories);
    var hasAnyErrors = false;
    var fileChecker = new FileChecker();

    foreach (var file in files)
    {
        var fileResult = fileChecker.CheckFile(file);

        if (fileResult.Success)
        {
            continue;
        }

        hasAnyErrors = true;
        PrintErrors(file, fileResult.Errors);
    }
    if (!hasAnyErrors)
    {
        Console.WriteLine("All files good");
        return 0;
    }
    return -1;
}

void PrintErrors(string file, IEnumerable<FileError> errors)
{
    var language = Path.GetFileName(Path.GetDirectoryName(file));
    var fileName = Path.GetFileName(file);
    Console.Error.WriteLine($"Error with: {language}\\{fileName}");
    foreach (var error in errors)
    {
        Console.Error.WriteLine(error.Error);
    }
    Console.Error.WriteLine();
}