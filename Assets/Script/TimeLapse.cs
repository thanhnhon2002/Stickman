using TMPro;
using UnityEngine;

public class TimeLapse : MonoBehaviour
{
    public static TimeLapse instance;
    [SerializeField] float timeEndGame;
    [SerializeField] float currentTime;

    TextMeshProUGUI text;
    public bool isDecTime=true;
    private void Awake()
    {
        this.text = GetComponent<TextMeshProUGUI>();
        instance = this;
    }
    public void InitTimeText()
    {
        isDecTime = true;
        this.currentTime = timeEndGame;
        this.text.SetText(((int)this.timeEndGame).ToString());
    }
    private void Update()
    {
        if(currentTime <timeEndGame&&currentTime>0&&isDecTime)
        {
            currentTime-=Time.deltaTime;
            SetTextTime();
        }
        else if(currentTime != timeEndGame&&currentTime<=0)
        {
            this.currentTime = timeEndGame;
            GameManager.instance.WinGame();
        }
    }
    public void CountDown()
    {
        isDecTime=true;
        currentTime = timeEndGame - 0.05f;
    }
    void SetTextTime()
    {
        this.text.SetText(((int)this.currentTime).ToString());
    }
}
