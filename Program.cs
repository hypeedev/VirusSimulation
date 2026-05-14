using System.Linq;

Board board = new Board(40, 40);

Virus virus = new Virus(
    "Covid",
    0.2f,
    0.01f,
    2);

// spawn humans
for (int i = 0; i < 50; i++)
{
    board.addRandomEntity((x, y) =>
        new Human(x, y));
}

// spawn doctors
for (int i = 0; i < 3; i++)
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

// simulation loop
while (true)
{
    Console.Clear();

    foreach (var entity in board.entities)
    {
        if (entity is Human human)
        {
            human.Update(board);
        }

        if (entity is Hospital hospital)
        {
            hospital.Update(board);
        }
    }

    virus.Spread(board);

    renderer.Render();

    Thread.Sleep(200);
}
