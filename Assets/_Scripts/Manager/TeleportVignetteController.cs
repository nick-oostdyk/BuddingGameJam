using UnityEngine;
using UnityEngine.Rendering;
using System.Threading.Tasks;

public class TeleportVignetteController : MonoBehaviour
{
	[SerializeField] private Volume _vignetteVolume;

	public async Task FadeIn()
	{
		for (float i = _vignetteVolume.weight; i > 0; i -= 0.01f)
		{
			_vignetteVolume.weight = i;
			await Task.Delay(8);
		}
	}
	public async Task FadeOut()
	{
		for (float i = _vignetteVolume.weight; i < 1; i += 0.01f)
		{
			_vignetteVolume.weight = i;
			await Task.Delay(8);
		}
	}
}