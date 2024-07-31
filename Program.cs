using System.Text;
using CheckLocalization;

var files = Directory.GetFiles(Environment.CurrentDirectory, "*.utxt", SearchOption.AllDirectories);
var hasAnyErrors = false;
foreach (var str1 in files)
{
    var hasErrors = false;
    var isWrongEncoding = false;
    var hasWrongLineEnds = false;
    var hasUnbalancedDelimiters = false;
    var hasTabsInsteadOfSpaces = false;
    var wrongLineEndLines = new List<int>();
    var unbalanedDelimitersLines = new List<int>();
    var lines = new List<int>();
    var str2 = str1.Substring(Environment.CurrentDirectory.Length).Trim('\\', '/');
    var encoding = GetEncoding(str1);
    if (encoding != Encoding.Unicode)
    {
        hasErrors = true;
        isWrongEncoding = true;
    }
    var str3 = File.ReadAllText(str1);
    var strArray = File.ReadAllLines(str1);
    int[] numArray = str3.Replace("\r\n", "\u0001\u0001").IndexesOf("\n");
    if (numArray.Length > 0)
    {
        hasErrors = true;
        hasWrongLineEnds = true;
        foreach (var length in numArray)
        {
            wrongLineEndLines.Add(((IEnumerable<int>)str3.Substring(0, length).IndexesOf("\n")).Count() + 1);
        }
    }
    for (var index1 = 0; index1 < strArray.Length; ++index1)
    {
        if (strArray[index1].Count(x => x == '\u001B') % 4 != 0)
        {
            for (var index2 = index1 + 1; index2 < strArray.Length; ++index2)
            {
                var num = strArray[index2].Count(x => x == '\u001B');
                if (num != 0)
                {
                    if (num % 4 == 0)
                    {
                        hasErrors = true;
                        hasUnbalancedDelimiters = true;
                        unbalanedDelimitersLines.Add(index1 + 1);
                        index1 = index2;
                        break;
                    }
                    if (num % 2 == 0)
                    {
                        index1 = index2;
                        break;
                    }
                }
            }
        }
    }
    for (var index = 0; index < strArray.Length; ++index)
    {
        if (strArray[index].Contains<char>('\t'))
        {
            hasErrors = true;
            hasTabsInsteadOfSpaces = true;
            lines.Add(index + 1);
        }
    }
    if (hasErrors)
    {
        hasAnyErrors = true;
        Console.Error.WriteLine("Error with: " + str2);
        if (isWrongEncoding)
        {
            Console.Error.WriteLine($" - Not UTF-16LE / UCS-2 - (Detected: {GetEncodingFriendly(encoding)}).");
        }
        if (hasWrongLineEnds)
        {
            Console.Error.WriteLine(" - Contains \\n characters instead of \\r\\n, check line endings are Windows:");
            foreach (var line in wrongLineEndLines)
            {
                Console.Error.WriteLine("   - Line " + line);
            }
        }
        if (hasUnbalancedDelimiters)
        {
            Console.Error.WriteLine(" - Has unbalanced delimiters, \u001B\u001B not correctly surrounding localized text:");
            foreach (var line in unbalanedDelimitersLines)
            {
                Console.Error.WriteLine("   - Line " + line);
            }
        }
        if (hasTabsInsteadOfSpaces)
        {
            Console.Error.WriteLine(" - Has tabs instead of spaces:");
            foreach (var line in lines)
            {
                Console.Error.WriteLine("   - Line " + line);
            }
        }
        Console.Error.WriteLine();
    }
}
if (!hasAnyErrors)
{
    Console.WriteLine("All files good");
    return -1;
}
return 0;

static Encoding GetEncoding(string filename)
{
    var buffer = new byte[4];
    using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
        fileStream.Read(buffer, 0, 4);
    if (buffer[0] == 43 && buffer[1] == 47 && buffer[2] == 118)
        return Encoding.UTF7;
    if (buffer[0] == 239 && buffer[1] == 187 && buffer[2] == 191)
        return Encoding.UTF8;
    if (buffer[0] == byte.MaxValue && buffer[1] == 254)
        return Encoding.Unicode;
    if (buffer[0] == 254 && buffer[1] == byte.MaxValue)
        return Encoding.BigEndianUnicode;
    return buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 254 && buffer[3] == byte.MaxValue ? Encoding.UTF32 : Encoding.ASCII;
}

static string GetEncodingFriendly(Encoding encoding)
{
    if (encoding == Encoding.UTF7)
        return "UTF-7";
    if (encoding == Encoding.UTF8)
        return "UTF-8";
    if (encoding == Encoding.Unicode)
        return "UTF-16LE";
    if (encoding == Encoding.BigEndianUnicode)
        return "UTF-16BE";
    return encoding == Encoding.UTF32 ? "UTF-32" : "ASCII";
}