using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Text;
using TollTracker.Configuration;
using Serilog.Extensions.Logging;
using TollTracker.Services;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Serilog;
using Serilog.Core;
using TollTracker.Metadata;
using Newtonsoft.Json;
using TollTracker.Enums;
using System.Collections.ObjectModel;
using TollTracker.Models;

namespace TollTracker.UnitTests.Services;

[TestClass]
public class TollServiceTests
{
    private static string s_deserializationFailedLogMessage = $"Failed to deserialize the JSON object to a {typeof(HolidayDayInfo).Name}";
    private static readonly string s_deserializationNullLogMessage = "Deserialized HolidayInfo object is null.";
    private static readonly InvalidOperationException s_deserializationFailedException =
        new(s_deserializationNullLogMessage);
    private static readonly HttpRequestException s_expectedException = new("Exception has been thrown");
    private static readonly Logger s_serilogLogger = new LoggerConfiguration()
        .MinimumLevel.Warning()
        .WriteTo.TestCorrelator(restrictedToMinimumLevel: LogEventLevel.Warning)
        .CreateLogger();

    private readonly CancellationToken _token = new CancellationTokenSource().Token;
    private readonly HolidayApiOptions holidayApiOptions = new()
    {
        BaseUrl = new Uri("https://test123.com"),
    };

    private readonly TollPriceOptions tollPriceOptions = new TollPriceOptions
    {
        TestData = new ReadOnlyCollection<TollPriceModel>(new List<TollPriceModel>
        {
        new TollPriceModel { StartTime = TimeOnly.Parse("06:00"), EndTime = TimeOnly.Parse("06:29"), Price = 8 },
        new TollPriceModel { StartTime = TimeOnly.Parse("06:30"), EndTime = TimeOnly.Parse("06:59"), Price = 13 },
        new TollPriceModel { StartTime = TimeOnly.Parse("07:00"), EndTime = TimeOnly.Parse("07:59"), Price = 18 },
        new TollPriceModel { StartTime = TimeOnly.Parse("08:00"), EndTime = TimeOnly.Parse("08:29"), Price = 13 },
        new TollPriceModel { StartTime = TimeOnly.Parse("08:30"), EndTime = TimeOnly.Parse("14:59"), Price = 8 },
        new TollPriceModel { StartTime = TimeOnly.Parse("15:00"), EndTime = TimeOnly.Parse("15:29"), Price = 13 },
        new TollPriceModel { StartTime = TimeOnly.Parse("15:30"), EndTime = TimeOnly.Parse("16:59"), Price = 18 },
        new TollPriceModel { StartTime = TimeOnly.Parse("17:00"), EndTime = TimeOnly.Parse("17:59"), Price = 13 },
        new TollPriceModel { StartTime = TimeOnly.Parse("18:00"), EndTime = TimeOnly.Parse("18:29"), Price = 8 }
        })
    };

    private HttpMessageHandlerFaker _handlerFaker = null!;
    private TollService _tollServiceFaker = null!;
    private IOptions<HolidayApiOptions> _holidayApiOptionsFaker = null!;
    private IOptions<TollPriceOptions> _tollPriceOptionsFaker = null!;
    private HttpClient _httpClientFaker = null!;
    private ILogger<TollService> _loggerFaker = null!;

    [TestInitialize]
    public void StartUp()
    {
        _handlerFaker = A.Fake<HttpMessageHandlerFaker>(x => x.Strict(StrictFakeOptions.AllowObjectMethods));
        _httpClientFaker = A.Fake<HttpClient>(x => x.Strict(StrictFakeOptions.AllowObjectMethods));

        _holidayApiOptionsFaker = A.Fake<IOptions<HolidayApiOptions>>(x => x.Strict(StrictFakeOptions.AllowObjectMethods));
        A.CallTo(() => _holidayApiOptionsFaker.Value).Returns(holidayApiOptions);

        _tollPriceOptionsFaker = A.Fake<IOptions<TollPriceOptions>>(x => x.Strict(StrictFakeOptions.AllowObjectMethods));
        A.CallTo(() => _tollPriceOptionsFaker.Value).Returns(tollPriceOptions);

        _loggerFaker = A.Fake<ILogger<TollService>>(x => x.Strict(StrictFakeOptions.AllowObjectMethods));

        _tollServiceFaker = A.Fake<TollService>(x => x.Strict(StrictFakeOptions.AllowObjectMethods)
        .WithArgumentsForConstructor([_httpClientFaker, _holidayApiOptionsFaker, _tollPriceOptionsFaker, _loggerFaker]));

    }

