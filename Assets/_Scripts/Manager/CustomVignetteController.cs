using UnityEngine;
using UnityEngine.Rendering;
using System.Threading.Tasks;

public class CustomVignetteController : MonoBehaviour
{
	[SerializeField] private Volume _vignetteVolume;

	private int perFrameMS = 6;

	public void SetWeight(float weight) => _vignetteVolume.weight = weight;
	public async Task FadeFromBlack(float duration)
	{
		var (iters, delta) = _getIterationsAndDelta(duration);
		for (int i = 0; i < iters; i += 1)
		{
			_vignetteVolume.weight = 1 - i * delta;
			await Task.Delay(perFrameMS);
		}
	}

	public async Task FadeToBlack(float duration)
	{
		var (iters, delta) = _getIterationsAndDelta(duration);
		for (int i = 0; i < iters; i += 1)
		{
			_vignetteVolume.weight = i * delta;
			await Task.Delay(perFrameMS);
		}
	}

	private (float, float) _getIterationsAndDelta(float duration)
	{
		var dtSeconds = perFrameMS * 0.001f;
		var iterations = duration / dtSeconds;
		var delta = 1 / iterations;
		return (iterations, delta);
	}
}