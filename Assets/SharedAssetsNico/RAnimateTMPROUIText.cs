using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI), typeof(RectTransform))]
public class RAnimateTMPROUIText : MonoBehaviour
{

	public float xSpeed = 2;
	public float ySpeed = 1;

	public bool ScaleWidth;
	public bool ScaleHeight;
	public bool Rotate;
	public bool Color;

	private void Update()
	{
		Vector3 origScale = GetComponent<RectTransform>().localScale;

		float sample = Mathf.PerlinNoise(Time.time * xSpeed, Time.time * ySpeed);
		float sample2 = Mathf.PerlinNoise(Time.time * ySpeed, Time.time * xSpeed);

		float sample3 = Mathf.PerlinNoise(Time.time * xSpeed, -Time.time * ySpeed);


		GetComponent<RectTransform>().localScale = new Vector3(ScaleWidth ? sample * 3 : origScale.x, ScaleHeight ? sample2 * 8 : origScale.y, 1);

		if (Rotate)
		{
			Quaternion rot = GetComponent<RectTransform>().rotation;

			rot.eulerAngles = new Vector3(0, 0, ((sample3 - 0.5f) * 180));

			GetComponent<RectTransform>().rotation = rot;
		}

		if (Color)
		{
			GetComponent<TextMeshProUGUI>().color = new Color(sample, sample2, sample3);
		}
	}

}

