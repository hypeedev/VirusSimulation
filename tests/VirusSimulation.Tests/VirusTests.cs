using Xunit;

public class VirusTests
{
    [Fact]
    public void Flu_HasExpectedProperties()
    {
        var flu = new Flu();

        Assert.Equal("Flu", flu.Name);
        Assert.Equal(0.25f, flu.Infectivity);
        Assert.Equal(0.005f, flu.Mortality);
        Assert.Equal(1, flu.InfectionRange);
    }

    [Fact]
    public void Covid_HasExpectedProperties()
    {
        var covid = new Covid();

        Assert.Equal("Covid", covid.Name);
        Assert.Equal(0.65f, covid.Infectivity);
        Assert.Equal(0.02f, covid.Mortality);
        Assert.Equal(2, covid.InfectionRange);
    }

    [Fact]
    public void Rabies_HasExpectedProperties()
    {
        var rabies = new Rabies();

        Assert.Equal("Rabies", rabies.Name);
        Assert.Equal(0.5f, rabies.Infectivity);
        Assert.Equal(0.3f, rabies.Mortality);
        Assert.Equal(1, rabies.InfectionRange);
    }

    [Fact]
    public void Spread_DoesNotThrowOnEmptyBoard()
    {
        var board = new Board(10, 10, true);
        var flu = new Flu();

        var ex = Record.Exception(() => flu.Spread(board));

        Assert.Null(ex);
    }

    [Fact]
    public void UniformSpread_CanInfectNearbyHumans()
    {
        var rng = GameRandom.Create(42);
        var board = new Board(10, 10, true);
        var target = new Human(6, 5);
        board.addEntity(target);

        var flu = new Flu(rng);
        for (int i = 0; i < 50; i++)
        {
            board.entities.RemoveAll(e => e is Human h && !h.IsAlive);
            var source = new Human(5, 5);
            source.Infect(new Flu());
            board.addEntity(source);

            flu.Spread(board);
            if (target.IsInfected) break;
        }

        Assert.True(target.IsInfected);
    }

    [Fact]
    public void DistanceWeightedSpread_CanInfectNearbyHumans()
    {
        var rng = GameRandom.Create(42);
        var board = new Board(10, 10, true);
        var target = new Human(6, 5);
        board.addEntity(target);

        var covid = new Covid(rng);
        for (int i = 0; i < 20; i++)
        {
            board.entities.RemoveAll(e => e is Human h && !h.IsAlive);
            var source = new Human(5, 5);
            source.Infect(new Covid());
            board.addEntity(source);

            covid.Spread(board);
            if (target.IsInfected) break;
        }

        Assert.True(target.IsInfected);
    }

    [Fact]
    public void FocusedSpread_CanInfect()
    {
        var rng = GameRandom.Create(42);
        var board = new Board(10, 10, true);
        var target = new Human(5, 6);
        board.addEntity(target);

        var rabies = new Rabies(rng);
        bool infected = false;

        for (int tick = 0; tick < 300; tick++)
        {
            board.entities.RemoveAll(e => e is Human h && !h.IsAlive);
            var source = new Human(5, 5);
            source.Infect(rabies);
            board.addEntity(source);
            rabies.Spread(board);
            if (target.IsInfected)
            {
                infected = true;
                break;
            }
        }

        Assert.True(infected);
    }
}
