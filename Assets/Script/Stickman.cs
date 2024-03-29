using System.Collections;
using UnityEngine;

public class Stickman:MonoBehaviour
{
    public Rigidbody2D rb;
    public ParticleSystem particaleDie;
    private void Awake()
    {
        transform.tag = "Stickman";
        rb = GetComponent<Rigidbody2D>();
        particaleDie = Resources.Load<ParticleSystem>("ParticleSystems/ExplosionDie");
    }
    private void Start()
    {
        rb.simulated=false;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("DangerousArea"))
        {
            if (collision.transform.GetComponent<AnimationBom>() != null&& collision.transform.GetComponent<AnimationBom>().isExplosionTimeLape) collision.transform.GetComponent<AnimationBom>().Explosion();
            this.Lose();
        }
    }
    public void Lose()
    {
        Debug.Log("You Lose");
        AnimationLose();      
    }
    void AnimationLose()
    {
        TimeLapse.instance.isDecTime = false;
        Instantiate(particaleDie, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        Instantiate(particaleDie, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        gameObject.SetActive(false);
        LeanTween.delayedCall(2.5f,()=>GameManager.instance.SetLevel(0));
    }
}
