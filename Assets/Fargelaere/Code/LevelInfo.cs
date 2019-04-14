
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct LevelInfo
{
	/// <summary>
	/// unactive gameobjects stored under given transform
	/// </summary>
    public List<GridEntityInfo> Entities;

	public bool hasInfo;

	public LevelInfo(List<GridEntityInfo> entities)
	{
		Entities = entities;
		hasInfo = true;
	}

	public void DeleteInfo()
	{
		if (!hasInfo)
		{
			Debug.Log("Tried to delete already deleted level Info");
			return;
		}
		foreach (GridEntityInfo item in Entities)
		{
			item.DeleteOriginal();
		}
		hasInfo = false;
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
		return GridEntityInfo.CompareLists(Entities,other.Entities);
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
        return Entities.GetHashCode();
    }

    public struct GridEntityInfo
	{
        public GridEntity Original;

        public GridEntityInfo(GridEntity original, Transform trans)
        {
            Original = UnityEngine.Object.Instantiate(original.gameObject, trans).GetComponent<GridEntity>();
			Original.transform.position = original.transform.position;
			Original.gameObject.SetActive(false);
        }

        public override bool Equals(object obj)
        {
            if (obj is GridEntityInfo i)
            {
				return Original.Equals(i.Original);
            }

            return false;
        }

		public void DeleteOriginal()
		{
			Object.Destroy(Original.gameObject);
		}

		public static bool CompareLists(List<GridEntityInfo> left , List<GridEntityInfo> right)
		{
			
			return left.All(right.Contains) && right.All(left.Contains);
		}

		public override int GetHashCode()
		{ 
			return Original.GetHashCode();
		}
	}
}
