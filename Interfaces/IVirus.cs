public interface IVirus
{
    string Name { get; }

    float Infectivity { get; }

    float Mortality { get; }

    int InfectionRange { get; }

    void Spread(Board board);
}
