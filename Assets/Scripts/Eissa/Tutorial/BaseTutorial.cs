using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class BaseTutorial : MonoBehaviour
{
    public Slider healthBarSlider;
    [SerializeField] public float _maxHealth;
    [SerializeField] public float _currentHealth;
    [SerializeField] private Material shieldMaterial;
    private void Start()
    {
        _currentHealth = _maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int dmg)
    {
        if (TutorialManager.Instance._currentIndex != 6)
        {
            _currentHealth -= dmg;
            UpdateHealthBar();
            CheckBaseHealth();
        }
    }

    public void UpdateHealthBar()
    {
        float healthPercentage  = (_currentHealth / _maxHealth) * 100;
        DOTween.To(() => healthBarSlider.value, x => healthBarSlider.value = x, healthPercentage / 100f, 1f);
        float shieldDissolve = shieldMaterial.GetFloat("_DISSOLVE_ctrl");
        shieldDissolve = healthPercentage / 100;
        shieldMaterial.SetFloat("_DISSOLVE_ctrl", shieldDissolve);
       
    }

    void CheckBaseHealth()
    {
        if (_currentHealth <= 0)
        {
            TutorialManager.Instance.NextTutorial();
            _currentHealth = _maxHealth;
            UpdateHealthBar();
        }
    }
}
