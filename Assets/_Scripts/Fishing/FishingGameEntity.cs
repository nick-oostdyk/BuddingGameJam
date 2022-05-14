using UnityEngine;

// subset of entity class that adds function to clamp local position
public class FishingGameEntity : Entity
{
	protected SpriteRenderer _sr;
	private void Start() => _onStart();

	protected virtual void _onStart() => _sr = GetComponent<SpriteRenderer>();

	// clamps x and y position to -clampTo, clampTo
	public void ClampPosition(float clampTo)
	{
		var localPos = transform.localPosition;
		localPos.x = Mathf.Clamp(localPos.x, -clampTo, clampTo);
		localPos.y = Mathf.Clamp(localPos.y, -clampTo, clampTo);
		transform.localPosition = localPos;
	}
}
