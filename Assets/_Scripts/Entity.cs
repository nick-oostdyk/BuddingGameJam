using UnityEngine;

public class Entity : MonoBehaviour
{
	private Rigidbody2D _rigidbody;
	private float _maxSpeed = 10f;
	private float _acceleration = 100f;

	private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();

	// called every delta frame
	public void DoMove(Vector2 direction)
	{
		direction.Normalize();

		// apply delta velocity and clamp to max speed
		_rigidbody.velocity += direction * _acceleration * Time.deltaTime;
		_rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, _maxSpeed);

		// apply damping force if input is 0 or velocity is opposed
		var d = Vector2.Dot(_rigidbody.velocity, direction);
		if (direction == Vector2.zero || d < 0.1f)
			_rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, .15f);
	}

	public void StopAllMovementImmediate() => _rigidbody.velocity = Vector3.zero;
	public void SetPosition(Vector3 position) => transform.position = position;
}