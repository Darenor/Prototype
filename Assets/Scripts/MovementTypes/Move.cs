using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(UnitController), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

    private UnitController _controller;
    private Vector2 _direction, _desiredVelocity, _velocity;
    private Rigidbody2D _rigidbody2D;
    private CollisionDataRetriever _collisionDataRetriever;
    private WallStaff _wallStaff;

    private float _maxSpeedChange, _acceleration;
    private bool _onGround;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        _controller = GetComponent<UnitController>();
        _wallStaff = GetComponent<WallStaff>();
    }

    private void Update()
    {
        _direction.x = _controller.Input.RetrieveMoveInput(gameObject);
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _collisionDataRetriever.Friction, 0f);
    }

    private void FixedUpdate()
    {
        _onGround = _collisionDataRetriever.OnGround;
        _velocity = _rigidbody2D.velocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        _rigidbody2D.velocity = _velocity;
    }
}
