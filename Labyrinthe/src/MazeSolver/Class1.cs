using System;
using System.Collections.Generic;

namespace MazeSolver
{
    public class Maze
    {
        public int[][] Distances { get; init; }
        public bool[][] Grid { get; init; }
        public Queue<(int x, int y, int distance)> ToVisit { get; init; }
        public (int x, int y) Start { get; init; }
        public (int x, int y) Exit { get; init; }

        public Maze(string maze)
        {
            if (maze == null)
            {
                throw new ArgumentNullException(nameof(maze));
            }

            var rows = maze.Replace("\r", string.Empty)
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);

            Grid = new bool[rows.Length][];
            Distances = new int[rows.Length][];
            ToVisit = new Queue<(int x, int y, int distance)>();

            for (var y = 0; y < rows.Length; y++)
            {
                var row = rows[y];
                Grid[y] = new bool[row.Length];
                Distances[y] = new int[row.Length];

                for (var x = 0; x < row.Length; x++)
                {
                    var cell = row[x];
                    switch (cell)
                    {
                        case '#':
                            Grid[y][x] = true;
                            break;
                        case 'D':
                            Start = (x, y);
                            Grid[y][x] = false;
                            break;
                        case 'S':
                            Exit = (x, y);
                            Grid[y][x] = false;
                            break;
                        case '.':
                            Grid[y][x] = false;
                            break;
                        default:
                            throw new ArgumentException($"Invalid cell '{cell}'", nameof(maze));
                    }
                }
            }

            ToVisit.Enqueue((Start.x, Start.y, 0));
        }

        public int GetDistance()
        {
            if (Exit == Start)
            {
                return 0;
            }

            if (Distances[Exit.y][Exit.x] != 0)
            {
                return Distances[Exit.y][Exit.x];
            }

            while (ToVisit.Count > 0)
            {
                if (Fill())
                {
                    return Distances[Exit.y][Exit.x];
                }
            }

            return -1;
        }

        public IList<(int, int)> GetNeighbours(int x, int y)
        {
            var neighbours = new List<(int, int)>();
            AddNeighbourIfValid(neighbours, x, y - 1);
            AddNeighbourIfValid(neighbours, x, y + 1);
            AddNeighbourIfValid(neighbours, x - 1, y);
            AddNeighbourIfValid(neighbours, x + 1, y);
            return neighbours;
        }

        public bool Fill()
        {
            if (ToVisit.Count == 0)
            {
                return false;
            }

            var (x, y, distance) = ToVisit.Dequeue();

            if (Exit == (x, y))
            {
                Distances[y][x] = distance;
                return true;
            }

            if (Distances[y][x] != 0)
            {
                return false;
            }

            Distances[y][x] = distance;

            foreach (var (nx, ny) in GetNeighbours(x, y))
            {
                ToVisit.Enqueue((nx, ny, distance + 1));
            }

            return false;
        }

        public IList<(int, int)> GetShortestPath()
        {
            var path = new List<(int, int)>();

            if (Exit == Start)
            {
                path.Add(Start);
                return path;
            }

            var distance = Distances[Exit.y][Exit.x];
            if (distance == 0)
            {
                distance = GetDistance();
            }

            if (distance <= 0)
            {
                return path;
            }

            var current = Exit;
            var currentDistance = Distances[current.y][current.x];
            path.Add(current);

            while (current != Start && currentDistance > 0)
            {
                var next = GetAdjacentWithDistance(current.x, current.y, currentDistance - 1);
                if (next == null)
                {
                    break;
                }

                current = next.Value;
                currentDistance = Distances[current.y][current.x];
                path.Add(current);
            }

            return path;
        }

        private void AddNeighbourIfValid(ICollection<(int, int)> neighbours, int x, int y)
        {
            if (y < 0 || y >= Grid.Length)
            {
                return;
            }

            if (x < 0 || x >= Grid[y].Length)
            {
                return;
            }

            if (Start == (x, y))
            {
                return;
            }

            if (Grid[y][x])
            {
                return;
            }

            neighbours.Add((x, y));
        }

        private (int x, int y)? GetAdjacentWithDistance(int x, int y, int targetDistance)
        {
            var candidates = new List<(int x, int y)>
            {
                (x, y - 1),
                (x, y + 1),
                (x - 1, y),
                (x + 1, y)
            };

            foreach (var candidate in candidates)
            {
                if (!IsWithinBounds(candidate.x, candidate.y))
                {
                    continue;
                }

                if (Grid[candidate.y][candidate.x])
                {
                    continue;
                }

                if (Distances[candidate.y][candidate.x] == targetDistance)
                {
                    return candidate;
                }
            }

            return null;
        }

        private bool IsWithinBounds(int x, int y)
        {
            return y >= 0 && y < Grid.Length && x >= 0 && x < Grid[y].Length;
        }
    }
}
