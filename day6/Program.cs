// https://adventofcode.com/2022/day/6

int DetectDistinct(int bufferLength)
{
    var buffer = new int[bufferLength];
    int i = 0;

    using var reader = new StreamReader(args[0]);
    while((buffer[i % bufferLength] = reader.Read()) != -1)
    {
        if (i > 3 && buffer.Distinct().Count() == bufferLength)
        {
            break;
        }
        i++;
    }
    return i + 1;
}

Console.WriteLine($"Puzzle 1 solution is : {DetectDistinct(4)}");
Console.WriteLine($"Puzzle 2 solution is : {DetectDistinct(14)}");