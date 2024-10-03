using CheckLocalization;
using JetBrains.Annotations;

namespace CheckLocalizationTests.Checkers;

[TestClass]
[TestSubject(typeof(LineEndingChecker))]
public class LineEndingCheckerTest
{

    [TestMethod]
    public void CheckLineEnding_CRLF()
    {
        var checker = new LineEndingChecker();

        var line = "FOO\r\n";

        var isValid = checker.CheckLineEnds(line);
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void CheckLineEnding_CR()
    {
        var checker = new LineEndingChecker();

        var line = "FOO\r";

        var isValid = checker.CheckLineEnds(line);
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckLineEnding_LF()
    {
        var checker = new LineEndingChecker();

        var line = "FOO\n";

        var isValid = checker.CheckLineEnds(line);
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckLineEnding_NoEnd()
    {
        var checker = new LineEndingChecker();

        var line = "FOO";

        var isValid = checker.CheckLineEnds(line);
        Assert.IsFalse(isValid);
    }
}