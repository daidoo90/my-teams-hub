namespace MyTeamsHub.IntegrationTests.Core.Extensions;

public static class Int32Extensions
{
    public static void ShouldBe(this int value, int expected)
    {
        Assert.Equal(expected, value);
    }
}
