using Xunit;
using Xunit.Abstractions;

namespace WaterSimulation;

public class TileTests
{
    private readonly WaterSimulation _waterSimulation;

    public TileTests(ITestOutputHelper testOutputHelper)
    {
        _waterSimulation = new WaterSimulation(testOutputHelper);
    }

    [Fact]
    public void AllTilesLevel1()
    {
        int[,] heightMap =
        {
            { 1, 1, 1 },
            { 1, 1, 1 },
            { 1, 1, 1 }
        };

        var tiles = _waterSimulation.SeedTiles(heightMap);
        _waterSimulation.ProcessWater(tiles);
        Assert.Equal(
            "{ 2.0000, 2.0000, 2.0000 },\n"+
            "{ 2.0000, 2.0000, 2.0000 },\n"+ 
            "{ 2.0000, 2.0000, 2.0000 }\n", 
            tiles.GetTilePrint(round:true));
    }

    
    [Fact]
    public void OneTileOneHigher()
    {
        int[,] heightMap =
        {
            { 1, 1, 1 },
            { 1, 2, 1 },
            { 1, 1, 1 }
        };

        var tiles = _waterSimulation.SeedTiles(heightMap);
        _waterSimulation.ProcessWater(tiles);
        Assert.Equal(
            "{ 2.1111, 2.1111, 2.1111 },\n"+
            "{ 2.1111, 2.1111, 2.1111 },\n"+ 
            "{ 2.1111, 2.1111, 2.1111 }\n", 
            tiles.GetTilePrint(round:true));
    }
    
    [Fact]
    public void OneTileMuchHigher()
    {
        int[,] heightMap =
        {
            { 1, 1, 1 },
            { 1, 12, 1 },
            { 1, 1, 1 }
        };

        var tiles = _waterSimulation.SeedTiles(heightMap);
        _waterSimulation.ProcessWater(tiles);
        Assert.Equal(
            "{ 2.1250, 2.1250, 2.1250 },\n"+
            "{ 2.1250, 12.0000, 2.1250 },\n"+ 
            "{ 2.1250, 2.1250, 2.1250 }\n", 
            tiles.GetTilePrint(round:true));
    }
    
    [Fact]
    public void SplitInHalf()
    {
        int[,] heightMap =
        {
            { 1, 12, 1 },
            { 1, 12, 1 },
            { 1, 12, 1 }
        };

        var tiles = _waterSimulation.SeedTiles(heightMap);
        _waterSimulation.ProcessWater(tiles);

        Assert.Equal(
            "{ 2.5000, 12.0000, 2.5000 },\n"+
            "{ 2.5000, 12.0000, 2.5000 },\n"+ 
            "{ 2.5000, 12.0000, 2.5000 }\n", 
            tiles.GetTilePrint(round:true));
    }
    
    [Fact]
    public void Cross()
    {
        int[,] heightMap =
        {
            { 1, 12, 1 },
            { 12, 12, 12 },
            { 1, 12, 1 }
        };

        var tiles = _waterSimulation.SeedTiles(heightMap);
        _waterSimulation.ProcessWater(tiles);

        Assert.Equal(
            "{ 3.2500, 12.0000, 3.2500 },\n"+
            "{ 12.0000, 12.0000, 12.0000 },\n"+ 
            "{ 3.2500, 12.0000, 3.2500 }\n", 
            tiles.GetTilePrint(round:true));
    }
}