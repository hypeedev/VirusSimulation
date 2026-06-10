using Xunit;

public class HumanTests
{
    [Fact]
    public void Infect_SetsVirusAndMarksInfected()
    {
        var human = new Human(0, 0);
        var virus = new Flu();

        human.Infect(virus);

        Assert.True(human.IsInfected);
        Assert.Same(virus, human.Virus);
    }

    [Fact]
    public void Heal_ClearsInfection()
    {
        var human = new Human(0, 0);
        human.Infect(new Flu());

        human.Heal();

        Assert.False(human.IsInfected);
        Assert.Null(human.Virus);
        Assert.Equal(0, human.HealingTicks);
    }

    [Fact]
    public void Die_KillsHumanAndClearsVirus()
    {
        var human = new Human(0, 0);
        human.Infect(new Flu());

        human.Die();

        Assert.False(human.IsAlive);
        Assert.Null(human.Virus);
    }

    [Fact]
    public void DoubleInfection_FirstVirusIsKept()
    {
        var human = new Human(0, 0);
        var flu = new Flu();
        var covid = new Covid();

        human.Infect(flu);
        human.Infect(covid);

        Assert.Same(flu, human.Virus);
    }

    [Fact]
    public void Update_DeadHumanDoesNotMove()
    {
        var board = new Board(10, 10, true);
        var human = new Human(5, 5);
        board.addEntity(human);
        human.Die();

        human.Update(board);

        Assert.Equal(5, human.x);
        Assert.Equal(5, human.y);
    }

    [Fact]
    public void Student_ApproximatelyFiftyPercentImmunity()
    {
        var rng = GameRandom.Create(42);
        var virus = new Flu();
        int infections = 0;
        int trials = 2000;

        for (int i = 0; i < trials; i++)
        {
            var student = new Student(0, 0, rng);
            student.Infect(virus);
            if (student.IsInfected)
                infections++;
        }

        Assert.InRange(infections, 800, 1200);
    }

    [Fact]
    public void Worker_MovesTwice()
    {
        var rng = GameRandom.Create(42);
        var board = new Board(10, 10, true);
        board.AwarenessLevel = 0f;
        var worker = new Worker(5, 5, rng);
        worker.MigrationCooldown = 10;
        board.addEntity(worker);
        int oldX = worker.x, oldY = worker.y;

        worker.Update(board);

        int displacement = Math.Abs(worker.x - oldX) + Math.Abs(worker.y - oldY);
        Assert.InRange(displacement, 1, 4);
    }

    [Fact]
    public void Elder_Update_MovesHalfTheTime()
    {
        var rng = GameRandom.Create(42);
        var board = new Board(10, 10, true);
        board.AwarenessLevel = 0f;
        int moved = 0;
        int trials = 1000;

        for (int i = 0; i < trials; i++)
        {
            var elder = new Elder(5, 5, rng);
            board.addEntity(elder);
            int oldX = elder.x, oldY = elder.y;
            elder.Update(board);
            if (elder.x != oldX || elder.y != oldY)
                moved++;
            board.entities.Remove(elder);
        }

        Assert.InRange(moved, 350, 650);
    }
}
