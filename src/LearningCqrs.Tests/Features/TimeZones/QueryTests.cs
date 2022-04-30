using System.Threading.Tasks;
using LearningCqrs.Features.TimeZones;
using LearningCqrs.Tests.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LearningCqrs.Tests.Features.TimeZones;

public class QueryTests : BaseUnitTest
{
    private async Task InitData(IMediator mediator)
    {
        var command = new InitTimeZone.InitTimeZoneCommand();
        await mediator.Send(command);
    }

    [Fact]
    public async Task Get_Timezones_ByGuid()
    {
        var testContext = GetTestContext();
        await InitData(testContext.Mediator);

        var firstTimeZone = await testContext.DbContext.TimeZones.FirstAsync();
        var command = new Query.QueryTimeZoneCommand
        {
            Filter = $"Id == \"{firstTimeZone.Id}\""
        };
        var result = await testContext.Mediator.Send(command);
        Assert.Single(result.Data);
        Assert.Equal(firstTimeZone.Id, result.Data[0].Id);
        Assert.Equal(firstTimeZone.Name, result.Data[0].Name);
    }

    [Fact]
    public async Task Get_Timezones_ByContains()
    {
        var testContext = GetTestContext();
        await InitData(testContext.Mediator);

        var command = new Query.QueryTimeZoneCommand
        {
            Filter = "Name.Contains(\"Mountain Standard Time\")"
        };
        var result = await testContext.Mediator.Send(command);
        Assert.Equal(3, result.Data.Length);
    }

    [Fact]
    public async Task Get_Timezones_Equal()
    {
        var testContext = GetTestContext();
        await InitData(testContext.Mediator);

        var command = new Query.QueryTimeZoneCommand
        {
            Filter = "Name == \"Mountain Standard Time\""
        };
        var result = await testContext.Mediator.Send(command);
        Assert.Single(result.Data);
    }

    [Fact]
    public async Task Get_Timezones_All()
    {
        var testContext = GetTestContext();
        await InitData(testContext.Mediator);

        var length = await testContext.DbContext.TimeZones.CountAsync();
        var command = new Query.QueryTimeZoneCommand {Take = length};
        var result = await testContext.Mediator.Send(command);
        Assert.Equal(length, result.Data.Length);
    }
}