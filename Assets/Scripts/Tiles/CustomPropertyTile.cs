using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    GOAL,
    ENEMY,
    SCOREITEM,
    ADDITIONALMIRROR,
    MIRRORSHARD,
    DANGER
}

[CreateAssetMenu(fileName = "CustomPropertyTile", menuName = "Tiles/CustomPropertyTile")]
public class CustomPropertyTile : Tile
{
    public TileType type;
}
