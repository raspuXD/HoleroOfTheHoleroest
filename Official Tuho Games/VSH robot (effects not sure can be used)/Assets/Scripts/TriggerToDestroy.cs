using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToDestroy : MonoBehaviour
{
    public AudioSource m_AudioSource;
    public ParticleSystem system;
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        if(m_AudioSource != null)
        {
            m_AudioSource.Play();
        }
        if(system != null)
        {
            system.Play();
        }
    }
}
