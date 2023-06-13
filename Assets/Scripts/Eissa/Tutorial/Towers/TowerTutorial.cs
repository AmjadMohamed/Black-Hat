using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class TowerTutorial : MonoBehaviour
{
    private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private Transform towerIconPosition;
    private GameObject _currentTowerIcon;
    private GameObject _towerIcon;
    [SerializeField] private Transform towerHead;
    [SerializeField] private TowerModifications baseTowerSo;
    [HideInInspector] public TowerModifications currentModifications;
    public event EventHandler TowerGotModified;
    [SerializeField] private Slider healthBarSlider;

    private void Start()
    {
        ModifyTower(baseTowerSo);
        _currentHealth = _maxHealth;
        healthBarSlider.value = 1;
        TutorialManager.Instance.towerCounter++;
    }
    public void ModifyTower(TowerModifications towerModifications)
    {
        this.name = towerModifications.ModificationName;
        _maxHealth = towerModifications.maxHealth;
        _towerIcon = towerModifications.towerIcon;
        currentModifications = towerModifications;
        TowerGotModified?.Invoke(this, EventArgs.Empty);
        SwitchTowerIcon();
    }
    private void SwitchTowerIcon()
    {
        if (_currentTowerIcon != null)
        {
            DestroyImmediate(_currentTowerIcon);
        }
        _currentTowerIcon = Instantiate(_towerIcon, towerIconPosition.transform.position, quaternion.identity, towerHead);
    }
    public void DamageTower(int dmg)
    {
        _currentHealth -= dmg;
        UpdateHealthBar();
        CheckTowerHealth();
        
    }

    void CheckTowerHealth()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    private void UpdateHealthBar()
    {
        float healthPercentage  = (_currentHealth / _maxHealth) * 100;
        DOTween.To(() => healthBarSlider.value, x => healthBarSlider.value = x, healthPercentage / 100f, 1f);
    }
}
