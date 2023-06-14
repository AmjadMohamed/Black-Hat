using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILayer : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private Canvas attackerUI;
    [SerializeField] private Canvas defenderUI;
    [SerializeField] private GameObject switchingSidesPanel;
    [SerializeField] private TMP_Text roundNumText;
    [SerializeField] private TMP_Text attackerDefenderTurnText;
    [SerializeField] private GameObject ads;
    [SerializeField] private GameObject matchDisconnetedPanel;

    #endregion

    #region Public Variables

    public TextMeshProUGUI roundText;
    public TextMeshProUGUI p1WinsText;
    public TextMeshProUGUI p1NameText;
    public TextMeshProUGUI p2WinsText;
    public TextMeshProUGUI p2NameText;
    public GameObject VictoryPanel;
    public GameObject DefeatPanel;

    public static UILayer Instance { get; private set; }

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        StartCoroutine(EnableSwitchingSidesPanel(0));
    }

    #endregion

    #region Public Methods

    public void LoadPlayerUI(MatchManager.Side side)
    {
        if (attackerUI.enabled || defenderUI.enabled)
        {
            attackerUI.enabled = false;
            defenderUI.enabled = false;
        }

        switch (side)
        {
            case MatchManager.Side.Attacker:
                defenderUI.enabled = false;
                attackerUI.enabled = true;
                break;
            case MatchManager.Side.Defender:
                attackerUI.enabled = false;
                defenderUI.enabled = true;
                break;
        }
    }

    public IEnumerator EnableSwitchingSidesPanel(int increment)
    {
        roundNumText.text = $"Round: {MatchManager.Instance.currentRound + increment}";
        if ((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Attacker)
        {
            attackerDefenderTurnText.text = "Now You Are An <color=red>Attacker</color>";
        }
        else
        {
            attackerDefenderTurnText.text = "Now You Are A <color=blue>Defender</color>";
        }


        switchingSidesPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        switchingSidesPanel.GetComponent<PanelsDotween>().HidePanel();
    }

    public void ReturnToMainMenu()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SceneManager.LoadSceneAsync(MatchManager.Instance.MAIN_MENU_SCENE_NAME);
            PhotonNetwork.Disconnect();
            Destroy(gameObject);
        }
    }

    public void ShowAds()
    {
        foreach (Transform AD in ads.transform)
        {
            AD.gameObject.SetActive(true);
        }
        ads.SetActive(true);
    }

    public void EnableDisconnectionPanel()
    {
        if (SceneManager.GetActiveScene().name == MatchManager.Instance.GAMEPLAY_SCENE_NAME)
        {
            StartCoroutine(EnableDisconnectedPanel());
        }
    }

    public void EnableEndgamePanel(GameObject endGamePanel)
    {
        StartCoroutine(EnableGameEndedPanel(endGamePanel));
    }

    #endregion

    #region Private Methods

    private IEnumerator EnableDisconnectedPanel()
    {
        matchDisconnetedPanel.SetActive(true);
        SceneManager.LoadScene(MatchManager.Instance.MAIN_MENU_SCENE_NAME);
        yield return new WaitForSeconds(3.0f);
        matchDisconnetedPanel.GetComponent<PanelsDotween>().HidePanel();
        Destroy(gameObject, .5f);
    }

    private IEnumerator EnableGameEndedPanel(GameObject endgamePanel)
    {
        endgamePanel.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        endgamePanel.SetActive(false);
        ReturnToMainMenu();
    }

    #endregion
}
