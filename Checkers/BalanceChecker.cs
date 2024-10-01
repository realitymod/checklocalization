namespace CheckLocalization;

public sealed class BalanceChecker
{
    private const string EscapeSequence = "\u001B\u001b";
    // TODO Check for single escape char

    private bool _isOpenSequence = false;

    public bool CheckBalance(ReadOnlySpan<char> line)
    {
        if (line.Length <= 2)
        {
            return true;
        }

        if (_isOpenSequence)
        {
            return CheckForCloseSequence(line);
        }

        return CheckForOpenSequence(line);
    }

    private bool CheckForOpenSequence(ReadOnlySpan<char> line)
    {
        var firstPair = line.IndexOf(EscapeSequence);
        if (firstPair == -1)
        {
            return false;
        }
        var lastPair = line.LastIndexOf(EscapeSequence);
        var isOpenClose = firstPair != lastPair;
        if (!isOpenClose)
        {
            _isOpenSequence = true;
            return true;
        }

        var hasAtleastOneChar = lastPair > firstPair + EscapeSequence.Length;
        return hasAtleastOneChar;
    }

    private bool CheckForCloseSequence(ReadOnlySpan<char> line)
    {
        var firstPair = line.IndexOf(EscapeSequence);
        if (firstPair == -1)
        {
            return true;
        }

        var lastPair = line.LastIndexOf(EscapeSequence);
        var isOpenClose = firstPair != lastPair;
        if (isOpenClose)
        {
            return false;
        }

        _isOpenSequence = false;
        return true;
    }
}