using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSFX : MonoBehaviour
{
    [SerializeField] AudioClip CardClickSFX;

    public void CardClick()
    {
        SoundManager.Instance.PlaySoundEffect(CardClickSFX);
    }
}
