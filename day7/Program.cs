// https://adventofcode.com/2022/day/7

var rootNode = ParseInput(args[0]);
var dirs = Flatten(rootNode);

var solution1 = dirs
    .Where(d => d.Size <= 100000)
    .Select(d => d.Size)
    .Sum();

Console.WriteLine($"Solution to Puzzle 1 : {solution1}");

var solution2 = dirs
    .OrderBy(d => d.Size)
    .First(d => d.Size + 70000000 - rootNode.Size > 30000000)
    .Size;

Console.WriteLine($"Solution to Puzzle 2 : {solution2}");

Node ParseInput(string inputFileName)
{
    Node rootNode = new(NodeType.Dir, "/", null);
    Node currentNode = rootNode;

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
                _ => currentNode
                        .Items!
                        .First(n => n.Name == arg)
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
                        .Add(new(NodeType.Dir, lines[i].Substring(4), currentNode));
                }
                else
                {
                    var info = lines[i].Split(" ");
                    currentNode
                        .Items!
                        .Add(new(NodeType.File, info[1], currentNode, int.Parse(info[0])));
                }
            }
        }
    }
    return rootNode;
}

IEnumerable<Node> Flatten(Node node)
{
    List<Node> result = new();
    if (node.NodeType == NodeType.Dir)
    {
        result.Add(node);
        foreach(var item in node.Items!)
        {
            if (item.NodeType == NodeType.Dir)
            {
                result.AddRange(Flatten(item));
            }
        }
    }
    return result;
}

public enum NodeType
{
    File,
    Dir
}

public class Node
{
    public Node(NodeType nodeType, string name, Node? parent, int? size = default)
    {
        NodeType = nodeType;
        Name = name;
        Parent = parent;
        this.size = size;

        if (nodeType == NodeType.Dir)
        {
            Items = new();
        }

        if (nodeType == NodeType.File && size == default)
        {
            throw new ArgumentException(nameof(size));
        }
    }

    private int? size;

    public NodeType NodeType { get; }

    public string Name { get; }

    public Node? Parent { get; }

    public List<Node>? Items { get; }

    public int Size => NodeType switch
    {
        NodeType.File => size!.Value,
        NodeType.Dir => Items!.Sum(c => c.Size),
        _ => throw new NotImplementedException()
    };
}
