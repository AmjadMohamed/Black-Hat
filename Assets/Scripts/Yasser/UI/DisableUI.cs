using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiToDisable;

    public void DisableUIElements()
    {
        foreach (GameObject ui in uiToDisable)
        {
            ui.SetActive(false);
        }
    }
}
