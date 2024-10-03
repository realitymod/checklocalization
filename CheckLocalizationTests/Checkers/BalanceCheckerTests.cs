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
    }

    [TestMethod]
    public void CheckBalanceTest_Open()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001bINVALID");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_Close()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("INVALID\u001b\u001b");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_SingleEsc()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001bINVALID");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_SingleEscOpenClose()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001bINVALID\u001b");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_ImbalancedOpen()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001bINVALID\u001b\u001b");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_ImbalancedOpen2()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001b\u001bINVALID\u001b\u001b");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_ImbalancedClose()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001bINVALID\u001b");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_ImbalancedClose2()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001bINVALID\u001b\u001b\u001b");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_EmptyOpenClose()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001b\u001b\u001b");
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_NonEmptyOpenClose()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001b \u001b\u001b");
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

    [TestMethod]
    public void CheckBalanceTest_EmptyMultiLine()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\u001b\u001b\r\n");
        Assert.IsTrue(isValid);

        isValid = checker.CheckBalance("\u001b\u001b");
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void CheckBalanceTest_EmptyLine()
    {
        var checker = new BalanceChecker();
        var isValid = checker.CheckBalance("\r\n");
        Assert.IsTrue(isValid);
    }
}