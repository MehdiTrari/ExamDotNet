using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MazeSolver;

const string BaseMaze =
    "D..#.\n" +
    "##...\n" +
    ".#.#.\n" +
    "...#.\n" +
    "####S";

const string LargeMaze =
    ".#.......\n" +
    "D#.#####.\n" +
    ".#.#...#.\n" +
    ".....#.#.\n" +
    "###.#..#.\n" +
    ".##.#.##.\n" +
    "..#.#..#.\n" +
    "#.#.##.#.\n" +
    "....#S.#.";

Run(args);

static void Run(string[] args)
{
    var useColor = ShouldUseColor(args);
    var cleanArgs = args.Where(arg => !IsFlag(arg, "no-color") && !IsFlag(arg, "color")).ToArray();

    if (cleanArgs.Length == 0)
    {
        RunExample("Base example", BaseMaze, useColor);
        Console.WriteLine();
        RunExample("Large example", LargeMaze, useColor);
        return;
    }

    var arg = cleanArgs[0];
    if (IsFlag(arg, "base"))
    {
        RunExample("Base example", BaseMaze, useColor);
        return;
    }

    if (IsFlag(arg, "large") || IsFlag(arg, "big"))
    {
        RunExample("Large example", LargeMaze, useColor);
        return;
    }

    if (!File.Exists(arg))
    {
        Console.WriteLine($"Fichier de labyrinthe introuvable: {arg}");
        Console.WriteLine("Utilisation :");
        Console.WriteLine("  dotnet run --project .../MazeRunner");
        Console.WriteLine("  dotnet run --project .../MazeRunner -- base|large [--no-color]");
        Console.WriteLine("  dotnet run --project .../MazeRunner /chemin/vers/maze.txt");
        Environment.Exit(1);
    }

    var input = File.ReadAllText(arg).Replace("\r", string.Empty).TrimEnd();
    RunExample($"File: {arg}", input, useColor);
}

static void RunExample(string title, string input, bool useColor)
{
    var maze = new Maze(input);
    var distance = maze.GetDistance();
    var path = maze.GetShortestPath();

    WriteTitle(title, useColor);
    PrintMazeWithPath(input, path, useColor);
    Console.WriteLine();
    WriteMetric("Distance", distance < 0 ? "Aucun chemin" : distance.ToString(), useColor);
    if (path.Count > 0)
    {
        WriteMetric("Etapes", (path.Count - 1).ToString(), useColor);
    }
    WriteLegend(useColor);

    if (path.Count == 0)
    {
        Console.WriteLine("Aucun chemin trouve.");
    }
    else
    {
        Console.WriteLine("Chemin (depart vers sortie) :");
        Console.WriteLine(string.Join(" -> ", path.AsEnumerable().Reverse()
            .Select(cell => $"({cell.Item1},{cell.Item2})")));
    }
}

static void PrintMazeWithPath(string input, IList<(int, int)> path, bool useColor)
{
    if (!useColor)
    {
        Console.WriteLine(RenderMazeWithPath(input, path));
        return;
    }

    var rows = input.Replace("\r", string.Empty)
        .Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var pathCells = new HashSet<(int x, int y)>(path);
    var maxWidth = rows.Max(row => row.Length);

    WriteAxisHeader(maxWidth);
    WriteBorder(maxWidth);

    for (var y = 0; y < rows.Length; y++)
    {
        WriteAxisLabel($"{y,2} |");
        for (var x = 0; x < maxWidth; x++)
        {
            var cell = x < rows[y].Length ? rows[y][x] : '#';
            if (pathCells.Contains((x, y)) && cell != 'D' && cell != 'S' && cell != '#')
            {
                cell = '*';
            }

            WriteCell(cell);
            if (x < maxWidth - 1)
            {
                Console.Write(' ');
            }
        }
        WriteAxisLabel("|");
        Console.WriteLine();
    }

    WriteBorder(maxWidth);
    ResetColor();
}

