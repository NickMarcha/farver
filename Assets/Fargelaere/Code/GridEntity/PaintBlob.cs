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
	/// The color combination to use if the two colors don't combine
	/// </summary>
	public PaintColor Fallback;

	/// <summary>
	/// Merges this blob with another blob, combining their colors.
	/// </summary>
	/// <param name="blob"></param>
	public void MergeWith(PaintBlob blob)
	{
		PaintColor result = Fallback;


		//The colors were identical. No merge is required
		if (blob.Color == Color)
		{
			result = Color;
		}
		else
		{
			//Attempts to find a matching color combination
			foreach (ColorCombination i in PossibleCombinations)
			{
				if (i.Matches(Color, blob.Color))
				{
					result = i.Result;
				}
			}
		}

		Destroy(blob.gameObject);
		result.Apply(this);
		AudioController.PlayThumpHigh();

        //Patches goals not recognizing blobs if they merge while ontop of it
        GetGridEntities(TileMap)
            .Where(i => i.TilePosition == TilePosition && !(i is PaintBlob)) //Please don't remove the second part of this and operation here or Unity will be very mad >:( (Stack Overflow Exception)
            .ForEach(i => { i.OnSlideIntoObject(this); OnSlideIntoObject(i); });


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
        if (Color == null)
        {
            return;
        }

		Color.Apply(this);
	}

	public override bool Equals(GridEntity other)
	{
		return Color == (other as PaintBlob)?.Color && transform.position == other.transform.position;
	}
}
