using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVolume : MonoBehaviour
{
    // Private Variables
    [SerializeField] private Slider _volumeSlider;

    void Start()
    {
        _volumeSlider.value = SoundManager.Instance.GetAudioSourceVolume();
    }

    public void SetGameVolume()
    {
        SoundManager.Instance.SetBackgroundMusicVolume(_volumeSlider.value);
    }
}
