using UnityEngine;

/// <summary>
/// Allows a prefab to contain nested prefabs
/// </summary>
public abstract class NestedPrefabProviderBase : MonoBehaviour
{
    /// <summary>
    /// Gets or sets the prefab to nest. This is the prefab that will be added to this GameObject's children on start
    /// </summary>
    public abstract GameObject NestedPrefab { get; }

    /// <summary>
    /// Gets or sets the local position of the prefab to nest
    /// </summary>
    public abstract Vector3 NestedLocalPosition { get; }

    /// <summary>
    /// Gets or sets the local rotation of the prefab to nest
    /// </summary>
    public abstract Vector3 NestedLocalRotation { get; }

    protected virtual void Start()
    {
        OnPrefabCreated(Instantiate(NestedPrefab, NestedLocalPosition, Quaternion.Euler(NestedLocalRotation), transform));
        
    }

    /// <summary>
    /// Called when the target prefab has been instantiated
    /// </summary>
    /// <param name="prefab">Instantiated prefab</param>
    protected virtual void OnPrefabCreated(GameObject prefab)
    {

    }
}