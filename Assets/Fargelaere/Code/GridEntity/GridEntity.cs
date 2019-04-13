using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Represents a dynamic object bound to a tile map
/// </summary>
public class GridEntity : MonoBehaviour
{
    /// <summary>
    /// The tilemap this object is connected to
    /// </summary>
    protected Tilemap TileMap;

    /// <summary>
    /// The position of this object in tilemap coordinates
    /// </summary>
    public Vector3Int TilePosition
    {
        //TODO: this needs to be handled, changed because of bug after loading map
        get => TileMap ? TileMap.LocalToCell(transform.position) : GetComponentInParent<Tilemap>().LocalToCell(transform.position);
        set => TileMap.CellToLocal(value);
    }

    private void Awake()
    {
        TileMap = GetComponentInParent<Tilemap>();
    }

    /// <summary>
    /// Moves this object to a given tile over a specific amount of time
    /// </summary>
    /// <param name="tilePosition"></param>
    /// <param name="time"></param>
    public void SlideToTilePosition(Vector3Int tilePosition, float time)
    {
        StopCoroutine(nameof(CoSlideToTilePosition));
        StartCoroutine(CoSlideToTilePosition(tilePosition, time));
    }

    protected IEnumerator CoSlideToTilePosition(Vector3Int tilePosition, float time)
    {        
        Vector3 originalPosition = transform.localPosition;
        Vector3 targetPosition = TileMap.CellToLocal(tilePosition);

        float elapsedSeconds = 0f;
        while (elapsedSeconds < time)
        {
            //Lerp position
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedSeconds / time);
            elapsedSeconds += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //Error correct position
        transform.position = targetPosition;
    }

    /// <summary>
    /// Can this GridEntity allow other objects to pass through it when they are moving in the given direction
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public virtual bool CanPass(Direction4 incommingDirection)
    {
        return true;
    }

    /// <summary>
    /// Gets every GridEntity in a given tilemap
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static IEnumerable<GridEntity> GetGridEntities(Tilemap map)
    {
        return map.GetComponentsInChildren<GridEntity>();
    }
}
