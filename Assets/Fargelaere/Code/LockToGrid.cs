using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class LockToGrid : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            transform.position = Vector3Int.RoundToInt(transform.position);

			Tilemap tmap = transform.GetComponentInParent<Tilemap>();
			if (!tmap)
			{
				tmap = FindObjectOfType<Tilemap>();
				if (tmap)
				{
					transform.parent = tmap.transform;
				}
			}
        }
#endif
    }
}