    [TestMethod]
    [DataRow("2024-04-13")]   // Saturday
    [DataRow("2024-11-03")]   // Sunday
    [DataRow("2024-06-22")]   // Saturday
    [DataRow("2024-06-23")]   // Sunday 
    [DataRow("2024-06-29")]   // Saturday
    [DataRow("2024-06-30")]   // Sunday 
    public async Task IsDateExemptFromTollAsync_DateIndicateWeekend_ReturnsTrueAsync(string dateString)
    {
        DateTime date = DateTime.Parse(dateString);

        var isDateTollExempt = await _tollServiceFaker.IsDateExemptFromTollAsync(date, _token).ConfigureAwait(false);

        Assert.IsTrue(isDateTollExempt);
    }

    [TestMethod] // It's hard to fully test this because I've been using an external API to find out if it's a red day.
    [DataRow("2024-06-21", false)]  // Midsummer Eve but not red day
    [DataRow("2024-01-01", true)]   // New year is red day
    [DataRow("2024-06-01", true)]   // Sweden's national day is red day
    [DataRow("2024-06-19", false)]  // Ordinary day
    [DataRow("2024-06-4", false)]  // Ordinary day
    public async Task IsDateExemptFromTollAsync_DateIndicateRedDayAndClientReturnsValidResponse_ReturnsExpectedResultAsync(string dateString, bool expectedResult)
    {
        DateTime date = DateTime.Parse(dateString);
        string query = date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

        string expectedUrl = $"{holidayApiOptions.BaseUrl}/{query}";
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(GetValidJsonSampleData(query, expectedResult), Encoding.UTF8, "application/json")
        };

        A.CallTo(() => _handlerFaker.FakeSendAsync(
                 A<HttpRequestMessage>.That.Matches(req => req.RequestUri!.ToString() == expectedUrl),
                 A<CancellationToken>._))
            .Returns(expectedResponse);

        _httpClientFaker = new HttpClient(_handlerFaker);

        _tollServiceFaker = A.Fake<TollService>(x => x.Strict(StrictFakeOptions.AllowObjectMethods)
        .WithArgumentsForConstructor([_httpClientFaker, _holidayApiOptionsFaker, _tollPriceOptionsFaker, _loggerFaker]));

        var actualResult = await _tollServiceFaker.IsDateExemptFromTollAsync(date, _token).ConfigureAwait(false);

