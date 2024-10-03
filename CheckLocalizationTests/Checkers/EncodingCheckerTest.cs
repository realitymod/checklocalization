using CheckLocalization;
using JetBrains.Annotations;

namespace CheckLocalizationTests.Checkers;

[TestClass]
[TestSubject(typeof(EncodingChecker))]
public class EncodingCheckerTest
{

    [TestMethod]
    public void CheckEncoding_Utf16LE()
    {
        var checker = new EncodingChecker();

        using var stream = new MemoryStream([0xFF, 0xFE]);

        var isValid = checker.CheckEncoding(stream);
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void CheckEncoding_Utf16BE()
    {
        var checker = new EncodingChecker();

        using var stream = new MemoryStream([0xFE, 0xFF]);

        var isValid = checker.CheckEncoding(stream);
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckEncoding_Utf8()
    {
        var checker = new EncodingChecker();

        using var stream = new MemoryStream([0xEF, 0xBB, 0xBF]);

        var isValid = checker.CheckEncoding(stream);
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckEncoding_EmptyFile()
    {
        var checker = new EncodingChecker();

        using var stream = new MemoryStream([]);

        var isValid = checker.CheckEncoding(stream);
        Assert.IsFalse(isValid);
    }
}