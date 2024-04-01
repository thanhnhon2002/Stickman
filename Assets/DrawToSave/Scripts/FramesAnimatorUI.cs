using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramesAnimatorUI : MonoBehaviour
{
    public List<Sprite> sprites;
    public float speed = 0.1f;
    private Image img;
    private float nextChange;
    private int current;
    private void Awake()
    {
        img = GetComponent<Image>();
    }
    private void Update()
    {
        if (Time.time > nextChange)
        {
            nextChange = Time.time + speed;
            current++;
            if (current >= sprites.Count) current = 0;
            img.sprite = sprites[current];
        }
    }
}