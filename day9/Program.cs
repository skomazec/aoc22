// https://adventofcode.com/2022/day/9

var tailSteps = GetTailSteps(0, 0, GetHeadSteps(0, 0));
Console.WriteLine($"Solution to puzzle 1 : {tailSteps.ToHashSet().Count()}");

tailSteps = GetTail9Steps(0, 0, GetHeadSteps(0, 0));
Console.WriteLine($"Solution to puzzle 2 : {tailSteps.ToHashSet().Count()}");

IEnumerable<(int, int)> GetTailSteps(int initX, int initY, IEnumerable<(int, int)> headSteps)
{
    int tailX = initX;
    int tailY = initY;

    foreach (var (headX, headY) in headSteps)
    {
        (tailX, tailY) = GetNextPosition(headX, headY, tailX, tailY);
        yield return (tailX, tailY);
    }
}

IEnumerable<(int, int)> GetTail9Steps(int initX, int initY, IEnumerable<(int, int)> headSteps)
{
    (int X, int Y)[] tails = Enumerable
        .Range(1, 10)
        .Select (_ => (initX, initY))
        .ToArray();

    foreach (var (headX, headY) in headSteps)
    {
        (tails[0].X, tails[0].Y) = (headX, headY);
        for (int i = 1; i < 10; i++)
        {
            (tails[i].X, tails[i].Y) = GetNextPosition(tails[i-1].X, tails[i-1].Y, tails[i].X, tails[i].Y);
        }

        yield return (tails[9].X, tails[9].Y);
    }
}

(int, int) GetNextPosition (int headX, int headY, int tailX, int tailY)
{
    int deltaX = headX - tailX;
    int deltaY = headY - tailY;

    if (Math.Abs(deltaX) == 2 || Math.Abs(deltaY) == 2)
    {
        tailX = deltaX switch
        {
            < 0 => tailX - 1,
            > 0 => tailX + 1,
            0 => tailX
        };

        tailY = deltaY switch
        {
            < 0 => tailY - 1,
            > 0 => tailY + 1,
            0 => tailY
        };
    }
    return (tailX, tailY);
}

IEnumerable<(int, int)> GetHeadSteps(int initX, int initY)
{
    int x = initX;
    int y = initY;

    foreach (var line in File.ReadLines(args[0]))
    {
        for (int step = 0; step < int.Parse(line.Substring(2)); step++)
        {
            switch (line[0])
            {
                case 'L': 
                    yield return (--x, y);
                    break;
                case 'R':
                    yield return (++x, y);
                    break;
                case 'U':
                    yield return (x, ++y);
                    break;
                case 'D':
                    yield return (x, --y);
                    break;
            }
        }
    }
}