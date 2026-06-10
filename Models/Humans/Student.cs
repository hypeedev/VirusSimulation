public class Student : Human
{
    public override char Symbol => 'S';

    public override string Description =>
        $"Student{(IsInfected ? " (Infected)" : "")}";

    public Student(int x, int y) : this(x, y, GameRandom.Create())
    {
    }

    public Student(int x, int y, IRandom rng)
        : base(x, y, rng.Next(16, 26), rng)
    {

    }

    public override void Infect(IVirus virus)
    {
        if (rng.NextSingle() < 0.5f)
            return;

        base.Infect(virus);
    }
}
