using Photon.Pun;
using UnityEngine;

public class AdwareAbility : GenericAbility, IAbility
{
    public override void Update()
    {
        base.Update();
        if (MalwaresManager.Instance.AdwareCount >= numberOfMalwareNeededToUse)
        {
            if (_nextUseTime > Time.time)
            {
                _card.interactable = false;
                cooldownText.gameObject.SetActive(true);
                _isOnCooldown = true;
            }
            else if (EnergyManager.Instance._energy < cost)
            {
                _card.interactable = false;
            }
            else
            {
                _card.interactable = true;
                cooldownText.gameObject.SetActive(false);
                _isOnCooldown = false;
            }
        }
        else
        {
            _card.interactable = false;
        }
    }


    public void Use()
    {
        if (Time.time > _nextUseTime)
        {
            EnergyManager.Instance.DecreaseEnergy(cost);
            MatchManager.Instance.AdwareAbilityRaiseEvent();
            _nextUseTime = Time.time + COOLDOWN_TIME;
        }
    }

}
