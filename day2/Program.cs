// https://adventofcode.com/2022/day/2

const int Shape_Rock = 1;
const int Shape_Paper = 2;
const int Shape_Scissors = 3;

const int Outcome_Loss = 0;
const int Outcome_Draw = 3;
const int Outcome_Win = 6;

Dictionary<char, int> opponentHandMap = new()
{
    {'A', Shape_Rock},
    {'B', Shape_Paper},
    {'C', Shape_Scissors},
};

Dictionary<char, int> playerHandMap = new()
{
    {'X', Shape_Rock},
    {'Y', Shape_Paper},
    {'Z', Shape_Scissors},
};

Dictionary<char, int> playOutcomeMap = new()
{
    {'X', Outcome_Loss},
    {'Y', Outcome_Draw},
    {'Z', Outcome_Win},
};

Dictionary<(int, int), int> RoundPlay = new()
{
    {(Shape_Rock, Shape_Rock),          Outcome_Draw},
    {(Shape_Rock, Shape_Paper),         Outcome_Loss},
    {(Shape_Rock, Shape_Scissors),      Outcome_Win},

    {(Shape_Paper, Shape_Rock),         Outcome_Win},
    {(Shape_Paper, Shape_Paper),        Outcome_Draw},
    {(Shape_Paper, Shape_Scissors),     Outcome_Loss},

    {(Shape_Scissors, Shape_Rock),      Outcome_Loss},
    {(Shape_Scissors, Shape_Paper),     Outcome_Win},
    {(Shape_Scissors, Shape_Scissors),  Outcome_Draw}
};

Dictionary<(int, int), int> MoveToPlay = new()
{
    {(Shape_Rock, Outcome_Loss),       Shape_Scissors},
    {(Shape_Rock, Outcome_Draw),       Shape_Rock},
    {(Shape_Rock, Outcome_Win),        Shape_Paper},

    {(Shape_Paper, Outcome_Loss),      Shape_Rock},
    {(Shape_Paper, Outcome_Draw),      Shape_Paper},
    {(Shape_Paper, Outcome_Win),       Shape_Scissors},
    
    {(Shape_Scissors, Outcome_Loss),   Shape_Paper},
    {(Shape_Scissors, Outcome_Draw),   Shape_Scissors},
    {(Shape_Scissors, Outcome_Win),    Shape_Rock}
};

int scorePlayer = 0;
foreach(var (OpponentHand, PlayerHand) in File.ReadLines(args[0]).Select(line => (opponentHandMap[line[0]], playerHandMap[line[2]])))
{
    scorePlayer += PlayerHand + RoundPlay[(PlayerHand, OpponentHand)];
}

Console.WriteLine($"Puzzle 1 answer: {scorePlayer}");

scorePlayer = 0;
foreach(var (OpponentHand, GameOutcome) in File.ReadLines(args[0]).Select(line => (opponentHandMap[line[0]], playOutcomeMap[line[2]])))
{
    scorePlayer += MoveToPlay[(OpponentHand, GameOutcome)] + GameOutcome;
}

Console.WriteLine($"Puzzle 2 answer: {scorePlayer}");