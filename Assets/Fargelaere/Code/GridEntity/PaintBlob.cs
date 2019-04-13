using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
/// <summary>
/// Represents a paint blob that can combine with other paintblobs
/// </summary>
public class PaintBlob : Pushable
{
    /// <summary>
    /// The PaintColor of the PaintBlob
    /// </summary>
    public PaintColor Color;

    /// <summary>
    /// All the combinations that can be applied to this PaintBlob
    /// </summary>
    public ColorCombination[] PossibleCombinations;

    /// <summary>
    /// Merges this blob with another blob, combining their colors.
    /// </summary>
    /// <param name="blob"></param>
    public void MergeWith(PaintBlob blob)
    {
        foreach (ColorCombination i in PossibleCombinations)
        {
            if (i.Matches(Color, blob.Color))
            {
                Destroy(blob.gameObject);
                i.Result.Apply(this);
            }
        }
    }

    public override void OnSlideIntoObject(GridEntity other)
    {
        //GetInstanceID() > other.getInstanceID() makes sure the merge only happens for one of the blobs and not both.
        if (other is PaintBlob && GetInstanceID() > other.GetInstanceID())
        {
            MergeWith(other as PaintBlob);
        }
    }

    
    private void Update()
    {
        Color.Apply(this);
    }

    private void OnEnable()
    {
        TouchInputController.AddListeners(swipe: Push);
    }

    private void OnDisable()
    {
        TouchInputController.RemoveListeners(swipe: Push);
    }
}
