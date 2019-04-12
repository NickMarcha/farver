using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputTestCube : MonoBehaviour
{
	private TouchInputController contrl;

	Color COne = Color.red;
	bool isOne;

	Color CTwo = Color.blue;

	private void Start()
	{
		contrl = TouchInputController.Instance;

		contrl.SwipeD4.AddListener(Move);

		contrl.TapScreenV2.AddListener(Tap);

		contrl.TapScreenObject.AddListener(TapObject);
	}

	private void Move( Direction4 dir)
	{
		transform.position += (Vector3)dir.ToVector2();
	}

	private void Tap(Vector2 vec)
	{
		Debug.Log("Tapped" + vec);
	}

	private void TapObject(GameObject obj)
	{
		if(obj == gameObject)
		{
			isOne = !isOne;

			GetComponent<MeshRenderer>().material.color = isOne ? COne : CTwo;
		}
	}
}
