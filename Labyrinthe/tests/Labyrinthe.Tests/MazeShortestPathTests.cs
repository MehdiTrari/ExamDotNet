using System.Collections.Generic;
using MazeSolver;
using Xunit;

namespace Labyrinthe.Tests
{
    public class MazeShortestPathTests
    {
        [Fact]
        public void GetShortestPath_ReturnsExpectedPath_ForMazeWithDetour()
        {
            var maze = new Maze(
                "D#.\n" +
                ".#.\n" +
                "..S");

            var path = maze.GetShortestPath();

            var expected = new List<(int, int)>
            {
                (2, 2),
                (1, 2),
                (0, 2),
                (0, 1),
                (0, 0)
            };

            Assert.Equal(expected, path);
        }

        [Fact]
        public void GetShortestPath_ReturnsExpectedPath_ForMazeWithDirectPath()
        {
            var maze = new Maze(
                "....\n" +
                ".##.\n" +
                ".#S.\n" +
                "D...");

            var path = maze.GetShortestPath();

            var expected = new List<(int, int)>
            {
                (2, 2),
                (2, 3),
                (1, 3),
                (0, 3)
            };

            Assert.Equal(expected, path);
        }
    }
}
