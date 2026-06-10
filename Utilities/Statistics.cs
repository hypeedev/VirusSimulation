public class Statistics
{
    private List<string> lines = new();
    private readonly string outputPath;

    public Statistics() : this("simulation.csv")
    {
    }

    public Statistics(string outputPath)
    {
        this.outputPath = outputPath;
        lines.Add("Tick,Infected,Dead");
    }

    public void Save(int tick, int infected, int dead)
    {
        lines.Add($"{tick},{infected},{dead}");
    }

    public void Export()
    {
        File.WriteAllLines(
            outputPath,
            lines);
    }
}
