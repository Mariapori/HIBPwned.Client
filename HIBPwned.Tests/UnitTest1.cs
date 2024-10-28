namespace HIBPwned.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CheckArePasswordPwned()
    {
        Assert.IsTrue(HIBPwned.Client.IsPasswordPwned("demo"));
    }
}