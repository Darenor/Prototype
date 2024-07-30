using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AiController", menuName = "InputController/AiController")]
public class AiController : InputController
{
    [Header("Interction")]
    [SerializeField] private LayerMask _layerMask = -1;
    [Header("Ray")]
    [SerializeField] private float _bottomDistance = 1f;
    [SerializeField] private float _topDistance = 1f;
    [SerializeField] private float _xOffSet = 1f;
    
    private RaycastHit2D _groundInfoBottom;
    private RaycastHit2D _groundInfoTop;
    
    public override float RetrieveMoveInput(GameObject gameObject)
    {
        _groundInfoBottom = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (_xOffSet * gameObject.transform.localScale.x),
            gameObject.transform.position.y), Vector2.down, _bottomDistance, _layerMask);
        Debug.DrawRay(new Vector2(gameObject.transform.position.x + (_xOffSet * gameObject.transform.localScale.x), gameObject.transform.position.y),
            Vector2.down * _bottomDistance, Color.red);
        
        _groundInfoTop = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (_xOffSet * gameObject.transform.localScale.x),
            gameObject.transform.position.y), Vector2.right * gameObject.transform.localScale.x, _topDistance, _layerMask);
        Debug.DrawRay(new Vector2(gameObject.transform.position.x + (_xOffSet * gameObject.transform.localScale.x), gameObject.transform.position.y),
            Vector2.right * (_topDistance * gameObject.transform.localScale.x), Color.red);

        if (_groundInfoTop.collider == true || _groundInfoBottom.collider == false)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
        }
        
        return gameObject.transform.localScale.x;
    }
    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return false;
    }
    public override bool RetrieveJumpHoldInput(GameObject gameObject)
    {
        return false;
    }
}
