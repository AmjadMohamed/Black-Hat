using System;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TowerModificationManager : MonoBehaviour
{
    // Private
    private static TowerModificationManager _instance;


    // Public
    public static TowerModificationManager Instance => _instance;
    [HideInInspector] public TowerModifications TowerModifications;
    [HideInInspector] public int ObjectEnergyCost;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        if (!_instance)
        {
            _instance = this;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            ApplyModificationToTower(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject())
        {
            ApplyModificationToTower(Input.GetTouch(0).position);
        }
    }


    void ApplyModificationToTower(Vector3 position)
    {
        Ray ray = _camera.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if ((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Defender)
            {
                if (hit.transform.CompareTag("Tower") && TowerModifications != null)
                {
                    if (ObjectEnergyCost <= EnergyManager.Instance._energy)
                    {
                        hit.transform.GetComponent<Tower>().ModifyTower(TowerModifications);
                        if (TowerModifications.SpawnSFX != null)
                        {
                            SoundManager.Instance.PlaySoundEffect(TowerModifications.SpawnSFX);
                        }
                        EnergyManager.Instance.DecreaseEnergy(ObjectEnergyCost);
                    }
                }
            }
        }
    }
}