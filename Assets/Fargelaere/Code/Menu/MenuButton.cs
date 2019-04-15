using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class MenuButton : MonoBehaviour
{

	
	public UnityEvent GotPressed = new UnityEvent();
	RectTransform rTrans;
	Image sRend;

	public Sprite activeSprite;
	public Sprite unActiveSprite;

	bool _active;
	public bool Active
	{
		get
		{
			return _active;
		}
		set
		{
			sRend.sprite = (value) ? activeSprite : unActiveSprite;
			_active = value;
		}
	}

	private void OnEnable()
	{
		TouchInputController.AddListeners(tapV2: SomethingTapped);
		rTrans = GetComponent<RectTransform>();
		sRend = GetComponent<Image>();
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
