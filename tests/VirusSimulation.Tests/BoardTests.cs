using Xunit;

public class BoardTests
{
    [Fact]
    public void IsWalkable_OutOfBounds_ReturnsFalse()
    {
        var board = new Board(10, 10, true);

        Assert.False(board.IsWalkable(-1, 0));
        Assert.False(board.IsWalkable(0, -1));
        Assert.False(board.IsWalkable(10, 0));
        Assert.False(board.IsWalkable(0, 10));
    }

    [Fact]
    public void IsWalkable_OnLand_ReturnsTrue()
    {
        var board = new Board(10, 10, true);

        Assert.True(board.IsWalkable(5, 5));
        Assert.True(board.IsWalkable(0, 0));
        Assert.True(board.IsWalkable(9, 9));
    }

    [Fact]
    public void GetEntityAt_ReturnsCorrectEntity()
    {
        var board = new Board(10, 10, true);
        var human = new Human(3, 4);
        board.addEntity(human);

        var result = board.GetEntityAt(3, 4);

        Assert.Same(human, result);
    }

    [Fact]
    public void GetEntityAt_DeadHuman_ReturnsNull()
    {
        var board = new Board(10, 10, true);
        var human = new Human(3, 4);
        board.addEntity(human);
        human.Die();

        var result = board.GetEntityAt(3, 4);

        Assert.Null(result);
    }

    [Fact]
    public void GetEntityAt_NoEntity_ReturnsNull()
    {
        var board = new Board(10, 10, true);

        var result = board.GetEntityAt(7, 7);

        Assert.Null(result);
    }

    [Fact]
    public void GetHumansInRange_ReturnsOnlyHumansWithinRange()
    {
        var board = new Board(10, 10, true);
        var center = new Human(5, 5);
        var close = new Human(6, 6);
        var far = new Human(0, 0);
        board.addEntity(center);
        board.addEntity(close);
        board.addEntity(far);

        var result = board.GetHumansInRange(5, 5, 2);

        Assert.Contains(center, result);
        Assert.Contains(close, result);
        Assert.DoesNotContain(far, result);
    }

    [Fact]
    public void GetHumansInRange_DeadHumansAreIncluded()
    {
        var board = new Board(10, 10, true);
        var alive = new Human(5, 5);
        var dead = new Human(6, 5);
        dead.Die();
        board.addEntity(alive);
        board.addEntity(dead);

        var result = board.GetHumansInRange(5, 5, 2);

        Assert.Contains(alive, result);
        Assert.Contains(dead, result);
    }

    [Fact]
    public void GetInfectedHumans_OnlyReturnsAliveInfected()
    {
        var board = new Board(10, 10, true);
        var infected = new Human(5, 5);
        infected.Infect(new Flu());
        var deadInfected = new Human(6, 5);
        deadInfected.Infect(new Flu());
        deadInfected.Die();
        var healthy = new Human(4, 5);
        board.addEntity(infected);
        board.addEntity(deadInfected);
        board.addEntity(healthy);

        var result = board.GetInfectedHumans();

        Assert.Contains(infected, result);
        Assert.DoesNotContain(deadInfected, result);
        Assert.DoesNotContain(healthy, result);
    }

    [Fact]
    public void AddRandomEntity_DoesNotThrow()
    {
        var board = new Board(10, 10, true);

        var ex = Record.Exception(() =>
            board.addRandomEntity((x, y) => new Human(x, y)));

        Assert.Null(ex);
        Assert.Single(board.entities);
    }

    [Fact]
    public void WidthAndHeight_MatchConstructor()
    {
        var board = new Board(15, 25, true);

        Assert.Equal(15, board.Width);
        Assert.Equal(25, board.Height);
    }

    [Fact]
    public void GetTileAt_OutOfBounds_ReturnsWater()
    {
        var board = new Board(10, 10, true);

        Assert.Equal(Tile.Water, board.GetTileAt(-1, 0));
        Assert.Equal(Tile.Water, board.GetTileAt(0, 15));
    }

    [Fact]
    public void LockdownEnabled_PreventsRegionCrossing()
    {
        var rng = GameRandom.Create(42);
        var board = new Board(15, 15, false, rng);
        board.LockdownEnabled = true;
        board.AwarenessLevel = 0f;

        var human = new Human(7, 7, rng);
        board.addEntity(human);

        var startRegion = board.GetRegionAt(7, 7);
        human.Update(board);

        Assert.Equal(startRegion, board.GetRegionAt(human.x, human.y));
    }
}
