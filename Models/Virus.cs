public abstract class Virus
{
    public string Name { get; set; }

    public float Infectivity { get; set; }

    public float Mortality { get; set; }

    public int InfectionRange { get; set; }

    Random rng = new();

    public Virus(
        string name,
        float infectivity,
        float mortality,
        int infectionRange)
    {
        Name = name;
        Infectivity = infectivity;
        Mortality = mortality;
        InfectionRange = infectionRange;
    }

    public void Spread(Board board)
    {
        var infected = board.GetInfectedHumans();

        foreach (var source in infected)
        {
            if (!source.IsAlive)
                continue;
            var nearby =
                board.GetHumansInRange(
                    source.x,
                    source.y,
                    InfectionRange);

            foreach (var target in nearby)
            {
                if (target == source)
                    continue;

                if (target.IsInfected)
                    continue;

                if (rng.NextSingle() <= Infectivity)
                {
                    target.Infect(this);
                }
            }
            
            if (!source.IsAlive)
                continue;
            
            if (rng.NextSingle() <= Mortality)
            {
                source.Die();
            }
        }
    }
}
