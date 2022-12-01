int calSum = 0;
var cals = new List<int>();

foreach (string line in File.ReadLines(args[0]))
{
    if (line == string.Empty)
    {
        cals.Add(calSum);
        calSum = 0;
    }
    else
    {
        calSum += int.Parse(line);
    }
}

Console.WriteLine($"Puzzle 1 answer: {cals.Max()}.");
Console.WriteLine($"Puzzle 2 answer: {cals.OrderDescending().Take(3).Sum()}.");