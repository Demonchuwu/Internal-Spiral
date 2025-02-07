// maded by Pedro M Marangon
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PedroUtils
{
	public static class Extensions
	{
		//---------GENERICS-----------//
		public static T GetOrAddComponent<T>(this GameObject gObj) where T : Component
		{
			var comp = gObj.GetComponent<T>();
			if (comp == null) comp = gObj.AddComponent<T>();
			return comp;
		}
		public static T[] RemoveAll<T>(this T[] array, Predicate<T> match)
		{
			List<T> list = array.ToList();
			list.RemoveAll(match);
			return list.ToArray();
		}

		//---------COMPONENT-----------//
		public static void Suicide(this Component gObj, float time = 0) => Object.Destroy(gObj.gameObject, time);
		public static List<Transform> GetParents(this Component[] comps)
		{
			List<Transform> list = new List<Transform>();

			foreach (var comp in comps)
				list.Add(comp.transform.parent);

			return list;
		}
		public static List<Transform> GetParents(this List<Component> comps)
		{
			List<Transform> list = new List<Transform>();

			foreach (var comp in comps)
				list.Add(comp.transform.parent);

			return list;
		}

		//---------FLOAT-----------//
		public static float With2Decimals(this float v) => Mathf.Round(v * 100f) / 100f;
		public static float With3Decimals(this float v) => Mathf.Round(v * 1000f) / 1000f;
		public static float ClampAngle(this float angle, float min, float max)
		{
			if (angle < -360f) angle += 360f;
			if (angle > 360f) angle -= 360f;
			return Mathf.Clamp(angle, min, max);
		}
		public static float Clamp01WithAction(float value, Action<float> action)
        {
			var val = Mathf.Clamp01(value);
			action?.Invoke(val);
			return val;
		}
		
		//---------VECTOR3-----------//
		public static Vector3 With2Decimals(this Vector3 v) => new Vector3(v.x.With2Decimals(), v.y.With2Decimals(), v.z.With2Decimals());
		public static Vector3 With3Decimals(this Vector3 v) => new Vector3(v.x.With3Decimals(), v.y.With3Decimals(), v.z.With3Decimals());

		//---------VECTOR2-----------//
		public static Vector2Int RoundToInt(this Vector2 val)
		{
			int x = Mathf.RoundToInt(val.x);
			int y = Mathf.RoundToInt(val.y);
			return new Vector2Int(x, y);
		}

		public static Vector2 ToVector2(this Vector2Int val) => new Vector2(val.x, val.y);

		//---------ANIMATOR-----------//
		public static bool IsPlayingAnimation(this Animator anim, string id, int layerIndex = 0) => anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(id);

		//---------GAMEOBJECT-----------//
		public static bool IsInsideLayerMask(this GameObject go, LayerMask layerMask) => (layerMask.value & (1 << go.layer)) > 0;
		public static bool IsInLayer(this GameObject go, string name) => (LayerMask.NameToLayer(name) & (1 << go.layer)) > 0;
		public static bool IsInSameLayerAsMe(this GameObject go, GameObject obj) => (1 << go.layer & (1 << obj.layer)) > 0;
		public static void Activate(this GameObject go) => go.SetActive(true);
		public static void Deactivate(this GameObject go) => go.SetActive(false);
		public static void Toggle(this GameObject go) => go.SetActive(!go.activeSelf);

		//---------CANVAS GROUP-----------//
		public static void SetValues(this CanvasGroup group, float alpha, bool combinedValue)
		{
			group.alpha = alpha;
			group.interactable = group.blocksRaycasts = combinedValue;
		}

		//---------STRING-----------//
		public static string Color(this string myStr, string color) => $"<color={color}>{myStr}</color>";
		public static string Color(this string myStr, Color color) => myStr.Color($"#{ColorUtility.ToHtmlStringRGBA(color)}");
		public static string ToSize(this string myStr, int size) => $"<size={size}>{myStr}</size>";
		public static string Bold(this string myStr) => $"<b>{myStr}</b>";
	}
}