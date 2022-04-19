using LearningCqrs.Features.TimeZones;
using LearningCqrs.Tests.Core;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LearningCqrs.Tests.Features.TimeZones;

public class InitTimeZoneTests : BaseUnitTest
{
    [Fact]
    public async void Create_correct_user()
    {
        var testContext = GetTestContext();

        var command = new InitTimeZone.InitTimeZoneCommand();
        await testContext.Mediator.Send(command);
        var timeZonesCreated = await testContext.DbContext.TimeZones.AnyAsync();
        Assert.True(timeZonesCreated);
    }
}