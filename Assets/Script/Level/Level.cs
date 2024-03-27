using UnityEngine;

public abstract class Level :MonoBehaviour
{
    public virtual void StartLevel()
    {      
        TimeLapse.instance.CountDown();
        transform.GetComponentInChildren<Stickman>().rb.simulated = true;
    }
}
