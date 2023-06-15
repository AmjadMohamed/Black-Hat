using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSceneMusic : MonoBehaviour
{
    [SerializeField] AudioClip BgSceneMusic;

    private void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic(BgSceneMusic, 0.1f, true);
    }
}
