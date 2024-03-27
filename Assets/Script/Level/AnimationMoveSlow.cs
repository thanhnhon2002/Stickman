using UnityEngine;

public class AnimationMoveSlow : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    Vector3 pos;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rb.simulated == true) return;
        transform.position = transform.position + direction * speed * Time.deltaTime;
    }

}