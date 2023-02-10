using System.Diagnostics;
using Humanizer;
using WaterSimulation;

int[,] heightMap =
{
    { 1, 1, 1 },
    { 1, 12, 1 },
    { 1, 1, 1 }
};

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

Console.WriteLine("initial state");
Console.WriteLine(tiles.GetTilePrint());

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
        var highestPoint = highestTiles.Key;
        foreach (var tile in highestTiles.Where(x => !x.Drained))
        {
            var adjacentTiles = tile.GetNeighbours(tiles).ToList();
            if (!adjacentTiles.Any())
            {
                continue;
            }

            var minWaterDifference = Math.Max(highestPoint - adjacentTiles.Max(x => x.TotalHeight), 0) / 2;
            
            if (minWaterDifference < 0.00001M)
            {
                continue;
            }

            var waterAddition = minWaterDifference / adjacentTiles.Count;

            foreach (var adjacentTile in adjacentTiles)
            {
                adjacentTile.WaterAmountPending += waterAddition;
            }

            tile.WaterAmount -= minWaterDifference;
        }

        tiles.Commit();
        Console.WriteLine("after iteration state");
        Console.WriteLine(tiles.GetTilePrint());
    }

    if (initialState == tiles.GetTilePrint())
    {
        break;
    }
}

Console.WriteLine($"we're done!, Executed in {steps} steps");
Console.WriteLine($"Elapsed tipe: {stopWatch.Elapsed.Humanize(2)}");
Console.WriteLine(tiles.GetTilePrint());