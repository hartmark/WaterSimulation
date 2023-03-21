using System.Text;

namespace WaterSimulation;

public static class TilesExtensions
{
    public static string GetTilePrint(this List<List<TileData>> tiles, bool round = false)
    {
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < tiles.Count; i++)
        {
            stringBuilder.Append("{ ");
            for (var j = 0; j < tiles[0].Count; j++)
            {
                if (round)
                {
                    stringBuilder.Append(tiles[i][j].TotalHeight.ToString("N4"));
                }
                else
                {
                    stringBuilder.Append(tiles[i][j].TotalHeight);
                }
                if (j != tiles[0].Count - 1)
                {
                    stringBuilder.Append(", ");
                }
            }

            stringBuilder.Append(" }");
            if (i != tiles.Count - 1)
            {
                stringBuilder.Append(",");
            }

            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }

    public static void Commit(this List<List<TileData>> tiles)
    {
        for (var i = 0; i < tiles.Count; i++)
        {
            for (var j = 0; j < tiles[0].Count; j++)
            {
                tiles[i][j].Commit();
            }
        }
    }
}