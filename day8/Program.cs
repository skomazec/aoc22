// https://adventofcode.com/2022/day/8

var grid = ParseGrid();

Console.WriteLine($"Solution to puzzle 1 : {CollectVisibleTrees(grid) + 2 * grid.GetLength(0) + 2 * grid.GetLength(1) - 4}");
Console.WriteLine($"Solution to puzzle 2 : {GetScenicScores(grid).Max()}");

IEnumerable<int> GetScenicScores(byte[,] grid)
{
    int xLen = grid.GetLength(0);
    int yLen = grid.GetLength(1);

    for(int x = 1; x < xLen - 1; x++)
    {
        for(int y = 1; y < yLen - 1; y++)
        {
            int toLeft = x - 1;
            while(toLeft > 0 && grid[x,y] > grid[toLeft, y])
                toLeft--;

            int toRight = x + 1;
            while(toRight < xLen - 1 && grid[x,y] > grid[toRight,y])
                toRight++;

            int toUp = y - 1;
            while(toUp > 0 && grid[x,y] > grid[x, toUp])
                toUp--;

            int toDown = y + 1;
            while(toDown < yLen - 1 && grid[x,y] > grid[x,toDown])
                toDown++;

            yield return (y - toUp) * (x - toLeft) * (toRight - x) * (toDown - y);
        }
    }
}

int CollectVisibleTrees(byte[,] grid)
{
    int xLen = grid.GetLength(0);
    int yLen = grid.GetLength(1);

    int visibleTree = 0;
    for(int x = 1; x < xLen - 1; x++)
        for(int y = 1; y < yLen - 1; y++)
            if (Enumerable.Range(0, x).All(i => grid[i, y] < grid[x, y]) || 
                Enumerable.Range(0, y).All(i => grid[x, i] < grid[x, y]) ||
                Enumerable.Range(x + 1, xLen - x - 1).All(i => grid[i, y] < grid[x, y]) ||
                Enumerable.Range(y + 1, yLen - y - 1).All(i => grid[x, i] < grid[x, y]))
                visibleTree++;
                
    return visibleTree;
}

byte[,] ParseGrid()
{
    var lines = File.ReadLines(args[0]).ToArray();
    var grid = new byte[lines[0].Length, lines.Length];
    for (int y = 0; y < grid.GetLength(1); y++)
        for (int x = 0; x < grid.GetLength(0); x++)
            grid[x,y] = byte.Parse(lines[y][x].ToString());

    return grid;
}