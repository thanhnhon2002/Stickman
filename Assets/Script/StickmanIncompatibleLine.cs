using UnityEngine;

public class StickmanIncompatibleLine : Stickman
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.transform.tag.Equals("Line")) this.Lose();
    }
}
