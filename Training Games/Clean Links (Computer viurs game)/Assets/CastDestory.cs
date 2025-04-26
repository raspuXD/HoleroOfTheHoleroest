using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastDestory : MonoBehaviour
{
    public ParticleSystem particle;
    public string nameForSfx;

    public void CallThis()
    {
        particle.Play();
        AudioManager.Instance.PlaySFX(nameForSfx);
    }
}
