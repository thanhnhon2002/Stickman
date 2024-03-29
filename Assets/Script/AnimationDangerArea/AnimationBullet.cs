using UnityEngine;

public class AnimationBullet : AnimationDangerObjOnRigid
{
    public Vector2 direction;
    public float force;
    private void Update()
    {
        Stickman stickman = GameManager.instance.GetComponentInChildren<Stickman>();
        if (stickman!=null&& Vector3.Distance(stickman.transform.position, transform.position) <= 1) stickman.Lose();
    }
    public override void OnAnimation()
    {
        base.OnAnimation();
        GetComponent<Rigidbody2D>().AddForce(direction*force,ForceMode2D.Impulse);
    }
}
