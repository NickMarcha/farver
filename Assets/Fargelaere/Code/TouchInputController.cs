using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchInputController : MonoBehaviour
{
	public static TouchInputController Instance { get; private set; }

	/// <summary>
	/// Event for swipes, return swipe
	/// </summary>
	public Direction4Event SwipeD4 = new Direction4Event();

	/// <summary>
	/// Event for Taps, returns pixel position
	/// </summary>
	public Vector2Event TapScreenV2 = new Vector2Event();

	/// <summary>
	/// Event for Taps, returns raycast Object
	/// </summary>
	public GameObjectEvent TapScreenObject = new GameObjectEvent();

	/// <summary>
	/// Enable debug for swipes with arrow keys.
	/// </summary>
	[Header("Debug swipes with arrowkeys")]
	public bool debugSwipes = false;

	/// <summary>
	/// Enable debug for taps with MouseClicks.
	/// </summary>
	[Header("Debug taps with mouse left click")]
	public bool debugTaps = false;

	Camera mainCam;

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
		else
		{
			Debug.LogWarning("There is more than one TouchInputController in scene");
			enabled = false;
			return;
		}


		mainCam = Camera.main;
	}


	private Vector3 fp;   //First touch position
	private Vector3 lp;   //Last touch position
	private float dragDistance;  //minimum distance for a swipe to be registered

	void Start()
	{
		dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
	}

	void Update()
	{
		#region Debug Swipes & taps
		if (debugSwipes)
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				SwipeD4?.Invoke(Direction4.Right);
			}

			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				SwipeD4?.Invoke(Direction4.Left);
			}

			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				SwipeD4?.Invoke(Direction4.Down);
			}

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				SwipeD4?.Invoke(Direction4.Up);
			}

		}
		if (debugTaps)
		{
			if (Input.GetMouseButtonDown(0))
			{
				TapScreenV2?.Invoke(Input.mousePosition);

				RaycastHit hit;
				Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit))
				{
					Transform objectHit = hit.transform;

					TapScreenObject.Invoke(objectHit.gameObject);
				}
			}
		}
		#endregion

		#region Swipe & Tap
		if (Input.touchCount == 1) // user is touching the screen with a single touch
		{
			Touch touch = Input.GetTouch(0); // get the touch
			if (touch.phase == TouchPhase.Began) //check for the first touch
			{
				fp = touch.position;
				lp = touch.position;
			}
			else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
			{
				lp = touch.position;
			}
			else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
			{
				lp = touch.position;  //last touch position. Ommitted if you use list

				//Check if drag distance is greater than 20% of the screen height
				if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
				{//It's a drag
				 //check if the drag is vertical or horizontal
					if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
					{   //If the horizontal movement is greater than the vertical movement...
						if ((lp.x > fp.x))  //If the movement was to the right)
						{   //Right swipe
							SwipeD4?.Invoke(Direction4.Right);
						}
						else
						{   //Left swipe
							SwipeD4?.Invoke(Direction4.Left);
						}
					}
					else
					{   //the vertical movement is greater than the horizontal movement
						if (lp.y > fp.y)  //If the movement was up
						{   //Up swipe
							SwipeD4?.Invoke(Direction4.Up);
						}
						else
						{   //Down swipe
							SwipeD4?.Invoke(Direction4.Down);
						}
					}
				}
				else
				{   //It's a tap as the drag distance is less than 20% of the screen height
					TapScreenV2?.Invoke(touch.position);

					RaycastHit hit;
					Ray ray = mainCam.ScreenPointToRay(touch.position);

					if (Physics.Raycast(ray, out hit))
					{
						Transform objectHit = hit.transform;

						TapScreenObject.Invoke(objectHit.gameObject);
					}
				}
			}
		}

		#endregion
	}


	#region custom Unity events

	[System.Serializable]
	public class Direction4Event : UnityEvent<Direction4>
	{

	}

	[System.Serializable]
	public class Vector2Event : UnityEvent<Vector2>
	{

	}

	[System.Serializable]
	public class GameObjectEvent : UnityEvent<GameObject>
	{

	}


	#endregion
}


