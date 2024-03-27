using System.Collections;
using UnityEngine;

public class AnimationRotateDanger : AnimaionDangerObj
{
    public float angle;
    public float speed;
    public float offsetAngle;

    Vector3 rotation;
    float currentAngle;
    float leftAngle;
    float rightAngle;
    private void Start()
    {
        rotation = transform.eulerAngles;
        rightAngle = (180-angle)/-2;
        leftAngle = rightAngle - angle;
        transform.eulerAngles = new Vector3(rotation.x,rotation.y,rightAngle-1);
    }
    public override void OnAnimation()
    {
        StartCoroutine(Animation());
    }
    void Rotate(float speed)
    {
        if (rightAngle == leftAngle)
        {
            transform.eulerAngles = new Vector3(rotation.x, rotation.y, -90);
            return;
        }
        transform.eulerAngles = new Vector3(rotation.x,rotation.y,rotation.z+speed*Time.deltaTime);
    }
    IEnumerator Animation()
    {
        while (true)
        {
            rotation = transform.eulerAngles;
            currentAngle = -360 + rotation.z;

            if (currentAngle < leftAngle)
            {
                // rightAngle -= offsetAngle;
                //leftAngle += offsetAngle;
                angle = rightAngle;
            }
            else if (currentAngle > rightAngle)
            {
                //rightAngle -= offsetAngle;
                // leftAngle += offsetAngle;
                angle = leftAngle;
            }
            if (rightAngle <= leftAngle)
            {
                rightAngle = leftAngle;
                offsetAngle = 0;
            }
            if (angle > -90) Rotate(this.speed / (1 + Mathf.Abs(currentAngle + 90) / 20)); else if (angle < -90) Rotate(-this.speed / (1 + Mathf.Abs(currentAngle + 90) / 20));
            yield return null;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Line")|collision.transform.tag.Equals("Stickman")) this.speed = 0;
    }
}
