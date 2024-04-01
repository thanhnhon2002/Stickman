using System.Collections;
using UnityEngine;

public class AnimaitonSpawn : AnimaionDangerObj
{
    GameObject objectSpawn;
    public string path;
    public float spawnCount;
    public float timeSpawn;
    private void Awake()
    {
        objectSpawn = Resources.Load<GameObject>(path);
    }
    public override void OnAnimation()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        for(int i=0;i<spawnCount;i++)
        {
            Instantiate(objectSpawn,transform);
            yield return new WaitForSeconds(timeSpawn);
        }
    }
}