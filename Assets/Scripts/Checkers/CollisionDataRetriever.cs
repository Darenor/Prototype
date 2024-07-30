using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CollisionDataRetriever : MonoBehaviour
{
    public bool OnGround { get; private set; }
    public bool OnWall { get; private set; }
    public float Friction { get; private set; }

    public Vector2 ContactNormal { get; private set; }
    private PhysicsMaterial2D _material2D;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        EvaluateCollision(collision2D);
        RetrieveFriction(collision2D);
    }

    private void OnCollisionStay2D(Collision2D collision2D)
    {
        EvaluateCollision(collision2D);
        RetrieveFriction(collision2D);
    }

    private void OnCollisionExit2D(Collision2D collision2D)
    {
        OnGround = false;
        Friction = 0;
        OnWall = false;
    }

    public void EvaluateCollision(Collision2D collision2D)
    {
        for (int i = 0; i < collision2D.contactCount; i++)
        {
            ContactNormal = collision2D.GetContact(i).normal;
            OnGround |= ContactNormal.y >= 0.9f;
            OnWall = Mathf.Abs(ContactNormal.x) >= 0.9f;
        }
    }

    private void RetrieveFriction(Collision2D collision2D)
    {
        _material2D = collision2D.rigidbody.sharedMaterial;

        Friction = 0;

        if (_material2D != null)
        {
            Friction = _material2D.friction;
        }
    }
}
