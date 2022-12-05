using System.Linq;

Stack<char>[] GetInputStacks()
{
    using var reader = new StreamReader(args[0]);

    List<string> stackLines = new();
    string? line;
    while((line = reader.ReadLine()) != string.Empty)
    {
        stackLines.Add(line!);
    }

    var stackCount = stackLines
        .Last()
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(stackNo => int.Parse(stackNo))
        .Max();
    
    Stack<char>[] stacks = Enumerable
        .Range(1, stackCount)
        .Select(i => new Stack<char>())
        .ToArray();

    foreach (var stackLine in stackLines.SkipLast(1).Reverse())
    {
        for (int i = 0; i < stackCount; i++)
        {
            char crate = stackLine[i*4+1];
            if (char.IsLetter(crate))
            {
                stacks[i].Push(crate);
            }
        }
    }

    return stacks;
}

IReadOnlyList<(int Count, int From, int To)> GetInputMoves()
{
    using var reader = new StreamReader(args[0]);

    string? line;
    while((line = reader.ReadLine()) != string.Empty)
    { }

    List<(int, int, int)> moves = new();
    while((line = reader.ReadLine()) != null)
    {
        var tokens = line.Split(' ');
        moves.Add((int.Parse(tokens[1]), int.Parse(tokens[3]), int.Parse(tokens[5])));
    }
    return moves;
}

var moves = GetInputMoves();
var stacks = GetInputStacks();
foreach (var (Count, From, To) in moves)
{
    for (int i = 0 ; i < Count; i++)
    {
        stacks[To - 1].Push(stacks[From - 1].Pop());
    }
}
Console.WriteLine($"Puzzle 1 solution is : {string.Concat(stacks.Select(s => s.Peek()))}");

stacks = GetInputStacks();
var tempStack = new Stack<char>();
foreach (var (Count, From, To) in moves)
{
    for (int i = 0 ; i < Count; i++)
    {
        tempStack.Push(stacks[From - 1].Pop());
    }
    for (int i = 0 ; i < Count; i++)
    {
        stacks[To -1].Push(tempStack.Pop());
    }
    tempStack.Clear();
}
Console.WriteLine($"Puzzle 2 solution is : {string.Concat(stacks.Select(s => s.Peek()))}");