using System.Collections.Generic;
using System.Linq;
using MazeSolver;
using Xunit;

namespace Labyrinthe.Tests
{
    public class MazeFillTests
    {
        [Fact]
        public void Constructor_SeedsQueueWithStartDistanceZero()
        {
            var maze = new Maze("D.S");

            Assert.Single(maze.ToVisit);
            Assert.Equal((0, 0, 0), maze.ToVisit.Peek());
        }

        [Fact]
        public void Fill_ReturnsFalseUntilExitThenTrue()
        {
            var maze = new Maze("DS");

            var first = maze.Fill();
            var second = maze.Fill();

            Assert.False(first);
            Assert.True(second);
            Assert.Equal(1, maze.Distances[0][1]);
        }

        [Fact]
        public void Fill_IgnoresAlreadySetCellAndDoesNotEnqueueNeighbours()
        {
            var maze = new Maze(
                "D..\n" +
                "...\n" +
                "..S");

            maze.ToVisit.Clear();
            maze.Distances[0][1] = 2;
            maze.ToVisit.Enqueue((1, 0, 2));

            var result = maze.Fill();

            Assert.False(result);
            Assert.Empty(maze.ToVisit);
        }

        [Fact]
        public void Fill_EnqueuesNeighboursWithIncrementedDistance()
        {
            var maze = new Maze(
                "...\n" +
                ".D.\n" +
                "..S");

            var result = maze.Fill();

            Assert.False(result);
            Assert.Equal(
                Sort(new List<(int, int, int)>
                {
                    (1, 0, 1),
                    (1, 2, 1),
                    (0, 1, 1),
                    (2, 1, 1)
                }),
                Sort(maze.ToVisit));
        }

        private static List<(int, int, int)> Sort(IEnumerable<(int, int, int)> cells)
        {
            return cells.OrderBy(cell => cell.Item2)
                .ThenBy(cell => cell.Item1)
                .ThenBy(cell => cell.Item3)
                .ToList();
        }
    }
}
