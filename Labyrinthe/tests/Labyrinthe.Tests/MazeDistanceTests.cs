using MazeSolver;
using Xunit;

namespace Labyrinthe.Tests
{
    public class MazeDistanceTests
    {
        [Fact]
        public void GetDistance_ReturnsCorrectDistance_ForMazeWithDetour()
        {
            var maze = new Maze(
                "D#.\n" +
                ".#.\n" +
                "..S");

            var distance = maze.GetDistance();

            Assert.Equal(4, distance);
        }

        [Fact]
        public void GetDistance_ReturnsCorrectDistance_ForMazeWithDirectPath()
        {
            var maze = new Maze(
                "....\n" +
                ".##.\n" +
                ".#S.\n" +
                "D...");

            var distance = maze.GetDistance();

            Assert.Equal(3, distance);
        }
    }
}
