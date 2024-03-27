using UnityEngine;

public class Stickman:MonoBehaviour
{
    public Rigidbody2D rb;
   // public ParticleSystem particaleDie;
    private void Awake()
    {
        transform.tag = "Stickman";
        rb = GetComponent<Rigidbody2D>();
       // particaleDie = Resources.Load<ParticleSystem>("ParticalSystem/Explosion1");
    }
    private void Start()
    {
        rb.simulated=false;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("DangerousArea")) this.Lose();
    }
    protected void Lose()
    {
        Debug.Log("You Lose");
        gameObject.SetActive(false);
       // Instantiate(particaleDie,transform.position+new Vector3(0,-1,0),Quaternion.identity);
       // Instantiate(particaleDie,transform.position+new Vector3(0,1,0),Quaternion.identity);
        GameManager.instance.SetLevel(0);
    }
}
