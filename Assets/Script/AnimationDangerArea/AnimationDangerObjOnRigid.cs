using System.Collections;
using UnityEngine;

public class AnimationDangerObjOnRigid:AnimaionDangerObj
{
    public override void OnAnimation()
    {
        GetComponent<Rigidbody2D>().simulated = true;
    }
}
public class AnimationBom:AnimationDangerObjOnRigid
{
   // ParticleSystem particaleBom;
    private void Awake()
    {
        //particaleBom = Resources.Load<ParticleSystem>("ParticalSystem/Explosion2");
    }
    public override void OnAnimation()
    {
        base.OnAnimation();
        //StartCoroutine(BomTimeLapse());
    }
    IEnumerator BomTimeLapse()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
        //Instantiate(particaleBom, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
    }
}
