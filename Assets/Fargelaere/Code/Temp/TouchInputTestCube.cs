using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputTestCube : MonoBehaviour
{
	Color cOne = Color.red;
	bool isOne;

	Color cTwo = Color.blue;

	private void OnEnable()
	{
		TouchInputController.AddListeners(Move, Tap, TapObject);
	}

	private void OnDisable()
	{
		TouchInputController.RemoveListeners(Move, Tap, TapObject);
	}

	private void Move( Direction4 dir)
	{
		transform.position += (Vector3)dir.ToVector2();
	}

	private void Tap(Vector2 vec)
	{
		//Debug.Log("Tapped" + vec);
	}

	private void TapObject(GameObject obj)
	{
		if(obj == gameObject)
		{
			isOne = !isOne;

			GetComponent<MeshRenderer>().material.color = isOne ? cOne : cTwo;
		}
	}
}
