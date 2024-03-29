using UnityEngine;
using DG.Tweening;
public class AnimationBom:AnimationDangerObjOnRigid
{
    ParticleSystem particaleBom;
    public bool isExplosionTimeLape = true;
    bool isExplosioned = false;
    private void Awake()
    {
        particaleBom = Resources.Load<ParticleSystem>("ParticleSystems/ExplosionBom2");
    }
    public override void OnAnimation()
    {
        base.OnAnimation();
        if(isExplosionTimeLape) LeanTween.delayedCall(3,()=>BomTimeLapse());
        //Sequence sequence = DOTween.Sequence();
        //sequence.AppendInterval(3);
        //sequence.AppendCallback(() => BomTimeLapse());  
    }
    void BomTimeLapse()
    {
        if(!isExplosioned) Explosion();
    }
    public void Explosion()
    {
        Instantiate(particaleBom, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        isExplosioned = true;
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<AnimationBom>() != null)
        {
            Explosion();     
        }
    }
}
