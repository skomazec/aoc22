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

Node ParseInput(string inputFileName)
{
    Node rootNode = new(NodeType: NodeType.Dir, Name: "/", Parent: null, Size: null, Items: new());
    Node currentNode = rootNode;

    var lines = File.ReadLines(inputFileName).ToArray();

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
                _ => currentNode.Items!.First(n => n.Name == arg)
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
                        .Items!
                        .Add(new(NodeType: NodeType.Dir, Name: lines[i].Substring(4), Parent: currentNode, Size: null, Items: new()));
                }
                else
                {
                    var info = lines[i].Split(" ");
                    currentNode
                        .Items!
                        .Add(new(NodeType: NodeType.File, Name: info[1], Parent: currentNode, Size: int.Parse(info[0]), Items: null));
                }
            }
        }
    }
    return rootNode;
}

IEnumerable<Node> Flatten(Node node)
{
    List<Node> result = new();
    if (node.NodeType == NodeType.File)
        return result;

    result.Add(node);
    foreach(var item in node.Items!)
    {
        if (item.NodeType == NodeType.Dir)
            result.AddRange(Flatten(item));
    }
    return result;
}

public enum NodeType
{
    File,
    Dir
}

public record Node(NodeType NodeType, string Name, Node? Parent, int? Size, List<Node>? Items)
{
    public int TotalSize 
        => NodeType switch
        {
            NodeType.File => Size!.Value,
            NodeType.Dir => Items!.Sum(c => c.TotalSize),
            _ => throw new NotImplementedException()
        };
}
