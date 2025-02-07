// maded by Pedro M Marangon
using PedroUtils;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CCIJ
{
	public class SceneTransitions : MonoBehaviour
	{
		[SerializeField] private int menuScene;
		[SerializeField] private int gameplayScene;
		[SerializeField] private FadeController fadeScreen;

		private void Awake()
		{
			Time.timeScale = 1;
		}

		public void ReloadScene() => fadeScreen.FadeOut(() => SceneManager.LoadScene(gameplayScene), Color.black);
		public void GoToMenu() => fadeScreen.FadeOut(() => SceneManager.LoadScene(menuScene), Color.white);
	}
}
