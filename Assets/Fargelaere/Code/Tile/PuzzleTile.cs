using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Puzzle Tile")]
/// <summary>
/// Represents a tile used in this game
/// </summary>
public class PuzzleTile : Tile
{
    /// <summary>
    /// Is this tile solid
    /// </summary>
    public bool Solid;
}
