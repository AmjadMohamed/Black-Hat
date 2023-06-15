using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject timerGameObject;
    private bool _surrendered;

    #region Public Variables

    public static GameManager Instance { get; private set; }
    public GameObject TowerManager;

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
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += ShowAds;
        PhotonNetwork.NetworkingClient.EventReceived += Surrender;
        Application.targetFrameRate = 60;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= ShowAds;
        PhotonNetwork.NetworkingClient.EventReceived -= Surrender;
    }

    private void Start()
    {
        _surrendered = false;
        UpdateUI();
    }

    #endregion

    private void UpdateUI()
    {
        UILayer.Instance.ads.gameObject.SetActive(false);
        UILayer.Instance.roundText.text = "Round: " + MatchManager.Instance.currentRound;

        UILayer.Instance.p1NameText.text = (string)PhotonNetwork.PlayerList[0].CustomProperties[CustomKeys.User_Name];
        UILayer.Instance.p2NameText.text = (string)PhotonNetwork.PlayerList[1].CustomProperties[CustomKeys.User_Name];

        for (int i = 0; i < PhotonNetwork.PlayerList.Length;)
        {
            UILayer.Instance.p1WinsText.text = ((int)PhotonNetwork.PlayerList[0].CustomProperties[CustomKeys.WINS]).ToString();
            UILayer.Instance.p2WinsText.text = ((int)PhotonNetwork.PlayerList[1].CustomProperties[CustomKeys.WINS]).ToString();
            break;
        }

        UILayer.Instance.LoadPlayerUI((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE]);
    }

    private void ShowAds(EventData obj)
    {
        if (obj.Code == MatchManager.AdwareAbilityEventCode)
        {
            UILayer.Instance.ShowAds();
        }
    }

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
                UILayer.Instance.SurrenderUI(UILayer.Instance.DefeatPanel);
            }
            else
            {
                UILayer.Instance.SurrenderUI(UILayer.Instance.VictoryPanel);
            }
        }        
    }

    public void Surrender()
    {
        _surrendered = true;
        SurrenderRaiseEvent();
    }
}
