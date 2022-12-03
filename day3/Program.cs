char FindDuplicatedItem(string items)
{
    var first = items
        .AsSpan()
        .Slice(0, items.Length / 2);
    var second = items
        .AsSpan()
        .Slice(items.Length / 2);

    foreach (char itemFirst in first)
    {
        if (second.Contains(itemFirst))
        {
            return itemFirst;
        }
    }
    throw new Exception("Diplicated item not found.");
}

char FindCommon(string first, string second, string third)
{
    foreach (var item in first)
    {
        if (second.IndexOf(item) != -1 && third.IndexOf(item) != -1)
        {
            return item;
        }
    }
    throw new Exception("Common item not found.");
}

int GetPriority(char item)
{
    return (int)item switch
    {
        (>= 65) and (<= 90) => item - 38,
        (>= 97) and (<= 122) => item - 96,
        _ => throw new Exception($"Unsupported character: {item}")  
    };
}

var rucksacks = File.ReadAllLines(args[0]);
int prioritySum = 0;
foreach(var rucksack in rucksacks)
{
    prioritySum += GetPriority(FindDuplicatedItem(rucksack));
}

Console.WriteLine($"Puzzle 1 answer: {prioritySum}");

prioritySum = 0;
for (int groupIndex = 0; groupIndex < rucksacks.Length / 3; groupIndex++)
{
    prioritySum += GetPriority(FindCommon(
        rucksacks[groupIndex * 3],
        rucksacks[groupIndex * 3 + 1],
        rucksacks[groupIndex * 3 + 2]));
}

Console.WriteLine($"Puzzle 2 answer: {prioritySum}");