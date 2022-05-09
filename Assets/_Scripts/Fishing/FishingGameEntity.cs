using UnityEngine;

public class FishingGameEntity : Entity
{
	protected SpriteRenderer _sr;
	private void Start() => _onStart();

	protected virtual void _onStart() => _sr = GetComponent<SpriteRenderer>();

	public void ClampPosition(float clampTo)
	{
		var localPos = transform.localPosition;
		localPos.x = Mathf.Clamp(localPos.x, -clampTo, clampTo);
		localPos.y = Mathf.Clamp(localPos.y, -clampTo, clampTo);
		transform.localPosition = localPos;
	}
}
