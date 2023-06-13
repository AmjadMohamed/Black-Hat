using UnityEngine;
using UnityEngine.UI;

public class ModificationCardTutorial : MonoBehaviour
{
    
    [SerializeField] private TowerModifications _modificationCard;
    private Button _card;

    private void Awake()
    {
        _card = GetComponent<Button>();
    }

    void Update()
    {
        if (TutorialManager.Instance._currentEnergy < _modificationCard.EnergyCost)
        {
            _card.interactable = false;
        }
        else
        {
            _card.interactable = true;
        }
    }

    public void ChooseObjectToModify()
    {
        TutorialManager.Instance.towerModifications = _modificationCard;
    }
}
