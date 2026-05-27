public class Statistics
{
    private List<string> lines = new();

    public Statistics()
    {
        lines.Add("Tick,Infected,Dead");
    }

    public void Save(int tick, int infected, int dead)
    {
        lines.Add($"{tick},{infected},{dead}");
    }

    public void Export()
    {
        File.WriteAllLines(
            "simulation.csv",
            lines);
    }
}
