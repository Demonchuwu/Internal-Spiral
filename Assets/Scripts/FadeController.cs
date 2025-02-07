// maded by Pedro M Marangon
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CCIJ
{
	[RequireComponent(typeof(CanvasGroup))]
	public class FadeController : MonoBehaviour
    {
		[SerializeField] private bool fadeAtStart = true;
		[SerializeField] private bool delayFade = false;
		[SerializeField] private float delayToFade = 1f;
		[SerializeField] private float startingValue = 1f;
		[SerializeField] private float timeToFade = 0f;
		[SerializeField] private float durationToFade = 1f;
		[SerializeField] private bool destroyAfterFade = false;
		private Image image;
		private CanvasGroup _group;

		private static FadeController fadeCanvas = null;
		private bool isFading;
		private Action OnFadeEnd;
		private float targetAlpha;
		private bool fadingOut;

		public static FadeController FadeCanvas
		{
			get
			{
				if (!fadeCanvas) fadeCanvas = GameObject.Find("FadeCanvas").transform.GetChild(0).GetComponent<FadeController>();
				return fadeCanvas;
			}
		}

		private void Awake()
		{
			image = GetComponent<Image>();
			_group = GetComponent<CanvasGroup>();
		}

		private void Start()
		{
			if (!fadeAtStart) return;

			if (delayFade)
			{
				Invoke(nameof(FadeAtStart), delayToFade);
				return;
			}
			FadeAtStart();
		}

		private void FadeAtStart()
		{
			_group.alpha = startingValue;
			_group.blocksRaycasts = _group.interactable = false;
			WaitToFade();
		}

		public void SetDuration(float newDur) => durationToFade = newDur;

		private async void WaitToFade()
		{
			await Task.Delay((int)(1000 * timeToFade));
			Fade(0);
		}

		private void Update()
		{
			if (!isFading) return;

			_group.alpha = Mathf.Lerp(_group.alpha, targetAlpha, Time.unscaledDeltaTime * durationToFade);

			bool isFadeFinished = fadingOut ? (_group.alpha >= targetAlpha-0.01f) : (_group.alpha <= targetAlpha + 0.01f);

			if(isFadeFinished)
			{
				isFading = false;
				OnFadeEnd?.Invoke();
				OnFadeEnd = null;
			}
		}

		public void FadeOut(Action onEnd, Color colorToFade)
		{
			fadingOut = true;
			Fade(1, onEnd, colorToFade);
		}

		public void Fade(float value)
		{
			targetAlpha = value;
			isFading = true;
			OnFadeEnd = () => { if (destroyAfterFade) Destroy(gameObject); };
		}

		public void Fade(float value, Action onEnd, Color colorToFade)
		{
			targetAlpha = value;
			isFading = true;
			image.color = colorToFade;
			OnFadeEnd = onEnd;
		}
	}
}
