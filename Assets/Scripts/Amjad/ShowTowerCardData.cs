using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTowerCardData : MonoBehaviour
{
    // Private Variables
    [SerializeField] private TowerModifications _towerData;
    [SerializeField] private GameObject _dataCard;
    [SerializeField] private List<Button> _cards;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _attackSpeed;
    [SerializeField] private TMP_Text _attackDamage;
    [SerializeField] private TMP_Text _range;

    public void ShowData()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        if (_name != null)
            _name.text = $"{_towerData.name}";
        if (_attackSpeed != null)
            _attackSpeed.text = $"{_towerData.attackSpeed}";
        if (_attackDamage != null)
            _attackDamage.text = $"{_towerData.damage}";
        if (_range != null)
            _range.text = $"{_towerData.range}";
        _dataCard.SetActive(true);
    }
}
