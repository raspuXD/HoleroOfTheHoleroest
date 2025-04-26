using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public string theMusicWhichIsChangedTo;
    public float howFastChanges;
    public float howQuicklyTheNew;

    public void ChangeTheAudioMusic()
    {
        AudioManager.Instance.ChangeMusic(theMusicWhichIsChangedTo, howFastChanges, howQuicklyTheNew);
    }
}
