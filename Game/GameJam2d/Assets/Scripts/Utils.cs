using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debaka.Utils

{
	/// <summary>
	/// This class contains several useful utility functions
	/// </summary>
	public static class UtilsClass
	{
		public const int sortingOrderDefault = 0;

		public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), Vector3 lookDir = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
		{
			if (color == null) color = Color.white;
			if(lookDir == null) lookDir = Vector3.forward;
			return CreateWorldText(parent, text, localPosition, lookDir, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
		}

		// Create Text in the World
		public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, Vector3 lookDir, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
		{
			GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
			Transform transform = gameObject.transform;
			transform.SetParent(parent, false);
			transform.localPosition = localPosition;
			transform.forward = lookDir;
			TextMesh textMesh = gameObject.GetComponent<TextMesh>();
			textMesh.anchor = textAnchor;
			textMesh.alignment = textAlignment;
			textMesh.text = text;
			textMesh.fontSize = fontSize;
			textMesh.color = color;
			textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
			return textMesh;
		}

		// Get Mouse Position in World with Z = 0f
		public static Vector3 GetMouseWorldPosition()
		{
			Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
			vec.z = 0f;
			return vec;
		}
		public static Vector3 GetMouseWorldPositionWithZ()
		{
			return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
		}
		public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
		{
			return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
		}
		public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
		{
			Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
			return worldPosition;
		}

		/// <summary>
		/// Returns a random, normally distributed float value between a minimum and maximum (0.0 and 1.0 default)
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
		{
			float u, v, S;

			do
			{
				u = 2.0f * UnityEngine.Random.value - 1.0f;
				v = 2.0f * UnityEngine.Random.value - 1.0f;
				S = u * u + v * v;
			}
			while (S >= 1.0f);

			// Standard Normal Distribution
			float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

			// Normal Distribution centered between the min and max value
			// and clamped following the "three-sigma rule"
			float mean = (minValue + maxValue) / 2.0f;
			float sigma = (maxValue - mean) / 3.0f;
			return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
		}


	}
}

