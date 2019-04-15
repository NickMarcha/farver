using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grinder : GridEntity
{
	 public override void OnSlideIntoObject(GridEntity other) {
		if (other as PaintBlob) {
			Destroy(other.gameObject);
		} 
	 }

    public override bool CanPass(GridEntity other, Direction4 incommingDirection)
    {
        return other is PaintBlob;
    }
}
