using TMPro;
using UnityEngine;

public class TimeLapse : MonoBehaviour
{
    public static TimeLapse instance;
    [SerializeField] float timeEndGame;
    [SerializeField] float currentTime;

    TextMeshProUGUI text;
    private void Awake()
    {
        this.text = GetComponent<TextMeshProUGUI>();
        instance = this;
    }
    public void InitTimeText()
    {
        this.currentTime = timeEndGame;
        this.text.SetText(((int)this.timeEndGame).ToString());
    }
    private void Update()
    {
        if(currentTime <timeEndGame&&currentTime>0)
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
        currentTime = timeEndGame - 0.05f;
    }
    void SetTextTime()
    {
        this.text.SetText(((int)this.currentTime).ToString());
    }
}
