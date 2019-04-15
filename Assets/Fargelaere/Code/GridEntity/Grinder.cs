using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grinder : GridEntity
{
	 public override void OnSlideIntoObject(GridEntity other) {
		PaintBlob i = other as PaintBlob;
		if (other) {
			Destroy(other.gameObject);
		} 
	}
}
