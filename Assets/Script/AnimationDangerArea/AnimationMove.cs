using DG.Tweening;
using UnityEngine;
public class AnimationMove:AnimaionDangerObj
{
    public Vector3 direction;
    public float speed;
    public override void OnAnimation()
    {
        Vector3 pos = transform.position;
        transform.DOMove(pos + direction, speed).SetEase(Ease.Linear);
    }
}
