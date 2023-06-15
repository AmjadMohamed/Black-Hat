using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class AttackerDefenderPanelTween : MonoBehaviour
{
    // private variables
    private Tweener _panelTweener;
    [SerializeField] private GameObject _panel;
    [SerializeField] private float _delayTime;

    // public variables


    private void Start()
    {
        _panel.SetActive(false);
    }

    private void Update()
    {
        ShowPanel();
    }

    void ShowPanel()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _panelTweener?.Kill(); // Kill any previous tweens
                _panel.SetActive(true);
                //_panel.transform.localScale = new Vector3(0, -2000, 0);
                _panelTweener = _panel.transform.DOLocalMove(new Vector3(0, 70, 0), 1f);
                ClosePanel();
            }
        }
    }

    void ClosePanel()
    {
        //_panelTweener?.Kill(); // Kill any previous tweens
        _panelTweener = _panel.transform.DOLocalMove(new Vector3(0, -2000f, 0f), 10f).OnComplete(() =>
        {
            _panel.SetActive(false);
        }).SetDelay(_delayTime);
    }
}
