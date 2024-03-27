using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDengerAreaOnRigid : Level
{
    [SerializeField] List<AnimaionDangerObj> animaionDangerObj = new List<AnimaionDangerObj>();
    private void Awake()
    {
        animaionDangerObj = transform.GetComponentsInChildren<AnimaionDangerObj>().ToList();
    }
    public override void StartLevel()
    {
        base.StartLevel();
        foreach(var obj in animaionDangerObj)
        {
           obj.OnAnimation();
        }
    }
}
