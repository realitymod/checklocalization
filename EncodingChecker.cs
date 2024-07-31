namespace CheckLocalization;

internal class EncodingChecker
{
    public bool CheckEncoding(string file)
    {
        var buffer = new byte[4];
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        const int byteCount = 2;
        var read = fileStream.Read(buffer, 0, byteCount);
        if (read != byteCount)
        {
            return false;
        }

        var isUtf16Le = buffer[0] == 0xFF && buffer[1] == 0xFE;

        return isUtf16Le;
    }
}