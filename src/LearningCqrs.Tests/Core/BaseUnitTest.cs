namespace LearningCqrs.Tests.Core;

public abstract class BaseUnitTest
{
    public TestContext GetTestContext()
    {
        return new TestContext();
    }
}