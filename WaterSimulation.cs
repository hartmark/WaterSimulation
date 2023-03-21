using System.Diagnostics;
using Humanizer;
using Xunit;
using Xunit.Abstractions;

namespace WaterSimulation;

public class WaterSimulation
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly bool _debug;
    private readonly Random _random;

    public WaterSimulation(ITestOutputHelper testOutputHelper, bool debug = false)
    {
        _testOutputHelper = testOutputHelper;
        _random = new Random();
        _debug = debug;
    }

    
    public List<List<TileData>> SeedTiles(int[,] heightMap)
    {
        const int initialAddedWater = 1;

        var tiles = new List<List<TileData>>();

        for (var i = 0; i < heightMap.GetLength(0); i++)
        {
            var row = new List<TileData>();
            for (var j = 0; j < heightMap.GetLength(1); j++)
            {
                row.Add(new TileData(heightMap[i, j], initialAddedWater, i, j));
            }

            tiles.Add(row);
        }

        _testOutputHelper.WriteLine("initial state");
        _testOutputHelper.WriteLine(tiles.GetTilePrint());
        return tiles;
    }
    
    public void ProcessWater(List<List<TileData>> tiles)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var steps = 0;
        while (steps < 10_000)
        {
            steps++;

            var initialState = tiles.GetTilePrint();

            foreach (var highestTiles in tiles
                         .SelectMany(x => x.ToList())
                         .ToLookup(key => key.TotalHeight, value => value)
                         .OrderByDescending(x => x.Key))
            {
                foreach (var tile in highestTiles.OrderBy(_ => _random.Next()).Where(x => !x.Drained))
                {
                    var adjacentTiles = tile.GetNeighbours(tiles).ToList();
                    if (!adjacentTiles.Any())
                    {
                        continue;
                    }

                    var minWaterDifference = Math.Max(tile.TotalHeight - adjacentTiles.Max(x => x.TotalHeight), 0) / 2;
                    minWaterDifference = Math.Min(minWaterDifference, tile.WaterAmount);

                    if (minWaterDifference < 0.0000000000000000000000001M)
                    {
                        continue;
                    }

                    var waterAddition = minWaterDifference / adjacentTiles.Count;

                    foreach (var adjacentTile in adjacentTiles.OrderBy(_ => _random.Next()))
                    {
                        adjacentTile.WaterAmountPending += waterAddition;
                    }

                    tile.WaterAmount -= minWaterDifference;
                }

                tiles.Commit();
                if (_debug)
                {
                    _testOutputHelper.WriteLine("after iteration state");
                    _testOutputHelper.WriteLine(tiles.GetTilePrint());
                }
            }

            if (initialState == tiles.GetTilePrint())
            {
                break;
            }
        }

        Assert.Equal(tiles.Sum(x => x.Count), Math.Round(tiles.Sum(x => x.Sum(y => y.WaterAmount)), 15));
        
        _testOutputHelper.WriteLine($"we're done!, Executed in {steps} steps");
        _testOutputHelper.WriteLine($"Elapsed time: {stopWatch.Elapsed.Humanize(2)}");
        _testOutputHelper.WriteLine(tiles.GetTilePrint(round: true));
    }
}