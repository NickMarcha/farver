using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
public class LockToGrid : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            transform.position = Vector3Int.RoundToInt(transform.position);
        }
#endif
    }
}
