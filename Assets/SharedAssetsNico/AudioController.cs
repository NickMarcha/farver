using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	public AudioSource ThumpHigh;
	public AudioSource ThumpLow;

	private static AudioController instance;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
		}
		else
		{
			Debug.LogWarning("There is more than one "+ GetType().Name + " in scene");
			enabled = false;
			return;
		}
	}

	public static void PlayThumpHigh()
	{
		if (!instance)
		{
			Debug.Log("No "+ instance.GetType().Name + " in scene");
			return;
		}

		instance.ThumpHigh.Play();
	}

	public static void PlayThumpLow()
	{
		if (!instance)
		{
			Debug.Log("No " + instance.GetType().Name + " in scene");
			return;
		}

		instance.ThumpLow.Play();
	}

}
