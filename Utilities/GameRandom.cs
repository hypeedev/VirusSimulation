public class GameRandom : IRandom
{
    private readonly Random _rng;

    public GameRandom()
    {
        _rng = new Random();
    }

    public GameRandom(int seed)
    {
        _rng = new Random(seed);
    }

    public int Next() => _rng.Next();
    public int Next(int maxValue) => _rng.Next(maxValue);
    public int Next(int minValue, int maxValue) => _rng.Next(minValue, maxValue);
    public float NextSingle() => _rng.NextSingle();

    public static IRandom Create() => new GameRandom();
    public static IRandom Create(int seed) => new GameRandom(seed);
}
