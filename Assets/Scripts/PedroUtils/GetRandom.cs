// maded by Pedro M Marangon
using System.Collections.Generic;
using UnityEngine;

namespace PedroUtils
{
	public class GetRandom
	{
		public static TObject Element<TObject>(List<TObject> obj)
		{
			if (obj.Count <= 0) return default(TObject);
			if (obj.Count == 1) return obj[0];
			int position = Random.Range(0, obj.Count);
			return obj[position];
		}

		public static bool Boolean => Random.value > 0.5f;

		public static float ValueInRange(Vector2 range) => Random.Range(range.x, range.y);

		public static Vector2 Position(Vector2 size, Vector2 offset)
		{
			float x = Random.Range(-size.x / 2, size.x / 2) + offset.x;
			float y = Random.Range(-size.y / 2, size.y / 2) + offset.y;

			return new Vector2(x, y);
		}
	}
}