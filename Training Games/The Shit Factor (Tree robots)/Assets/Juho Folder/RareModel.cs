using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class RareModel : MonoBehaviour
{
    public Sprite normal, hiden;
    public SpriteRenderer normalRenderer;
    public ParticleSystem close, open;

    public void HideLol(int huh)
    {
        if(huh == 0)
        {
            normalRenderer.sprite = hiden;
            open.Play();
        }
        else
        {
            normalRenderer.sprite = normal;
            close.Play();
        }
    }
}
