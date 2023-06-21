using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILayer : MonoBehaviourPunCallbacks
{
    #region Serialized Fields

    [SerializeField] private GameObject[] uiToDisable;
    [SerializeField] private GameObject attackerUI;
    [SerializeField] private GameObject defenderUI;
    [SerializeField] private GameObject switchingSidesPanel;
    [SerializeField] private TMP_Text roundNumText;
    [SerializeField] private TMP_Text attackerDefenderTurnText;
    public GameObject ads;
    [SerializeField] private GameObject matchDisconnetedPanel;
    private bool _surrendered;

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
        }

        if (SceneManager.GetActiveScene().name == MatchManager.Instance.GAMEPLAY_SCENE_NAME)
        {
            StartCoroutine(EnableSwitchingSidesPanel(0));
        }

    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += EnableEndgamePanel;
        PhotonNetwork.NetworkingClient.EventReceived += Surrender;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= EnableEndgamePanel;
        PhotonNetwork.NetworkingClient.EventReceived -= Surrender;
    }

    private void Start()
    {
        _surrendered = false;
    }

    #endregion

    #region Public Methods

    public void LoadPlayerUI(MatchManager.Side side)
    {
        if(attackerUI.activeSelf|| defenderUI.activeSelf)
        {
            attackerUI.SetActive(false);
            defenderUI.SetActive(false);
        }

        switch(side)
        {
            case MatchManager.Side.Attacker:
                defenderUI.SetActive(false);
                attackerUI.SetActive(true);
                break;
            case MatchManager.Side.Defender:
                defenderUI.SetActive(true);
                attackerUI.SetActive(false);
                break;
        }
    }

    public IEnumerator EnableSwitchingSidesPanel(int increment)
    {
        roundNumText.text = $"Round: {MatchManager.Instance.currentRound + increment}";
        if ((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Attacker)
        {
            attackerDefenderTurnText.text = "<color=red>Attacker</color>";
        }
        else
        {
            attackerDefenderTurnText.text = "<color=blue>Defender</color>";
        }


        switchingSidesPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        switchingSidesPanel.GetComponent<PanelsDotween>().HidePanel();
    }

    private void ReturnToMainMenu()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(MatchManager.Instance.MAIN_MENU_SCENE_NAME);
            MatchManager.Instance.InGame = false;
            PhotonNetwork.LeaveRoom();
        }
        Destroy(gameObject);
    }

    public void ShowAds()
    {
        foreach (Transform AD in ads.transform)
        {
            AD.gameObject.SetActive(false);
        }
        ads.SetActive(false);


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

    public void EnableEndgamePanelRaiseEvent()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(MatchManager.GameEndEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    private void SurrenderUI(GameObject endgamePanel)
    {
        StartCoroutine(EnableGameEndedPanel(endgamePanel));
    }

    private void EnableEndgamePanel(EventData obj)
    {
        if (obj.Code == MatchManager.GameEndEventCode)
        {
            GameObject endgamePanel = null;

            if ((int)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.WINS] >
                (int)PhotonNetwork.PlayerListOthers[0].CustomProperties[CustomKeys.WINS])
            {
                endgamePanel = VictoryPanel;
            }
            else if ((int)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.WINS] <
                     (int)PhotonNetwork.PlayerListOthers[0].CustomProperties[CustomKeys.WINS])
            {
                endgamePanel = DefeatPanel;
            }

            StartCoroutine(EnableGameEndedPanel(endgamePanel));
        }
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
        SoundManager.Instance.PlaySoundEffect(MatchManager.Instance.endgameClip);
        //SoundManager.Instance.StopBackgroundMusic();

        foreach (GameObject ui in uiToDisable)
        {
            ui.SetActive(false);
        }

        DisableUI disableUI = FindObjectOfType<DisableUI>();
        disableUI.DisableUIElements();

        yield return new WaitForSeconds(3.0f);
        endgamePanel.SetActive(false);

        ReturnToMainMenu();
    }

    #endregion


    private void SurrenderRaiseEvent()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(MatchManager.SurrenderEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    private void Surrender(EventData obj)
    {
        if (obj.Code == MatchManager.SurrenderEventCode)
        {
            MatchManager.Instance.SetPlayerDisconnected(false);
            if (_surrendered)
            {
               SurrenderUI(DefeatPanel);
            }
            else
            {
                SurrenderUI(VictoryPanel);
            }
        }
    }

    public void Surrender()
    {
        _surrendered = true;
        SurrenderRaiseEvent();
    }
}
