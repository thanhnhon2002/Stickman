using UnityEngine;

public class AnimationBullet : AnimationDangerObjOnRigid
{
    public Vector2 direction;
    public float force;
    public Stickman stickman;

    private void Update()
    {
        if (stickman == null) stickman = GameManager.instance.GetComponentInChildren<Stickman>();
        else
        {
            if (stickman.gameObject.activeInHierarchy && Vector3.Distance(stickman.transform.position, transform.position) <= 1.5)
            {
                stickman.Lose();
                gameObject.SetActive(false);
            }
        }
    }
    public override void OnAnimation()
    {
        base.OnAnimation();
        GetComponent<Rigidbody2D>().AddForce(direction*force,ForceMode2D.Impulse);
    }
}
