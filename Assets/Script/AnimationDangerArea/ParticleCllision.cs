﻿using UnityEngine;

public class ParticleCllision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<Stickman>() != null) other.GetComponent<Stickman>().Lose();
        if (other.GetComponent<AnimationBom>() != null) other.GetComponent<AnimationBom>().Explosion();
    }
}
