namespace CheckLocalization;

internal class LineEndingChecker
{
    public bool CheckLineEnds(ReadOnlySpan<char> line)
    {
        var endsInCrLf = line.EndsWith("\r\n");
        var endsInLf = line.EndsWith("\n");
        return endsInCrLf || !endsInLf;
    }
}