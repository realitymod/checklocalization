namespace CheckLocalization;

internal class LineEndingChecker
{
    public bool CheckLineEnds(ReadOnlySpan<char> line)
    {
        var endsInCrLf = line.EndsWith("\r\n");
        return endsInCrLf;
    }
}