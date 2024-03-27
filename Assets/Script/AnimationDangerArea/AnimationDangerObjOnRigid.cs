using System.Collections;
using UnityEngine;

public class AnimationDangerObjOnRigid:AnimaionDangerObj
{
    public override void OnAnimation()
    {
        GetComponent<Rigidbody2D>().simulated = true;
    }
}
