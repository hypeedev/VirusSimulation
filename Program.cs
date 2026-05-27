using System.Linq;
Console.WriteLine("Choose virus:");
Console.WriteLine("1 - Flu");
Console.WriteLine("2 - Covid");
Console.WriteLine("3 - Rabies");
int virusChoice = int.Parse(Console.ReadLine()!);

Virus virus;

switch (virusChoice)
{
    case 1:
        virus = new Flu();
        break;

    case 2:
        virus = new Covid();
        break;

    case 3:
        virus = new Rabies();
        break;

    default:
        virus = new Flu();
        break;
}

Console.Write("Workers: ");
int workers = int.Parse(Console.ReadLine()!);

Console.Write("Students: ");
int students = int.Parse(Console.ReadLine()!);

Console.Write("Elders: ");
int elders = int.Parse(Console.ReadLine()!);

Console.Write("Doctors: ");
int doctors = int.Parse(Console.ReadLine()!);

Board board = new Board(40, 40);

// spawn workers
for (int i = 0; i < workers; i++)
{
    board.addRandomEntity((x, y) =>
        new Worker(x, y));
}

// spawn students
for (int i = 0; i < students; i++)
{
    board.addRandomEntity((x, y) =>
        new Student(x, y));
}

// spawn elders
for (int i = 0; i < elders; i++)
{
    board.addRandomEntity((x, y) =>
        new Elder(x, y));
}

// spawn doctors
for (int i = 0; i < doctors; i++)
{
    board.addRandomEntity((x, y) =>
        new Doctor(x, y));
}

// spawn hospital
board.addRandomEntity((x, y) =>
    new Hospital(x, y));

// infect first human
var firstHuman =
    board.GetHumansInRange(0, 0, 999)
        .First();

firstHuman.Infect(virus);

var renderer = new Renderer(board);

int tick = 0;

int lastInfected = 0;
int stagnationTicks = 0;

Statistics stats = new();

// simulation loop
while (true)
{
    Console.Clear();

    foreach (var entity in board.entities)
    {
        entity.Update(board);
    }

    virus.Spread(board);

    tick++;

    int infected =
        board.GetInfectedHumans().Count;

    int dead = board.entities.Count(e => e is Human h && !h.IsAlive);

    board.AwarenessLevel =
        MathF.Min(1f, infected / 300f);

    stats.Save(
        tick,
        infected,
        dead);

    int alive =
        board.entities.Count(
            e => e is Human h && h.IsAlive);

    if (infected > alive * 0.4f)
    {
        board.LockdownEnabled = true;
    }

    renderer.Render();

    Console.WriteLine();
    Console.WriteLine($"Tick: {tick}");
    Console.WriteLine($"Infected: {infected}");
    Console.WriteLine($"Dead: {dead}");
    Console.WriteLine($"Awareness: {board.AwarenessLevel:F2}");

    if (infected == 0)
    {
        Console.WriteLine("Virus eliminated!");

        stats.Export();

        break;
    }

    if (infected >= board.entities.Count(e => e is Human) - 1)
    {
        Console.WriteLine("World fully infected!");

        stats.Export();

        break;
    }

    if (infected == lastInfected)
    {
        stagnationTicks++;
    }
    else
    {
        stagnationTicks = 0;
    }

    lastInfected = infected;

    if (stagnationTicks > 100)
    {
        Console.WriteLine("Virus stagnated!");

        stats.Export();

        break;
    }

    Thread.Sleep(200);
}
