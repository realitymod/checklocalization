namespace CheckLocalization;

internal class BalanceChecker
{
    private const char EscapeChar = '\u001B';
    private const string EscapeSequence = "\u001B\u001b";

    public bool CheckBalance(ReadOnlySpan<char> line)
    {
        var escapeCharCount = line.Count(EscapeChar);
        if (escapeCharCount != 4)
        {
            return false;
        }

        var firstPair = line.IndexOf(EscapeSequence);
        if (firstPair == -1)
        {
            return false;
        }

        var lastPair = line.LastIndexOf(EscapeChar);
        if (firstPair == lastPair)
        {
            return false;
        }

        var hasAtleastOneChar = lastPair > firstPair + 1;
        return hasAtleastOneChar;
    }

}