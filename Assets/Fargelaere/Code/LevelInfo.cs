using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct LevelInfo : IEquatable<LevelInfo>
{
    public TileBase[,,] tiles;

    public GridEntityInfo[] Entities;

    public Vector3Int startPos;

    public LevelInfo(TileBase[,,] tiles, GridEntityInfo[] entities, Vector3Int startPos)
    {
        this.tiles = tiles;
        Entities = entities;
        this.startPos = startPos;
    }

    /// <summary>
    /// Checks if the two given 3D arrays are equal
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private bool ThreeDArrayEquals(TileBase[,,] a, TileBase[,,] b)
    {
        //Check for equal width, height and depth
        for (int dimension = 0; dimension < 3; dimension++)
        {
            //Arrays are note of the same size
            if (a.GetLength(dimension) != b.GetLength(dimension))
            {
                return false;
            }
        }

        //Check value equality
        for (int x = 0; x < a.GetLength(0); x++)
        {
            for (int y = 0; y < a.GetLength(1); y++)
            {
                for (int z = 0; z < a.GetLength(2); z++)
                {
                    //Values at position (x, y, z) are not equal
                    if (a[x, y, z] != b[x, y, z])
                    {
                        return false;
                    }
                }
            }
        }

        //All values are equal
        return true;
    }





    public override bool Equals(object obj)
    {
        if (obj is LevelInfo level)
        {
            return Equals(level);
        }

        return false;
    }

    public bool Equals(LevelInfo other)
    {
        return startPos == other.startPos &&
            Entities.SequenceEqual(other.Entities) /* &&
            ThreeDArrayEquals(tiles, other.tiles)*/;
    }

    public static bool operator ==(LevelInfo left, LevelInfo right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(LevelInfo left, LevelInfo right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return tiles.GetHashCode() ^ Entities.GetHashCode() ^ startPos.GetHashCode();
    }

    public struct GridEntityInfo
	{
        public GridEntity Original;

        public GridEntityInfo(GridEntity original)
        {
            Original = UnityEngine.Object.Instantiate(original.gameObject).GetComponent<GridEntity>();
            Original.hideFlags = HideFlags.HideAndDontSave;
        }

        public override bool Equals(object obj)
        {
            if (obj is GridEntityInfo i)
            {
                return Original.transform.position == i.Original.transform.position;
            }

            return false;
        }

    }
}
