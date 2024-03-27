using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class FramesAnimator : MonoBehaviour
{
    public List<Sprite> sprites;
    public float speed = 0.1f;
    private SpriteRenderer spriteRenderer;
    private float nextChange;
    private int current;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(Time.time > nextChange)
        {
            nextChange = Time.time + speed;
            current++;
            if (current >= sprites.Count) current = 0;
            spriteRenderer.sprite = sprites[current];
        }
    }
}