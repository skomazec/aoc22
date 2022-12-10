// --- Day 10: Cathode-Ray Tube ---
// https://adventofcode.com/2022/day/10

var execution = ExecuteOps(1, GetOps());
var result = execution
    .Where(e => e.Cycle % 20 == 0 && e.Cycle / 20 % 2 != 0)
    .Take(6)
    .Aggregate(0, (res, next) => res + next.Cycle * next.Reg);

Console.WriteLine($"Solution to puzzle 1 : {result}");

var buffer = Enumerable
    .Range(1, 240)
    .Select(i => ' ')
    .ToArray();

foreach(var (cycle, reg) in execution)
{
    var pixel = cycle % 40 - 1;
    if (reg - 1 <= pixel && reg + 1 >= pixel)
    {
        buffer[cycle - 1] = '#';
    }
}

Console.WriteLine($"Solution to puzzle 2 :");
foreach (int line in Enumerable.Range(1, 6))
{
    Console.WriteLine(string.Join("", buffer.Skip(40 * (line - 1)).Take(40)));
}

IEnumerable<(int Cycle, int Reg)> ExecuteOps(int initReg, IEnumerable<Op> ops)
{
    int reg = initReg;
    int cycle = 0;

    foreach (var op in ops)
    {
        switch (op)
        {
            case Noop _:
                
                cycle++;
                yield return (cycle, reg);
                break;

            case AddX addX:
                
                cycle++;
                yield return (cycle, reg);
                cycle++;
                yield return (cycle, reg);
                reg += addX.Value;
                break;
        }
    }
}

IEnumerable<Op> GetOps()
{
    foreach (var line in File.ReadLines(args[0]))
    {
        yield return line.Substring(0, 4) switch
        {
            "noop" => new Noop(),
            "addx" => new AddX(int.Parse(line.Substring(5))),
            _ => throw new Exception($"Unknown CPU instruction {line}.")
        };
    }
}

abstract record Op();
record AddX(int Value) : Op;
record Noop : Op;