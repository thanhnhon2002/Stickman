using UnityEngine;

public class AnimationBullet : AnimationDangerObjOnRigid
{
    public Vector2 direction;
    public float force;
    public override void OnAnimation()
    {
        base.OnAnimation();
        GetComponent<Rigidbody2D>().AddForce(direction*force,ForceMode2D.Impulse);
    }
}
