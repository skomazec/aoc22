// https://adventofcode.com/2022/day/4

IEnumerable<(int, int, int, int)> GetSectionPairs()
{
    foreach(string line in File.ReadLines(args[0]))
    {
        var edges = line
            .Split(',')
            .SelectMany(s => s.Split('-'))
            .Select(e => int.Parse(e))
            .ToArray();
        yield return (edges[0], edges[1], edges[2], edges[3]);
    }
}

int pairCount = 0;
foreach (var (a, b, c, d) in GetSectionPairs())
{
    if ((c <= a && b <= d) ||
        (a <= c && d <= b))
    {
        pairCount++;
    }
}

Console.WriteLine($"Puzzle 1 solution: {pairCount}");

pairCount = 0;
foreach (var (a, b, c, d) in GetSectionPairs())
{
    if ((c <= a && b <= d) ||
        (a <= c && d <= b) ||
        (a >= c && a <= d) ||
        (b >= c && b <= d) ||
        (c >= a && c <= b) ||
        (d >= a && d <= b))
    {
        pairCount++;
    }
}

Console.WriteLine($"Puzzle 2 solution: {pairCount}");