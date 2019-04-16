using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

/// <summary>
/// Represents a GridEntity that can be pushed by swiping inputs
/// </summary>
public abstract class Pushable : GridEntity
{
	public static UnityEvent blockStopped = new UnityEvent();
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
        while (CanSlideIntoPosition(this, getNextPosition(), direction))
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
        OnSlideStop();
    }

    /// <summary>
    /// Can this tile slide into the given position or is it occupied?
    /// </summary>
    /// <param name="tilePosition"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    private bool CanSlideIntoPosition(GridEntity sender, Vector3Int tilePosition, Direction4 dir)
    {
        //The position is occupied by a solid tile
        if (!((TileMap.GetTile(tilePosition) as PuzzleTile)?.CanPass(dir) ?? true))
        {
            return false;
        }

        foreach (GridEntity i in GetGridEntities(TileMap).Where(i => i.TilePosition == tilePosition))
        {
            //The object is occupied by a solid object
            if (!i.CanPass(sender, dir))
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
    /// Event: Triggered when the object stops moving
    /// </summary>
    protected virtual void OnSlideStop() {
		blockStopped?.Invoke();
		AudioController.PlayThumpLow();
	}

    /// <summary>
    /// Pushes every pushable object in the given tilemap
    /// </summary>
    /// <param name="direction"></param>
    public static void PushAll(Tilemap map, Direction4 direction)
    {
        //Save -> Wait 1 frame
        IEnumerable<Pushable> pushables = GetGridEntities(map).OfType<Pushable>();

        //Sorts array based on the direction they are being pushed so that objects are pushed in the correct order
        switch (direction)
        {
            case Direction4.Up:
                pushables = pushables.OrderByDescending(i => i.TilePosition.y);
                break;
            case Direction4.Right:
                pushables = pushables.OrderByDescending(i => i.TilePosition.x);
                break;
            case Direction4.Down:
                pushables = pushables.OrderBy(i => i.TilePosition.y);
                break;
            case Direction4.Left:
                pushables = pushables.OrderBy(i => i.TilePosition.x);
                break;
            default:
                break;
        }

        pushables.ForEach(i => i.Push(direction));

    }
}