        Assert.AreEqual(actualResult, expectedResult);
    }

    [TestMethod]
    public async Task IsDateExemptFromTollAsync_DateIndicateRedDayAndClientThrowsException_LogsExceptionCaught()
    {
        DateTime date = DateTime.Parse("2024-06-4");
        string query = date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

        string expectedUrl = $"{holidayApiOptions.BaseUrl}/{query}";

        A.CallTo(() => _handlerFaker.FakeSendAsync(
                 A<HttpRequestMessage>.That.Matches(req => req.RequestUri!.ToString() == expectedUrl),
                 A<CancellationToken>._))
            .ThrowsAsync(s_expectedException);

        _httpClientFaker = new HttpClient(_handlerFaker);

        using SerilogLoggerFactory factory = new(s_serilogLogger);
        ILogger<TollService> logger = factory.CreateLogger<TollService>();

        _tollServiceFaker = A.Fake<TollService>(x => x.Strict(StrictFakeOptions.AllowObjectMethods)
        .WithArgumentsForConstructor([_httpClientFaker, _holidayApiOptionsFaker, _tollPriceOptionsFaker, logger]));

        using ITestCorrelatorContext testCorrelatorContext = TestCorrelator.CreateContext();

        var exception = await Assert.ThrowsExceptionAsync<HttpRequestException>(async () =>
        await _tollServiceFaker.IsDateExemptFromTollAsync(date, _token)).ConfigureAwait(false);

        Assert.AreEqual(s_expectedException.Message, exception.Message);
        LogEvent[] logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToArray();

        var expectedErrorLogEvent = logEvents.Where(l =>
        l.Level == LogEventLevel.Error && l.Exception == s_expectedException).FirstOrDefault();

        Assert.IsNotNull(expectedErrorLogEvent);
    }

    [TestMethod]
    public async Task IsDateExemptFromTollAsync_DateIndicateRedDayAndClientReturnsnull_LogsInvalidOperationExceptionCaught()
    {
        DateTime date = DateTime.Parse("2024-06-4");
        string query = date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

        string expectedUrl = $"{holidayApiOptions.BaseUrl}/{query}";

        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
        };

        A.CallTo(() => _handlerFaker.FakeSendAsync(
                 A<HttpRequestMessage>.That.Matches(req => req.RequestUri!.ToString() == expectedUrl),
                 A<CancellationToken>._))
            .Returns(expectedResponse);

        using SerilogLoggerFactory factory = new(s_serilogLogger);
        ILogger<TollService> logger = factory.CreateLogger<TollService>();

        _httpClientFaker = new HttpClient(_handlerFaker);

        _tollServiceFaker = A.Fake<TollService>(x => x.Strict(StrictFakeOptions.AllowObjectMethods)
        .WithArgumentsForConstructor([_httpClientFaker, _holidayApiOptionsFaker, _tollPriceOptionsFaker, logger]));

        using ITestCorrelatorContext testCorrelatorContext = TestCorrelator.CreateContext();

        var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        await _tollServiceFaker.IsDateExemptFromTollAsync(date, _token)).ConfigureAwait(false);

        Assert.AreEqual(s_deserializationFailedException.Message, exception.Message);
        LogEvent[] logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToArray();

        var expectedErrorLogEvent = logEvents.Where(l =>
        l.Level == LogEventLevel.Error && l.MessageTemplate.ToString() == s_deserializationNullLogMessage).FirstOrDefault();

        Assert.IsNotNull(expectedErrorLogEvent);
    }

    [TestMethod]
    public async Task IsDateExemptFromTollAsync_DateIndicateRedDayAndClientReturnsInvalidResponse_LogsJsonReaderExceptionCaught()
    {
        DateTime date = DateTime.Parse("2024-06-4");
        string query = date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

        string expectedUrl = $"{holidayApiOptions.BaseUrl}/{query}";

        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Invalid")
        };

        A.CallTo(() => _handlerFaker.FakeSendAsync(
                 A<HttpRequestMessage>.That.Matches(req => req.RequestUri!.ToString() == expectedUrl),
                 A<CancellationToken>._))
            .Returns(expectedResponse);

        using SerilogLoggerFactory factory = new(s_serilogLogger);
        ILogger<TollService> logger = factory.CreateLogger<TollService>();

        _httpClientFaker = new HttpClient(_handlerFaker);

        _tollServiceFaker = A.Fake<TollService>(x => x.Strict(StrictFakeOptions.AllowObjectMethods)
        .WithArgumentsForConstructor([_httpClientFaker, _holidayApiOptionsFaker, _tollPriceOptionsFaker, logger]));

        using ITestCorrelatorContext testCorrelatorContext = TestCorrelator.CreateContext();

        await Assert.ThrowsExceptionAsync<JsonReaderException>(async () =>
        await _tollServiceFaker.IsDateExemptFromTollAsync(date, _token)).ConfigureAwait(false);

        LogEvent[] logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToArray();

        var expectedErrorLogEvent = logEvents.Where(l =>
        l.Level == LogEventLevel.Error && l.MessageTemplate.ToString() == s_deserializationFailedLogMessage).FirstOrDefault();

        Assert.IsNotNull(expectedErrorLogEvent);
    }


    [TestMethod]
    [DataRow(VehicleType.Motorbike, true)]  // true = exempt from toll
    [DataRow(VehicleType.Tractor, true)]
    [DataRow(VehicleType.Emergency, true)]
    [DataRow(VehicleType.Diplomat, true)]
    [DataRow(VehicleType.Foreign, true)]
    [DataRow(VehicleType.Military, true)]
    [DataRow(VehicleType.Car, false)]
    [DataRow(VehicleType.Bus, false)]
    [DataRow(VehicleType.Taxi, false)]
    [DataRow(VehicleType.Truck, false)]
    [DataRow(VehicleType.Van, false)]
    [DataRow(VehicleType.Scooter, false)]
    public void IsVehicleTypeExemptFromToll_ValidVehicleType_ReturnsExpectedResult(VehicleType type, bool expectedResult)
    {
        var actualResult = _tollServiceFaker.IsVehicleTypeExemptFromToll(type);

        Assert.AreEqual(expectedResult, actualResult);
    }

    [TestMethod]
    [DynamicData(nameof(GetTollTrackingModelsData), DynamicDataSourceType.Method)]
    public void CalculateVehicleTollFee_ValidVehicleType_ReturnsExpectedResult(string registrationNumber, IEnumerable<DateTime> timestamps, int expectedResult)
    {
        var actualResult = _tollServiceFaker.CalculateVehicleTollFee(registrationNumber, timestamps);

        Assert.AreEqual(expectedResult, actualResult);
    }

    private static List<object[]> GetTollTrackingModelsData()
    {
        return new List<object[]>
        {
            new object[] { "ZZZ121", new List<DateTime>
            {
                DateTime.Parse("2024-06-16T08:29:00"), 
            },
            13 // The expected toll fee.
            },
            new object[] { "ABC123", new List<DateTime>
            {
                DateTime.Parse("2024-06-17T08:30:00"), 
                DateTime.Parse("2024-06-17T08:23:00"),
                DateTime.Parse("2024-06-17T08:15:00"),
                DateTime.Parse("2024-06-17T08:35:00"), 
                DateTime.Parse("2024-06-17T15:20:00"), 
                DateTime.Parse("2024-06-17T15:45:00"),
                DateTime.Parse("2024-06-17T09:45:00"),
                DateTime.Parse("2024-06-17T11:00:00"),
                DateTime.Parse("2024-06-17T16:00:00")
            },
            47 // The expected toll fee.
            },
            new object[] { "XYZ999", new List<DateTime>
            {
                DateTime.Parse("2024-06-18T09:30:00"),
                DateTime.Parse("2024-06-18T09:45:00"),
                DateTime.Parse("2024-06-18T12:00:00"),
                DateTime.Parse("2024-06-18T14:30:00"),
                DateTime.Parse("2024-06-18T18:55:00")
            },
            24 // The expected toll fee.
            },
            new object[] { "VVV789", new List<DateTime>
            {
                DateTime.Parse("2024-06-19T10:00:00"),
                DateTime.Parse("2024-06-19T12:45:00"),
                DateTime.Parse("2024-06-19T15:30:00"),
                DateTime.Parse("2024-06-19T09:00:00"),
                DateTime.Parse("2024-06-19T11:30:00"),
                DateTime.Parse("2024-06-19T20:30:00")
            },
            42 // The expected toll fee.
            },
            new object[] { "VVV33", new List<DateTime>
            {
                DateTime.Parse("2024-06-20T07:37:00"),
                DateTime.Parse("2024-06-20T09:37:00"),
                DateTime.Parse("2024-06-20T13:37:00"),
                DateTime.Parse("2024-06-20T13:45:00"),
                DateTime.Parse("2024-06-20T14:25:00"), 
                DateTime.Parse("2024-06-20T15:00:00"),
                DateTime.Parse("2024-06-20T15:31:00"),
                DateTime.Parse("2024-06-20T16:30:00"),
                DateTime.Parse("2024-06-20T16:33:00"),
                DateTime.Parse("2024-06-20T17:00:00"),
                DateTime.Parse("2024-06-20T17:10:00")
            },
            60, // The maximum possible toll fee. 
            }
    };
    }

    private static string GetValidJsonSampleData(string date, bool isDateTollExempt)
    {
        var result = isDateTollExempt ? "Ja" : "nej";
        return @$"{{""cachetid"": ""2024-06-19 19:21:33"", ""version"": ""2.1"", ""uri"": ""/dagar/v2.1/{date}"", ""startdatum"": ""{date}"",
               ""slutdatum"": ""{date}"", ""dagar"": [{{""datum"": ""{date}"", ""veckodag"": ""Tisdag"", ""arbetsfri dag"": ""Ja"",
               ""röd dag"": ""{result}"", ""vecka"": ""02"", ""dag i vecka"": ""222"", ""helgdag"": ""Unknown"", ""namnsdag"": [ ""Melker"" ],
               ""flaggdag"": """" }}]}}";
    }

    private static TimeOnly ParseTime(string timeString)
    {
        return TimeOnly.Parse(timeString);
    }
}

// Needed somehow to get around the mock of HttpClient because its GetAsync  method is abstract, 
// so i mocked HttpMessageHandlerFaker which inherits HttpMessageHandler. It is the handler that actually handles the httpclient calls.
public abstract class HttpMessageHandlerFaker : HttpMessageHandler 
{
    public abstract Task<HttpResponseMessage> FakeSendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken);

    protected sealed override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        => this.FakeSendAsync(request, cancellationToken);
}