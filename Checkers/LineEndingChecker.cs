namespace CheckLocalization;

internal sealed class LineEndingChecker
{
    public bool CheckLineEnds(ReadOnlySpan<char> line)
    {
        var endsInCrLf = line.EndsWith("\r\n");
        var endsInLf = line.EndsWith("\n");
        return endsInCrLf || !endsInLf;
    }
}