using System.Collections.Generic;
using System.Linq;
using MazeSolver;
using Xunit;

namespace Labyrinthe.Tests
{
    public class MazeNeighboursTests
    {
        private const string OpenMaze =
            ".....\n" +
            ".D...\n" +
            ".....\n" +
            ".....\n" +
            "...S.";

        [Fact]
        public void GetNeighbours_AllFourAvailable_ReturnsFour()
        {
            var maze = new Maze(OpenMaze);

            var neighbours = maze.GetNeighbours(2, 2);

            Assert.Equal(
                Sort(new List<(int, int)> { (2, 1), (2, 3), (1, 2), (3, 2) }),
                Sort(neighbours));
        }

        [Fact]
        public void GetNeighbours_TopEdge_SkipsOutside()
        {
            var maze = new Maze(OpenMaze);

            var neighbours = maze.GetNeighbours(2, 0);

            Assert.Equal(
                Sort(new List<(int, int)> { (2, 1), (1, 0), (3, 0) }),
                Sort(neighbours));
        }

        [Fact]
        public void GetNeighbours_BottomEdge_SkipsOutside()
        {
            var maze = new Maze(OpenMaze);

            var neighbours = maze.GetNeighbours(2, 4);

            Assert.Equal(
                Sort(new List<(int, int)> { (2, 3), (1, 4), (3, 4) }),
                Sort(neighbours));
        }

        [Fact]
        public void GetNeighbours_LeftEdge_SkipsOutside()
        {
            var maze = new Maze(OpenMaze);

            var neighbours = maze.GetNeighbours(0, 2);

            Assert.Equal(
                Sort(new List<(int, int)> { (0, 1), (0, 3), (1, 2) }),
                Sort(neighbours));
        }

        [Fact]
        public void GetNeighbours_RightEdge_SkipsOutside()
        {
            var maze = new Maze(OpenMaze);

            var neighbours = maze.GetNeighbours(4, 2);

            Assert.Equal(
                Sort(new List<(int, int)> { (4, 1), (4, 3), (3, 2) }),
                Sort(neighbours));
        }

        [Fact]
        public void GetNeighbours_SkipsWalls()
        {
            var maze = new Maze(
                ".....\n" +
                ".D#..\n" +
                ".#...\n" +
                ".....\n" +
                "...S.");

            var neighbours = maze.GetNeighbours(2, 2);

            Assert.Equal(
                Sort(new List<(int, int)> { (3, 2), (2, 3) }),
                Sort(neighbours));
        }

        [Fact]
        public void GetNeighbours_SkipsStart()
        {
            var maze = new Maze(OpenMaze);

            var neighbours = maze.GetNeighbours(1, 2);

            Assert.Equal(
                Sort(new List<(int, int)> { (0, 2), (2, 2), (1, 3) }),
                Sort(neighbours));
        }

        private static List<(int, int)> Sort(IEnumerable<(int, int)> cells)
        {
            return cells.OrderBy(cell => cell.Item2).ThenBy(cell => cell.Item1).ToList();
        }
    }
}
