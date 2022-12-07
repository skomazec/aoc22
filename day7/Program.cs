// https://adventofcode.com/2022/day/7

var rootNode = ParseInput(args[0]);
var dirs = Flatten(rootNode);

var solution1 = dirs
    .Where(d => d.TotalSize <= 100000)
    .Select(d => d.TotalSize)
    .Sum();

Console.WriteLine($"Solution to Puzzle 1 : {solution1}");

var solution2 = dirs
    .OrderBy(d => d.TotalSize)
    .First(d => d.TotalSize + 70000000 - rootNode.TotalSize > 30000000)
    .TotalSize;

Console.WriteLine($"Solution to Puzzle 2 : {solution2}");

DirNode ParseInput(string inputFileName)
{
    DirNode rootNode = new(Name: "/", Parent: null, Items: new());
    DirNode currentNode = rootNode;

    var lines = File
        .ReadLines(inputFileName)
        .ToArray();

    int i = 0;
    while(i != lines.Length - 1)
    {
        if (lines[i].StartsWith("$ cd"))
        {
            var arg = lines[i].Substring(5);
            currentNode = arg switch
            {
                "/" => rootNode,
                ".." => currentNode.Parent ?? throw new Exception("No parent node."),
                _ => (DirNode)currentNode.Items.First(n => n.Name == arg)
            };
            i++;
        }
        else if (lines[i].StartsWith("$ ls"))
        {
            while(i < lines.Length - 1 && !lines[++i].StartsWith("$"))
            {
                //Assumes no previous 'ls' on the same folder
                if (lines[i].StartsWith("dir"))
                {
                    currentNode
                        .Items
                        .Add(new DirNode(Name: lines[i].Substring(4), Parent: currentNode, Items: new()));
                }
                else
                {
                    var info = lines[i].Split(" ");
                    currentNode
                        .Items
                        .Add(new FileNode(Name: info[1], Parent: currentNode, Size: int.Parse(info[0])));
                }
            }
        }
    }
    return rootNode;
}

IList<DirNode> Flatten(DirNode node)
{
    List<DirNode> result = new() {node};
    foreach(var item in node.Items!)
    {
        if (item is DirNode itemDirNode)
            result.AddRange(Flatten(itemDirNode));
    }
    return result;
}

public abstract record Node(string Name, DirNode? Parent)
{
    public abstract int TotalSize { get; }
}

public record DirNode(string Name, DirNode? Parent, List<Node> Items)
    : Node(Name, Parent)
{
    public override int TotalSize => Items.Sum(c => c.TotalSize);
}

public record FileNode(string Name, DirNode? Parent, int Size)
    : Node(Name, Parent)
{
    public override int TotalSize => Size;
}