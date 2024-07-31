namespace CheckLocalization;

public static class Extensions
{
    public static int[] IndexesOf(this string str, string sub)
    {
        int[] array = new int[0];
        for (int startIndex = 0; startIndex < str.Length && startIndex + sub.Length <= str.Length; ++startIndex)
        {
            if (str.Substring(startIndex, sub.Length).Equals(sub))
            {
                Array.Resize<int>(ref array, array.Length + 1);
                array[array.Length - 1] = startIndex;
            }
        }
        return array;
    }
}