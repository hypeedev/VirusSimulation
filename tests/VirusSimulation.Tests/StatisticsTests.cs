using Xunit;

public class StatisticsTests : IDisposable
{
    private readonly string _filePath;

    public StatisticsTests()
    {
        _filePath = Path.Combine(Path.GetTempPath(), $"simulation_{Guid.NewGuid()}.csv");
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }

    [Fact]
    public void CSV_HasCorrectHeader()
    {
        var stats = new Statistics(_filePath);

        stats.Save(1, 0, 0);
        stats.Export();

        var lines = File.ReadAllLines(_filePath);
        Assert.Equal("Tick,Infected,Dead", lines[0]);
    }

    [Fact]
    public void CSV_SavesMultipleRows()
    {
        var stats = new Statistics(_filePath);

        stats.Save(1, 5, 0);
        stats.Save(2, 10, 1);
        stats.Save(3, 15, 3);
        stats.Export();

        var lines = File.ReadAllLines(_filePath);
        Assert.Equal(4, lines.Length);
        Assert.Equal("1,5,0", lines[1]);
        Assert.Equal("2,10,1", lines[2]);
        Assert.Equal("3,15,3", lines[3]);
    }

    [Fact]
    public void CSV_ZeroValues()
    {
        var stats = new Statistics(_filePath);

        stats.Save(0, 0, 0);
        stats.Export();

        var lines = File.ReadAllLines(_filePath);
        Assert.Equal("0,0,0", lines[1]);
    }
}
