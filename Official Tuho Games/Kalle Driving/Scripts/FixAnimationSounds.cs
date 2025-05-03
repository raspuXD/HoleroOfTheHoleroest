using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixAnimationSounds : MonoBehaviour
{
    public void PlayRage()
    {
        AudioManager.Instance.PlaySFX("Rage");
    }

    public void PlayTurbo()
    {
        AudioManager.Instance.PlaySFX("Turbo");
    }

    public void PlayAuts()
    {
        AudioManager.Instance.PlaySFX("Auts");
    }

    public void PlayMetal()
    {
        AudioManager.Instance.PlaySFX("Metal");
    }
}
