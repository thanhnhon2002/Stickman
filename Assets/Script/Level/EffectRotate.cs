using UnityEngine;

public class AnimationRotate : MonoBehaviour
{
    public float speed;
    Vector3 rotation = new Vector3();
    private void Update()
    {
        rotation = transform.eulerAngles + new Vector3(0, 0, speed * Time.deltaTime);
        transform.eulerAngles = rotation;
    }
}
