using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] AudioClip ButtonClickSFX;

    public void ButtonClick()
    {
        SoundManager.Instance.PlaySoundEffect(ButtonClickSFX);
    }
}
