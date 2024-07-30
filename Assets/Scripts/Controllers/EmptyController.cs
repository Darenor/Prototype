using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmptyController", menuName = "InputController/EmptyController")]
public class EmptyController : InputController
{
    public override float RetrieveMoveInput(GameObject gameObject)
    {
        return 0f;
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
