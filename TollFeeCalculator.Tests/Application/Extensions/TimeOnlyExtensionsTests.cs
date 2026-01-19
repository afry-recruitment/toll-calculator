using TollFeeCalculator.Application.Extensions;
using TollFeeCalculator.Tests.TestData;

namespace TollFeeCalculator.Tests.Application.Extensions;

[TestClass]
public sealed class TimeOnlyExtensionsTests
{
    [TestMethod]
    public void ChunkByChargeWindow_Null_ReturnsEmpty()
    {
        IEnumerable<TimeOnly>? times = null;
        var result = times!.ChunkByChargeWindow();
        Assert.AreEqual(0, result.Count);
    }

    [DataTestMethod]
    [DynamicData(nameof(ApplicationTestData.TimeOnlyExtensions_ChunkByChargeWindow_Cases), typeof(ApplicationTestData), DynamicDataSourceType.Method)]
    public void ChunkByChargeWindow_Cases_ReturnExpectedChunkCount(string[] times, int windowMinutes, int expectedChunks)
    {
        var parsed = times.Select(TimeOnly.Parse).ToArray();

        var result = parsed.ChunkByChargeWindow(windowMinutes);

        Assert.AreEqual(expectedChunks, result.Count);
    }

    [TestMethod]
    public void ChunkByChargeWindow_WhenDiffEqualsWindow_StartsNewChunk()
    {
        var times = new[]
        {
            TimeOnly.Parse("06:00"),
            TimeOnly.Parse("07:00"),
        };

        var chunks = times.ChunkByChargeWindow(60);

        Assert.AreEqual(2, chunks.Count);
        Assert.AreEqual(1, chunks[0].Count);
        Assert.AreEqual(1, chunks[1].Count);
    }

    [TestMethod]
    public void ChunkByChargeWindow_NonPositiveWindow_UsesDefault60()
    {
        var times = new[]
        {
            TimeOnly.Parse("06:00"),
            TimeOnly.Parse("06:59"),
            TimeOnly.Parse("07:00"),
        };

        var chunks = times.ChunkByChargeWindow(0);

        Assert.AreEqual(2, chunks.Count);
    }
}