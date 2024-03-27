using UnityEngine;

public class AnimationBom:AnimationDangerObjOnRigid
{
    ParticleSystem particaleBom;
    bool isExplosion=false;
    private void Awake()
    {
        particaleBom = Resources.Load<ParticleSystem>("ParticleSystems/ExplosionBom2");
    }
    public override void OnAnimation()
    {
        base.OnAnimation();
        LeanTween.delayedCall(3,()=>BomTimeLapse());
    }
    void BomTimeLapse()
    {
        if(!isExplosion)
        {
            Instantiate(particaleBom, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
            isExplosion = true;
            gameObject.SetActive(false);         
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<AnimationBom>() != null)
        {
            Instantiate(particaleBom, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
            isExplosion = true;
            gameObject.SetActive(false);       
        }
    }
}
