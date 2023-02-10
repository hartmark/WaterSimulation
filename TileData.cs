namespace WaterSimulation;

public class TileData
{
    public int Height { get; }
    public decimal WaterAmount { get; set; }
    public decimal WaterAmountPending { get; set; }
    public int I { get; }
    public int J { get; }

    public decimal TotalHeight => Height + WaterAmount;
    public bool Drained { get; set; }

    public TileData(int height, int waterAmount, int i, int j)
    {
        Height = height;
        WaterAmount = waterAmount;
        I = i;
        J = j;
    }

    public IEnumerable<TileData> GetNeighbours(List<List<TileData>> tiles)
    {
        // left
        if (TryGetTile(tiles, I - 1, J, out var tile))
        {
            if (tile.TotalHeight < TotalHeight)
            {
                yield return tile;
            }
        }
        
        // right
        if (TryGetTile(tiles, I + 1, J, out tile))
        {
            if (tile.TotalHeight < TotalHeight)
            {
                yield return tile;
            }
        }

        // up
        if (TryGetTile(tiles, I, J - 1, out tile))
        {
            if (tile.TotalHeight < TotalHeight)
            {
                yield return tile;
            }
        }
        
        // down
        if (TryGetTile(tiles, I, J + 1, out tile))
        {
            if (tile.TotalHeight < TotalHeight)
            {
                yield return tile;
            }
        }
    }

    private bool TryGetTile(List<List<TileData>> tiles, int i, int j, out TileData tile)
    {
        try
        {
            tile = tiles[i][j];
        }
        catch (ArgumentOutOfRangeException)
        {
            tile = null!;
            return false;
        }
        
        return true;
    }

    public void Commit()
    {
        WaterAmount += WaterAmountPending;
        WaterAmountPending = 0;
    }
}