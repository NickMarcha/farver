using System;
using UnityEngine;

public struct LevelInfo
{
	/// <summary>
	/// unactive gameobjects stored under given transform
	/// </summary>
    public GridEntityInfo[] Entities;

	public bool hasInfo;

	public LevelInfo(GridEntityInfo[] entities)
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
		return GridEntityInfo.CompareArrays(Entities,other.Entities);
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
				return Original.Equals(i);
            }

            return false;
        }

		public void DeleteOriginal()
		{
			UnityEngine.Object.Destroy(Original.gameObject);
		}

		public static bool CompareArrays(GridEntityInfo [] left , GridEntityInfo[] right)
		{
			if(left.Length != right.Length)
			{
				return false;
			}

			for (int i = 0; i < left.Length; i++)
			{
				if (!left[i].Original.Equals(right[i].Original))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			//TODO: what?
			return Original.GetHashCode();
		}
	}
}
