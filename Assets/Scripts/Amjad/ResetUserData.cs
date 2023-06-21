using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetUserData : MonoBehaviour
{
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(MatchManager.Instance.MAIN_MENU_SCENE_NAME);
    }
}
