using System.Threading.Tasks;
using LearningCqrs.Features.TimeZones;
using LearningCqrs.Tests.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LearningCqrs.Tests.Features.TimeZones;

public class GetTimeZonesTests : BaseUnitTest
{
    private async Task InitData(IMediator mediator){
         var command = new InitTimeZone.InitTimeZoneCommand();
        await mediator.Send(command);
    }
    
    [Fact]
    public async Task Get_Timezones_ByGuid()
    {
        var testContext = GetTestContext();
        await InitData(testContext.Mediator);

        var firstTimeZone = await testContext.DbContext.TimeZones.FirstAsync();
        var command = new GetTimeZones.GetTimeZonesQuery(firstTimeZone.Id, "", "");
        var result = await testContext.Mediator.Send(command);
        Assert.Single(result);
        Assert.Equal(firstTimeZone.Id, result[0].Id);
        Assert.Equal(firstTimeZone.Name, result[0].Name);
    }
    
    [Fact]
    public async Task Get_Timezones_ByContains()
    {
        var testContext = GetTestContext();
        await InitData(testContext.Mediator);
        
        var command = new GetTimeZones.GetTimeZonesQuery(null, "Mountain Standard Time", "");
        var result = await testContext.Mediator.Send(command);
        Assert.Equal(3, result.Length);
    }
    
    [Fact]
    public async Task Get_Timezones_Equal()
    {
        var testContext = GetTestContext();
        await InitData(testContext.Mediator);

        var command = new GetTimeZones.GetTimeZonesQuery(null, "", "Mountain Standard Time");
        var result = await testContext.Mediator.Send(command);
        Assert.Single(result);
    }
    
    [Fact]
    public async Task Get_Timezones_All()
    {
        var testContext = GetTestContext();
        await InitData(testContext.Mediator);

        var length = await testContext.DbContext.TimeZones.CountAsync();
        var command = new GetTimeZones.GetTimeZonesQuery(null, null, "");
        var result = await testContext.Mediator.Send(command);
        Assert.Equal(length, result.Length);
    }
}