// --- Day 11: Monkey in the Middle ---
// https://adventofcode.com/2022/day/11

var monkeys = GetInput();
Console.WriteLine($"Solution to puzzle 1 : {Solve(20, true)}");

monkeys = GetInput();
Console.WriteLine($"Solution to puzzle 2 : {Solve(10000, false)}");

long Solve(int rounds, bool calm)
{
    var inspectedItems = new long[monkeys.Length];

    int testDivisorProduct = 1;
    foreach (var m in monkeys)
    {
        testDivisorProduct *= m.TestDivisior;
    }

    foreach (int round in Enumerable.Range(1, rounds))
    {
        foreach (var m in monkeys)
        {
            foreach (var item in m.Items.ToArray())
            {
                long newLevel = m.Op.Execute(item);

                if (calm)
                    newLevel /= 3;

                if (newLevel > testDivisorProduct)
                    newLevel = newLevel % testDivisorProduct;

                monkeys[newLevel % m.TestDivisior == 0 ? m.ThrowToIfTrue : m.ThrowToIfFalse]
                    .Items
                    .Add(newLevel);
            }
            inspectedItems[m.Index] += m.Items.Count();
            m.Items.Clear();
        }
    }

    var topInspectors = inspectedItems.OrderDescending().ToArray();
    return topInspectors[0] * topInspectors[1];
}

Monkey[] GetInput()
{
    var monkeys = new List<Monkey>();

    using var reader = new StreamReader(args[0]);
    while(!reader.EndOfStream)
    {
        var index = reader.ReadLine()![7] - '0';
        var items = reader.ReadLine()!.Substring(18).Split(',').Select(num => long.Parse(num.Trim())).ToList();
        var op = GetOp(reader.ReadLine()!);
        var divisorTest = int.Parse(reader.ReadLine()!.Substring(21));
        var trueIndex = reader.ReadLine()![29] - '0';
        var falseIndex = reader.ReadLine()![30] - '0';

        monkeys.Add(new Monkey(index, items, op, trueIndex, falseIndex, divisorTest));

        if (!reader.EndOfStream)
            reader.ReadLine();
    }

    return monkeys.ToArray();
}

Op GetOp(string line)
{
    var isNum = int.TryParse(line.Substring(25), out int operand);
    return line[23] switch
    {
        '+' => new AddConst(operand),
        '*' => isNum ? new MultiplyConst(operand) : new Square(),
        _ => throw new Exception($"Cannot recognize operation.")
    };
}

record Monkey(int Index, List<long> Items, Op Op, int ThrowToIfTrue, int ThrowToIfFalse, int TestDivisior);
abstract record Op { public abstract long Execute(long x); }
record Square : Op { public override long Execute(long x) => x * x; }
record MultiplyConst(int c) : Op { public override long Execute(long x) => c * x; }
record AddConst(int c) : Op { public override long Execute(long x) => c + x; }