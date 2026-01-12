using System.Linq;
using MazeSolver;
using Xunit;

namespace Labyrinthe.Tests
{
    public class MazeParsingTests
    {
        private const string SampleMaze =
            "D..#.\n" +
            "##...\n" +
            ".#.#.\n" +
            "...#.\n" +
            "####S";

        [Fact]
        public void Constructor_ParsesStartExitAndWalls()
        {
            var maze = new Maze(SampleMaze);

            Assert.Equal((0, 0), maze.Start);
            Assert.Equal((4, 4), maze.Exit);

            Assert.True(maze.Grid[0][3]);
            Assert.True(maze.Grid[1][0]);
            Assert.True(maze.Grid[4][3]);

            Assert.False(maze.Grid[0][0]);
            Assert.False(maze.Grid[0][1]);
            Assert.False(maze.Grid[4][4]);
        }

        [Fact]
        public void Constructor_InitializesDistancesWithZeros()
        {
            var maze = new Maze(SampleMaze);

            Assert.Equal(maze.Grid.Length, maze.Distances.Length);

            for (var y = 0; y < maze.Grid.Length; y++)
            {
                Assert.Equal(maze.Grid[y].Length, maze.Distances[y].Length);
                Assert.All(maze.Distances[y], distance => Assert.Equal(0, distance));
            }
        }
    }
}
