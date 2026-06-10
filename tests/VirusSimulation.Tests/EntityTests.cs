using Xunit;

public class EntityTests
{
    [Fact]
    public void Hospital_HealsNearbyInfectedGradually()
    {
        var board = new Board(10, 10, true);
        var human = new Human(5, 5);
        human.Infect(new Flu());
        var hospital = new Hospital(5, 5);
        board.addEntity(human);
        board.addEntity(hospital);

        hospital.Update(board);

        Assert.Equal(1, human.HealingTicks);
        Assert.True(human.IsInfected);
    }

    [Fact]
    public void Hospital_FullyHealsAfterFiveTicks()
    {
        var board = new Board(10, 10, true);
        var human = new Human(5, 5);
        human.Infect(new Flu());
        var hospital = new Hospital(5, 5);
        board.addEntity(human);
        board.addEntity(hospital);

        for (int i = 0; i < 5; i++)
            hospital.Update(board);

        Assert.False(human.IsInfected);
        Assert.Equal(0, human.HealingTicks);
    }

    [Fact]
    public void Hospital_DoesNotHealOutOfRange()
    {
        var board = new Board(10, 10, true);
        var human = new Human(0, 0);
        human.Infect(new Flu());
        var hospital = new Hospital(9, 9);
        board.addEntity(human);
        board.addEntity(hospital);

        hospital.Update(board);

        Assert.Equal(0, human.HealingTicks);
    }

    [Fact]
    public void Doctor_HealsNearbyInfected()
    {
        var board = new Board(10, 10, true);
        var human = new Human(5, 5);
        human.Infect(new Flu());
        var doctor = new Doctor(5, 5);
        board.addEntity(human);
        board.addEntity(doctor);

        doctor.Update(board);

        Assert.Equal(1, human.HealingTicks);
    }

    [Fact]
    public void Doctor_HasExpectedProperties()
    {
        var doctor = new Doctor(0, 0);

        Assert.Equal(3, doctor.Range);
        Assert.Equal(0.8f, doctor.Effectiveness);
        Assert.Equal('D', doctor.Symbol);
    }

    [Fact]
    public void Hospital_HasExpectedProperties()
    {
        var hospital = new Hospital(0, 0);

        Assert.Equal(2, hospital.Range);
        Assert.Equal(1.0f, hospital.Effectiveness);
        Assert.Equal('+', hospital.Symbol);
    }

    [Fact]
    public void Entity_HasCorrectCoordinates()
    {
        var entity = new Human(7, 3);

        Assert.Equal(7, entity.x);
        Assert.Equal(3, entity.y);
    }

    [Fact]
    public void TileSymbol_ReturnsCorrectChar()
    {
        Assert.Equal('~', Tile.Water.Symbol());
        Assert.Equal('L', Tile.Land.Symbol());
        Assert.Equal('.', Tile.Region0.Symbol());
    }

    [Fact]
    public void SpreadLogicFactory_ReturnsCorrectTypes()
    {
        Assert.IsType<UniformSpreadLogic>(
            SpreadLogicFactory.Create(SpreadLogicType.Uniform));
        Assert.IsType<DistanceWeightedSpreadLogic>(
            SpreadLogicFactory.Create(SpreadLogicType.DistanceWeighted));
        Assert.IsType<FocusedSpreadLogic>(
            SpreadLogicFactory.Create(SpreadLogicType.Focused));
    }

    [Fact]
    public void SpreadLogicFactory_ThrowsForInvalidType()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            SpreadLogicFactory.Create((SpreadLogicType)999));
    }
}