static string RenderMazeWithPath(string input, IList<(int, int)> path)
{
    var rows = input.Replace("\r", string.Empty)
        .Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var pathCells = new HashSet<(int x, int y)>(path);
    var maxWidth = rows.Max(row => row.Length);
    var sb = new StringBuilder();

    sb.Append("    ");
    for (var x = 0; x < maxWidth; x++)
    {
        sb.Append(x % 10);
        if (x < maxWidth - 1)
        {
            sb.Append(' ');
        }
    }
    sb.AppendLine();

    sb.Append("   +");
    sb.Append(new string('-', maxWidth * 2 - 1));
    sb.AppendLine("+");

    for (var y = 0; y < rows.Length; y++)
    {
        sb.Append($"{y,2} |");
        for (var x = 0; x < maxWidth; x++)
        {
            var cell = x < rows[y].Length ? rows[y][x] : '#';
            if (pathCells.Contains((x, y)) && cell != 'D' && cell != 'S' && cell != '#')
            {
                cell = '*';
            }

            sb.Append(cell);
            if (x < maxWidth - 1)
            {
                sb.Append(' ');
            }
        }
        sb.AppendLine("|");
    }

    sb.Append("   +");
    sb.Append(new string('-', maxWidth * 2 - 1));
    sb.Append("+");

    return sb.ToString();
}

static bool IsFlag(string arg, string value)
{
    return string.Equals(arg, value, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(arg, $"--{value}", StringComparison.OrdinalIgnoreCase);
}

static bool ShouldUseColor(string[] args)
{
    if (args.Any(arg => IsFlag(arg, "no-color")))
    {
        return false;
    }

    if (args.Any(arg => IsFlag(arg, "color")))
    {
        return true;
    }

    return !Console.IsOutputRedirected;
}

static void WriteTitle(string title, bool useColor)
{
    if (useColor)
    {
        Console.Write("\u001b[1m");
        SetColor(ConsoleColor.Cyan);
    }

    Console.WriteLine($"== {title} ==");
    ResetColor();
    if (useColor)
    {
        Console.Write("\u001b[22m");
    }
}

static void WriteMetric(string label, string value, bool useColor)
{
    Console.Write($"{label}: ");
    if (useColor)
    {
        SetColor(ConsoleColor.Green);
    }
    Console.WriteLine(value);
    ResetColor();
}

static void WriteLegend(bool useColor)
{
    Console.WriteLine("Legende :");
    WriteLegendLine('#', "mur", ConsoleColor.DarkGray, useColor);
    WriteLegendLine('.', "case libre", ConsoleColor.Gray, useColor);
    WriteLegendLine('*', "chemin", ConsoleColor.Yellow, useColor);
    WriteLegendLine('D', "depart", ConsoleColor.Green, useColor);
    WriteLegendLine('S', "sortie", ConsoleColor.Red, useColor);
}

static void WriteLegendLine(char symbol, string label, ConsoleColor color, bool useColor)
{
    Console.Write("  ");
    if (useColor)
    {
        SetColor(color);
    }
    Console.Write(symbol);
    ResetColor();
    Console.WriteLine($" = {label}");
}

static void WriteAxisHeader(int maxWidth)
{
    SetColor(ConsoleColor.DarkGray);
    Console.Write("    ");
    for (var x = 0; x < maxWidth; x++)
    {
        Console.Write(x % 10);
        if (x < maxWidth - 1)
        {
            Console.Write(' ');
        }
    }
    Console.WriteLine();
    ResetColor();
}

static void WriteBorder(int maxWidth)
{
    SetColor(ConsoleColor.DarkGray);
    Console.Write("   +");
    Console.Write(new string('-', maxWidth * 2 - 1));
    Console.WriteLine("+");
    ResetColor();
}

static void WriteAxisLabel(string text)
{
    SetColor(ConsoleColor.DarkGray);
    Console.Write(text);
    ResetColor();
}

static void WriteCell(char cell)
{
    switch (cell)
    {
        case '#':
            SetColor(ConsoleColor.DarkGray);
            Console.Write(cell);
            ResetColor();
            break;
        case 'D':
            SetColor(ConsoleColor.Green);
            Console.Write(cell);
            ResetColor();
            break;
        case 'S':
            SetColor(ConsoleColor.Red);
            Console.Write(cell);
            ResetColor();
            break;
        case '*':
            SetColor(ConsoleColor.Yellow);
            Console.Write(cell);
            ResetColor();
            break;
        default:
            Console.Write(cell);
            break;
    }
}

static void SetColor(ConsoleColor color)
{
    Console.ForegroundColor = color;
}

static void ResetColor()
{
    Console.ResetColor();
}
