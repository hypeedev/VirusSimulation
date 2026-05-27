public class Student : Human
{
    public override char Symbol => 'S';

    public override string Description =>
        $"Student{(IsInfected ? " (Infected)" : "")}";

    public Student(int x, int y)
        : base(x, y, RandomAge(16, 25))
    {

    }

    public override void Infect(IVirus virus)
    {
        if (Random.Shared.NextSingle() < 0.5f)
            return;

        base.Infect(virus);
    }
}
