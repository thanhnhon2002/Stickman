using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string pathLevelPrefeb;
    Level currentLevel;
    public int levelIndex;
    public int levelMaxIndex;
    public ScrollRect scrollLevel;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
    }
    private void Start()
    {
       scrollLevel.gameObject.SetActive(false);
       SetLevel(1);
    }
    public void SetLevel(int unit)
    {
        DrawLine.instance.isDraw = true;
        levelIndex+=unit;
        if(levelIndex>levelMaxIndex) levelIndex = levelMaxIndex;
        else if(levelIndex<0) levelIndex = 0;
        if (currentLevel != null) Destroy(currentLevel.gameObject);
        this.currentLevel = Instantiate(Resources.Load<GameObject>(pathLevelPrefeb+levelIndex.ToString()),transform)
            .GetComponent<Level>();
        TimeLapse.instance.InitTimeText();
        DrawLine.instance.ResetLine();
    }
    public void GoToLevel(int level)
    {
        SetStatusScrollLevel();
        if (level > levelMaxIndex) level = levelMaxIndex;
        else if (level < 0) level = 0;
        levelIndex = level;
        SetLevel(0);
    }
    public void StartLevel()
    {
        this.currentLevel.StartLevel();
    }
    public void WinGame()
    {
        Debug.Log("You Win");
        SetLevel(1);
    }
    public void SetStatusScrollLevel()
    {
        if (scrollLevel.gameObject.activeInHierarchy)
        {
            scrollLevel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            scrollLevel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
} 
