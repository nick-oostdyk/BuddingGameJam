using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneChangeHandler : MonoBehaviour
{
	public static void LoadScene(int buildIndex) => SceneManager.LoadScene(buildIndex);
	public static void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
	public static async void LoadSceneDelay(int buildIndex, int delayMS = 250)
	{
		await Task.Delay(delayMS);
		LoadScene(buildIndex);
	}

	public static async void LoadSceneDelay(string sceneName, int delayMS = 250)
	{
		await Task.Delay(delayMS);
		LoadScene(sceneName);
	}

	public void Load(int buildIndex) => SceneManager.LoadScene(buildIndex);
	public void Load(string sceneName) => SceneManager.LoadScene(sceneName);
	public async void LoadDelay(int buildIndex, int delayMS = 250)
	{
		await Task.Delay(delayMS);
		LoadScene(buildIndex);
	}

	public async void LoadDelay(string sceneName, int delayMS = 250)
	{
		await Task.Delay(delayMS);
		LoadScene(sceneName);
	}

	public void Quit()
    {
		Application.Quit();
    }
}
