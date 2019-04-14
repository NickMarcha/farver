using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Represents a GridEntity that can be pushed by swiping inputs
/// </summary>
public abstract class Pushable : GridEntity
{
    /// <summary>
    /// The time it will take for this object to move one tile
    /// </summary>
    public float SlideTime;

    /// <summary>
    /// Is the object currently sliding?
    /// </summary>
    public bool Sliding { get; private set; }

    /// <summary>
    /// Pushes the object in the given direction until it hits something solid
    /// </summary>
    /// <param name="direction"></param>
    public void Push(Direction4 direction)
    {
        if (Sliding)
        {
            return;
        }
		//TODO: this should only run if this was the first entity to be called, we should also handle cases where nothing actually changed on the map
        LevelController.SaveOldState();
        StartCoroutine(CoPush(direction));
    }

    /// <summary>
    /// Immediately changes the direction the object is sliding. Should only be called from LPipeTiles
    /// </summary>
    /// <param name="newDirection"></param>
    public void ChangeDirection(Direction4 newDirection)
    {
        StopAllCoroutines();
        Sliding = false;

        StartCoroutine(CoPush(newDirection));
    }
    
    private IEnumerator CoPush(Direction4 direction)
    {
		yield return new WaitForEndOfFrame();
        /* local */ Vector3Int getNextPosition()
        {
            return TilePosition + Vector3Int.RoundToInt(direction.ToVector2());
        }

        Sliding = true;
        while (CanSlideIntoPosition(getNextPosition(), direction))
        {
            //Slide one tile over
            yield return CoSlideToTilePosition(getNextPosition(), SlideTime);

            //Run events
            OnSlideOnto(TileMap.GetTile(TilePosition) as PuzzleTile, direction);
            (TileMap.GetTile(TilePosition) as PuzzleTile)?.OnPaintSlide(this, TilePosition, direction);

            foreach (GridEntity i in GetGridEntities(TileMap).Where(i => i.TilePosition == TilePosition))
            {
                OnSlideIntoObject(i);
                i.OnSlideIntoObject(this);
            }

        }

        Sliding = false;
    }

    /// <summary>
    /// Can this tile slide into the given position or is it occupied?
    /// </summary>
    /// <param name="tilePosition"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    private bool CanSlideIntoPosition(Vector3Int tilePosition, Direction4 dir)
    {
        //The position is occupied by a solid tile
        if (!((TileMap.GetTile(tilePosition) as PuzzleTile)?.CanPass(dir) ?? true))
        {
            return false;
        }

        foreach (GridEntity i in GetGridEntities(TileMap).Where(i => i.TilePosition == tilePosition))
        {
            //The object is occupied by a solid object
            if (!i.CanPass(dir))
            {
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Event: Triggered when the object slides upon a tile
    /// </summary>
    /// <param name="tile"></param>
    protected virtual void OnSlideOnto(PuzzleTile tile, Direction4 dir) { }

    /// <summary>
    /// Pushes every pushable object in the given tilemap
    /// </summary>
    /// <param name="direction"></param>
    public static void PushAll(Tilemap map, Direction4 direction)
    {
        throw new NotImplementedException();
    }
}
