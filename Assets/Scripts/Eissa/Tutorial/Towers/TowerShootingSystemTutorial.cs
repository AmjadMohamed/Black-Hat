using UnityEngine;
using System;
using System.Collections.Generic;

 public class TowerShootingSystemTutorial : MonoBehaviour
 {
     [SerializeField] private TowerTutorial _towerScript;
    private TowerModifications _towerModifications;

    private float _range;
    private int _damage;
    private float _attackSpeed;
    private float _attackCountdown;
    private SphereCollider _targetingCollider;
    private PoolableObject _towerProjectilePrefab;
    private ObjectPool _projectilePool;
    private GameObject _currentTarget;
    private List<MalwareTutorial> _enemies = new List<MalwareTutorial>();

    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform towerHead;
    [SerializeField] private float towerHeadRotationSpeed;

    [SerializeField] private GameObject muzzleGameObject;
    private ParticleSystem _muzzleParticleSystem;
    private void Awake()
    {
        _towerScript.TowerGotModified += GetTowerModification;
        _targetingCollider = GetComponent<SphereCollider>();
        _muzzleParticleSystem = muzzleGameObject.GetComponent<ParticleSystem>();
        _muzzleParticleSystem.Stop();
    }

    private void Update()
    {
        if (_enemies.Count > 0)
        {
            GettingTheMostDangerousEnemy();
            RotateTheTowerHead();
            CalculateShootingRate();
        }
    }

    void GetTowerModification(object sender, EventArgs e)
    {
        _towerModifications = _towerScript.currentModifications;
        UpdateTowerStats();
    }

    private void UpdateTowerStats()
    {
        _range = _towerModifications.range;
        _damage = _towerModifications.damage;
        _attackSpeed = _towerModifications.attackSpeed;
        _towerProjectilePrefab = _towerModifications.bulletPrefab;
        _targetingCollider.radius = _range;
        _projectilePool = ObjectPool.CreatInstance(_towerProjectilePrefab, 20);
    }
    private void GettingTheMostDangerousEnemy()
    {
        float dangerousLevel = float.MinValue;
        GameObject tempEnemy = null;
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] == null) //!_enemies[i].gameObject.activeInHierarchy
            {
                _enemies.RemoveAt(i);
                continue;
            }
            float currentDangerousLevel = (-i + 1) * 0.6f + (0.5f * _enemies[i].MovementSpeed) + (0.7f * _enemies[i].Health);
            if (dangerousLevel < currentDangerousLevel)
            {
                dangerousLevel = currentDangerousLevel;
                tempEnemy = _enemies[i].gameObject;
            }
        }
        _currentTarget = tempEnemy;
    }
    private void RotateTheTowerHead()
    {
        if (_currentTarget != null)
        {
            Vector3 dir = _currentTarget.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, Time.deltaTime * towerHeadRotationSpeed).eulerAngles;
            towerHead.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
    private void CalculateShootingRate()
    {
        if (_enemies.Count > 0)
        {
            if (_attackCountdown <= 0)
            {
                Shoot();
                _attackCountdown = 1f / _attackSpeed;
            }

            _attackCountdown -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        var projectile = _projectilePool.GetObject() as TowerProjectileTutorial;
        _muzzleParticleSystem.Play();
        if (projectile != null)
        {
            projectile.transform.position = shootingPoint.position;
            if (_currentTarget != null)
            {
                projectile.GetTarget(_currentTarget.transform, _damage);
            }
        }

        if (_towerScript.currentModifications.ShootingSFX != null)
        {
            SoundManager.Instance.PlaySoundEffect(_towerScript.currentModifications.ShootingSFX , 1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Add(other.GetComponent<MalwareTutorial>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Remove(other.GetComponent<MalwareTutorial>());
        }
    }


 }
