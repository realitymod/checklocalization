using CheckLocalization;
using JetBrains.Annotations;

namespace CheckLocalizationTests.Checkers;

[TestClass]
[TestSubject(typeof(BalanceChecker))]
public class BalanceCheckerTests
{
    [TestMethod]
    public void CheckBalanceTest_OpenClose()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001bVALID\u001b\u001b");
        Assert.IsTrue(isValid);

        isValid = checker.CheckBalance("\u001bINVALID\u001b\u001b");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_EmptyOpenClose()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001b\u001b\u001b");
        Assert.IsFalse(isValid);

        isValid = checker.CheckBalance("\u001b\u001b \u001b\u001b");
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_MultiLine()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001bVALID\r\n");
        Assert.IsTrue(isValid);

        isValid = checker.CheckBalance("VALID\u001b\u001b");
        Assert.IsTrue(isValid);
    }
}