// Maded by Pedro M Marangon
using PedroUtils;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCIJ.Sounds
{
	[System.Serializable]
	public class SFX
	{
		[SerializeField] private SfxClip sfx;
		[SerializeField] private AudioSource source;

		public void Play()
		{
			if (!sfx)
			{
				Debug.LogWarning($"There isn't any SFXClip assigned.");
				return;
			}

			var clip = sfx.Clip;
			if(source == null)
			{
				var go = new GameObject(sfx.name);
				source = go.AddComponent<AudioSource>();
				ObjectID.DontDestroyOnLoad(go);
				ObjectID.Destroy(go, clip.length + 0.1f);
			}

			source.clip = clip;
			source.volume = sfx.Volume;
			source.pitch = sfx.Pitch;
			source.Play();
		}
	}

	[CreateAssetMenu(fileName = "SFX_", menuName = "SFX/SfxClip")]
    public class SfxClip : ScriptableObject
    {
		[System.Serializable]
		public struct Variation
		{
			[Range(0, 1), SerializeField] private float baseValue;
			[Range(0, .5f), SerializeField] private float variation;

			public Variation(float val, float chang)
			{
				baseValue = val;
				variation = chang;
			}

			public float Value => baseValue + Random.Range(-variation, variation);
			
			public static implicit operator float(Variation var) => var.Value;
		}

		[SerializeField] private List<AudioClip> clips;
		[SerializeField] private Variation volume = new Variation(1, 0.5f);
		[SerializeField] private Variation pitch = new Variation(0, 0.5f);

		public float Volume => volume;
		public float Pitch => pitch;
		public AudioClip Clip => GetRandom.Element(clips);



		#region Creating from selected clips
#if UNITY_EDITOR
		[MenuItem("Assets/Create/SFX/SfxClip from selection", priority = 0)]
		static void CreateSFXClipFromSelection()
		{
			List<AudioClip> clips = Selection.GetFiltered(typeof(AudioClip), SelectionMode.Assets).Cast<AudioClip>().ToList();
			clips.OrderBy(x => x.name);

			SfxClip sfxClip = CreateInstance<SfxClip>();
			sfxClip.name = clips[0].name;
			sfxClip.clips = clips;

			Undo.RegisterCreatedObjectUndo(sfxClip, "Create SFX Clip from selection");
			AssetDatabase.CreateAsset(sfxClip, $"Assets/Sounds/Clips/{sfxClip.name}.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();
			EditorGUIUtility.PingObject(sfxClip);
			Selection.activeObject = sfxClip;
		}
		[MenuItem("Assets/Create/New SFXClip from selection", true, priority = 0)]
		static bool ValidadeAudioClipSelection() => Selection.GetFiltered(typeof(AudioClip), SelectionMode.Assets).Length > 0;
#endif
		#endregion



	}
}