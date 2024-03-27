using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelContent : MonoBehaviour,IPointerClickHandler
{
    TextMeshProUGUI textLevel;
    int level;
    //bool isDone;
    private void Awake()
    {
        textLevel = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        level = transform.GetSiblingIndex()+1;
        textLevel.text = level.ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(level);
        GameManager.instance.GoToLevel(level);
    }
}