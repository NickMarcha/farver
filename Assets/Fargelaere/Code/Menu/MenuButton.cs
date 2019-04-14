using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class MenuButton : MonoBehaviour
{
	public UnityEvent GotPressed = new UnityEvent();
	RectTransform rTrans;

	private void OnEnable()
	{
		TouchInputController.AddListeners(tapV2: SomethingTapped);
		rTrans = GetComponent<RectTransform>();
	}

	private void OnDisable()
	{
		TouchInputController.RemoveListeners(tapV2: SomethingTapped);
	}

	public void SomethingTapped(Vector2 tapPos)
	{
		if (RectTransformUtility.RectangleContainsScreenPoint(rTrans,tapPos))
		{
			
			GotPressed?.Invoke();
		}
	}
}
