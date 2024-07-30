using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(UnitController), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int _maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.5f;
    [SerializeField, Range(0f, 0.3f)] private float _ledgeTimerForJump = 0.2f;
    [SerializeField, Range(0f, 0.3f)] private float _jumpBufferTimer = 0.2f;

    private Rigidbody2D _rigidBody2D;
    private CollisionDataRetriever _collisionDataRetriever;
    private UnitController _controller;
    private Vector2 _velocity;

    private int _jumpCount;
    private float _defaultGravityScale, _jumpSpeed, _ledgeCounterForJump, _jumpBufferCounter;

    private bool _desiredJump, _onGround, _isJumping;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        _controller = GetComponent<UnitController>();

        _defaultGravityScale = 1f;
    }

    private void Update()
    {
        _desiredJump |= _controller.Input.RetrieveJumpInput(gameObject);
    }

    private void FixedUpdate()
    {
        _onGround = _collisionDataRetriever.OnGround;
        _velocity = _rigidBody2D.velocity;

        if (_onGround && _rigidBody2D.velocity.y == 0)
        {
            _jumpCount = 0;
            _ledgeCounterForJump = _ledgeTimerForJump;
            _isJumping = false;
        }
        else
        {
            _ledgeCounterForJump -= Time.deltaTime;
        }

        if (_desiredJump)
        {
            _desiredJump = false;
            _jumpBufferCounter = _jumpBufferTimer;
        }
        else if(!_desiredJump && _jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0)
        {
            JumpAction();
        }
        
        if (_controller.Input.RetrieveJumpHoldInput(gameObject) && _rigidBody2D.velocity.y > 0)
        {
            _rigidBody2D.gravityScale = _upwardMovementMultiplier;
        }
        else if (!_controller.Input.RetrieveJumpHoldInput(gameObject) || _rigidBody2D.velocity.y < 0)
        {
            _rigidBody2D.gravityScale = _downwardMovementMultiplier;
        }
        else if(_rigidBody2D.velocity.y == 0)
        {
            _rigidBody2D.gravityScale = _defaultGravityScale;
        }

        _rigidBody2D.velocity = _velocity;
    }

    private void JumpAction()
    {
        if (_ledgeCounterForJump > 0f || (_jumpCount < _maxAirJumps && _isJumping))
        {
            if(_isJumping)
                _jumpCount += 1;

            _jumpBufferCounter = 0;
            _ledgeCounterForJump += 0;
            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight * _upwardMovementMultiplier);
            _isJumping = true;
            
            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_rigidBody2D.velocity.y);
            }
            _velocity.y += _jumpSpeed;
        }
    }
}

