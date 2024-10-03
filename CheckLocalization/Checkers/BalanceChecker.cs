namespace CheckLocalization;

public sealed class BalanceChecker
{
    private const char Escape = '\u001b';

    private const string EscapeSequence = "\u001B\u001b";

    private const string LineEnd = "\r\n";

    private bool _isOpenSequence = false;

    public bool CheckBalance(ReadOnlySpan<char> line)
    {
        if (line.Length <= 2)
        {
            return true;
        }

        var esacpeCount = line.Count(Escape);
        var isUneven = esacpeCount % 2 == 1;
        if (isUneven)
        {
            return false; 
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
            _isOpenSequence = line.EndsWith(LineEnd);
            return _isOpenSequence;
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