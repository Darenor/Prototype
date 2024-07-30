using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
public class WallStaff : MonoBehaviour
{
    public bool WallJumping { get; private set; }
    
    [Header("Wall Slide")]
    [SerializeField, Range(0.1f, 5f)] private float _wallSlideMaxSpeed = 2f;
    [Header("Wall Jump")]
    [SerializeField] private Vector2 _wallJumpingClimb = new Vector2(4f, 12f);
    [SerializeField] private Vector2 _wallJumpingBounce = new Vector2(10.7f, 10f);
    [SerializeField] private Vector2 _wallJumpLeap = new Vector2(14f, 12f);
    [Header("Wall Stick")]
    [SerializeField, Range(0f, 0.5f)] private float _wallStickTime = 0.25f;


    private CollisionDataRetriever _collisionDataRetriever;
    private Rigidbody2D _rigidbody2D;
    private UnitController _controller;

    private Vector2 _velocity;
    private bool _onWall, _onGround, _desiredJump;
    private float _wallDirectionX, _wallStickCounter;
    
    void Start()
    {
        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _controller = GetComponent<UnitController>();
    }

    void Update()
    {
        if (_onWall && !_onGround)
        {
            _desiredJump |= _controller.Input.RetrieveJumpInput(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _velocity = _rigidbody2D.velocity;
        _onWall = _collisionDataRetriever.OnWall;
        _onGround = _collisionDataRetriever.OnGround;
        _wallDirectionX = _collisionDataRetriever.ContactNormal.x;

        if (_onWall)
        {
            if (_velocity.y < -_wallSlideMaxSpeed)
            {
                _velocity.y = -_wallSlideMaxSpeed;
            }
        }
        
        if(_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && !WallJumping)
        {
            if (_wallStickCounter > 0)
            {
                _velocity.x = 0;

                if (_controller.Input.RetrieveMoveInput(gameObject) == _collisionDataRetriever.ContactNormal.x)
                {
                    _wallStickCounter -= Time.deltaTime;
                }
                else
                {
                    _wallStickCounter = _wallStickTime;
                }
            }
            else
            {
                _wallStickCounter = _wallStickTime;
            }
        }

        if ((_onWall && _velocity.x == 0) || _onGround)
            WallJumping = false;

        if (_desiredJump)
        {
            if (-_wallDirectionX == _controller.Input.RetrieveMoveInput(gameObject))
            {
                _velocity = new Vector2(_wallJumpingClimb.x * _wallDirectionX, _wallJumpingClimb.y);
                WallJumping = true;
                _desiredJump = false;
            }
            else if (_controller.Input.RetrieveMoveInput(gameObject) == 0)
            {
                _velocity = new Vector2(_wallJumpingBounce.x * _wallDirectionX, _wallJumpingBounce.y);
                WallJumping = true;
                _desiredJump = false;
            }
            else
            {
                _velocity = new Vector2(_wallJumpLeap.x * _wallDirectionX, _wallJumpLeap.y);
                WallJumping = true;
                _desiredJump = false;
            }
        }

        _rigidbody2D.velocity = _velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        _collisionDataRetriever.EvaluateCollision(collision2D);

        if (_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && WallJumping)
            _rigidbody2D.velocity = Vector2.zero;
    }
}
