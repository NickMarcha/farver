using UnityEngine;

/// <summary>
/// A variant of the NestedPrefabProviderBase class that allows the user to set settings
/// </summary>
public class NestedPrefabProvider : NestedPrefabProviderBase
{
    [Header("Prefab")]
    [Tooltip("The prefab to nest. This is the prefab that will be added to this GameObject's children on start")]
    public GameObject Prefab;

    [Header("Transform")]
    [Tooltip("The local position of the prefab to nest")]
    public Vector3 Position;

    [Tooltip("The local rotation of the prefab to nest")]
    public Vector3 Rotation;

    public override GameObject NestedPrefab
    {
        get
        {
            return Prefab;
        }
    }

    public override Vector3 NestedLocalRotation
    {
        get
        {
            return Rotation;
        }
    }


    public override Vector3 NestedLocalPosition
    {
        get
        {
            return Position;
        }
    }
}