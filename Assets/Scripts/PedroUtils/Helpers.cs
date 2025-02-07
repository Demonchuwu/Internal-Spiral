// maded by Pedro M Marangon
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PedroUtils
{
	public static class Helpers
	{
		private static Camera _cam;
		public static Camera Camera
		{
			get
			{
				if (_cam == null) _cam = Camera.main;
				return _cam;
			}
		}

		public static T GetIfNull<T>(ref T component) where T : Component
		{
			if (component == null) return Object.FindObjectOfType<T>();
			return component;
		}
		public static T GetIfNull<T>(ref T component, Component baseObject) where T : Component
		{
			if (component == null) return baseObject.GetComponent<T>();
			return component;
		}

		public static Task WaitForSeconds(float seconds) => Task.Delay((int)(1000 * seconds));

#if UNITY_EDITOR
		public static void DrawGizmoLine(Color color, Vector3 start, Vector3 end, bool additiveEnd)
		{
			Gizmos.color = color;
			Gizmos.DrawLine(start, additiveEnd? (start + end) : end);
		}
		public static void DrawGizmoSphere(Color color, Vector3 position, float radius, bool wireframe)
		{
			Gizmos.color = color;
			if(wireframe) Gizmos.DrawWireSphere(position, radius);
			else Gizmos.DrawSphere(position, radius);
		}
		public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
		{
			if (_color != default(Color))
				Handles.color = _color;
			Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
			using (new Handles.DrawingScope(angleMatrix))
			{
				var pointOffset = (_height - (_radius * 2)) / 2;

				//draw sideways
				Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
				Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
				Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
				Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
				//draw frontways
				Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
				Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
				Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
				Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
				//draw center
				Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
				Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

			}
		}
#endif
	}
}