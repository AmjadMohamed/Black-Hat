using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    #region Tutorial Variables

    [Header("Tutorial Variables")]
    [SerializeField] private List<GameObject> popups = new List<GameObject>();
    public int _currentIndex = 0;
    private static TutorialManager _instance;
    public static TutorialManager Instance => _instance;
    //private float _timeBetweenTutorials ;
    private Coroutine _timeBetweenTutorialCoroutine;
    private bool _closedTheTutorialPanel = false;

    #endregion
    
    #region Camera Variabels
    [Space]
    [Header("Camera Variables")]
    public Transform target;

    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoomDistance = 5f;
    [SerializeField] private float maxZoomDistance = 20f;
    [SerializeField] private float rotateDelay = 1f; // Delay in seconds before allowing rotation after zooming
    [SerializeField] private float minSwipeDistance = 1; // Minimum swipe distance required for rotation
    [SerializeField] private GameObject _camera;
    private Camera camera;

    private Vector3 previousMousePosition;
    private bool isDragging;
    private float rotateTimer;
    private bool shouldRotate;
    private bool isZoomed = false;
    private bool isRotate = false;
    private bool isPanned = false;

    #endregion

    #region Malwares 

    [Space] [Header("Malwares")] 
    [SerializeField] private List<GameObject> malware = new List<GameObject>();
    [HideInInspector] public int malwareIndex = int.MaxValue;
    [HideInInspector] public int currentMalwareCost;
    private int spwanedMalwareNumber;
    [SerializeField] private int requiredNumberOfMalware = 12;
    [SerializeField] private GameObject malwareParent;

    #endregion

    #region Energy Variabels

    [Space] [Header("Energy Variables")] 
    [SerializeField] private int maxEnergy;
    public int _currentEnergy;

    #endregion

    #region Tower and Tower Modifications Variables
    [Space] [Header("Tower")]

    [HideInInspector] public TowerModifications towerModifications;
    [SerializeField] private GameObject towerManager;
    [SerializeField] private GameObject towerParent;
    [HideInInspector] public int towerCounter;
    private int RequierdTowerNumber = 3;
    #endregion

    #region UI

    [Space] [Header("UI")]
    [SerializeField] private GameObject UI;
    [SerializeField] private List<GameObject> TowerModificationsUI = new List<GameObject>();
    [SerializeField] private GameObject attackerPanel;
    [SerializeField] private GameObject defenderPanel;

    #endregion

    private void Start()
    {
        if (!_instance)
        {
            _instance = this;
        }

      
        ResettingTheGameVariables();
        SwitchThePopupsOnAndOff();
        
        
        camera = Camera.main;
    }

    private void Update()
    {
        if (towerModifications != null)
        {
            print(towerModifications.ModificationName);
        }
        if (_closedTheTutorialPanel)
        {
            switch (_currentIndex)
            {
                case 0: // the Welcome massage
                    
                    break;
                case 1: // for teaching camera zoom
                    EnableTheZoomTutorial();
                    break;
                case 2: // for teaching camera rotation
                    EnableTheRotationsTutorial();
                    break;
                case 4: // for teaching Spawning the malware
                    EnableTheMalwareSpawningTutorial();
                    break;
                case 10: 
                    EnableTheAbilitySpawningTutorial();
                    break;;
                case 5: // for teaching the player about tower Placement
                    EnableTheTowerPlacementTutorial();
                    break;
                case 6: // for teaching the player about tower modifications
                    EnableTheTowerModificationsTutorial();
                    break;
            }
        }
        
        print(_currentIndex);
        
    }

    #region Tutorial Functions

    private void EnableTheZoomTutorial()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            HandleCameraZoom();
            print("in case 0");
            print(isZoomed);
        }
    }
    private void EnableTheRotationsTutorial()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            HandleCameraRotation();
        }
    }
    private void EnableTheZoomAndRotationsTutorial()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            HandleCameraRotation();
            HandleCameraZoom();
        }
    }

    private void EnableTheMalwareSpawningTutorial()
    {
        if (!UI.activeInHierarchy)
        {
            UI.SetActive(true);
        }
        towerManager.SetActive(false);
        defenderPanel.SetActive(false);
        if (!attackerPanel.activeInHierarchy)
        {
            attackerPanel.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            SpawnObject(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject())
        {
            SpawnObject(Input.GetTouch(0).position);
        }

        EnableTheZoomAndRotationsTutorial();
    }
    private void EnableTheTwoSideTutorial()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            HandleCameraRotation();
            HandleCameraZoom();
        }
        EnableTheZoomAndRotationsTutorial();
    }

    private void EnableTheTowerPlacementTutorial()
    {
        if (!UI.activeInHierarchy)
        {
            UI.SetActive(true);

        }
        attackerPanel.SetActive(false);
        if (!defenderPanel.activeInHierarchy)
        {
            defenderPanel.SetActive(true);
            towerManager.SetActive(true);
        }
        EnableTheZoomAndRotationsTutorial();
        if (towerCounter > RequierdTowerNumber)
        {
            if (_timeBetweenTutorialCoroutine == null)
            {
                _timeBetweenTutorialCoroutine = StartCoroutine(FinishTheCurrentTutorial(2f));
            }
        }

    }

    private void EnableTheTowerModificationsTutorial()
    {
        if (!UI.activeInHierarchy)
        {
            UI.SetActive(true);

        }
        attackerPanel.SetActive(false);
        if (!defenderPanel.activeInHierarchy)
        {
            defenderPanel.SetActive(true);
            towerManager.SetActive(true);
        }

        if (!TowerModificationsUI[0].activeInHierarchy)
        {
            SwitchingTheModificationCardUI(true);
        }

        EnableTheZoomAndRotationsTutorial();
        GetInputForTowerModifications();

    }

    private void EnableTheAbilitySpawningTutorial()
    {
       
    }

    #endregion
    

    public void SwitchThePopupsOnAndOff()
    {
        for (int i = 0; i < popups.Count; i++)
        {
            if (i == _currentIndex)
            {
                popups[i].gameObject.SetActive(true);
            }
            else
            {
                popups[i].gameObject.SetActive(false);
            }
        }
    }

    public void ClosedTheTutorialPanelButton()
    {
        _closedTheTutorialPanel = true;
    }

    public void NextTutorial()
    {
        if (_timeBetweenTutorialCoroutine == null)
        {
            _timeBetweenTutorialCoroutine = StartCoroutine(FinishTheCurrentTutorial(2f));
        }
    }

    #region Camera Functions

    private void HandleCameraRotation()
    {
        if (rotateTimer > 0f)
        {
            rotateTimer -= Time.deltaTime;
            shouldRotate = false; // Disable rotation during the delay
        }
        else
        {
            shouldRotate = true; // Enable rotation after the delay
        }

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && shouldRotate && !HasZoomInput())
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 deltaMousePosition = currentMousePosition - previousMousePosition;

            // Check if the delta position exceeds the minimum swipe distance for rotation
            if (deltaMousePosition.magnitude > minSwipeDistance)
            {
                // Rotate camera around the target
                camera.transform.RotateAround(target.position, Vector3.up, deltaMousePosition.x * rotationSpeed);
                //transform.RotateAround(target.position, transform.right, -deltaMousePosition.y * rotationSpeed);
                if (!isRotate)
                {
                    StartCoroutine(FinishTheCurrentTutorial(2f));
                    isRotate = true;
                    print("rotate");
                }
            }
        }

        previousMousePosition = Input.mousePosition;

    }

    private void HandleCameraZoom()
    {
        float zoomInput = GetZoomInput();
        ZoomCamera(zoomInput);

        // Start the rotate timer when zooming occurs
        if (Mathf.Abs(zoomInput) > 0.001f)
        {
            if (isZoomed == false)
            {
                StartCoroutine(FinishTheCurrentTutorial(2f));
                isZoomed = true;
            }

            rotateTimer = rotateDelay;
        }
    }

    private bool HasZoomInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            return Mathf.Abs(touchDeltaMag - prevTouchDeltaMag) > 0.001f;
        }
        else
        {
            return Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.001f;
        }
    }

    private float GetZoomInput()
    {
        float zoomInput = 0f;

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            zoomInput = prevTouchDeltaMag - touchDeltaMag;
        }
        else
        {
            zoomInput = -Input.GetAxis("Mouse ScrollWheel") * 100;

        }

        return zoomInput;

    }

    private void ZoomCamera(float zoomInput)
    {
        Vector3 cameraToTarget = target.position - camera.transform.position;
        float currentDistance = cameraToTarget.magnitude;

        float newDistance = currentDistance + zoomInput * zoomSpeed;
        newDistance = Mathf.Clamp(newDistance, minZoomDistance, maxZoomDistance);

        Vector3 newCameraPosition = target.position - cameraToTarget.normalized * newDistance;
        camera.transform.position = newCameraPosition;
    }

    #endregion

    #region Spawning Malwares Functions
    void SpawnObject(Vector3 position)
    {
        Ray ray = camera.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Road"))
            {
                Vector3 spawnPosition = hit.point;
                spawnPosition.y += .3f;
                print("hit road");
                if (malwareIndex < malware.Count && currentMalwareCost <= _currentEnergy)
                {
                    Instantiate(malware[malwareIndex], spawnPosition, Quaternion.identity,malwareParent.transform);
                    _currentEnergy -= currentMalwareCost;
                    CheckTheProgressOfSpawningTutorial();
                }
            }
        }
    }

    private void DestroyAllTheSpawnedMalware()
    {
        foreach (Transform malwareInstance in malwareParent.transform)
        {
            Destroy(malwareInstance.gameObject);
        }
    }

    private void CheckTheProgressOfSpawningTutorial()
    {
        spwanedMalwareNumber++;
        if (spwanedMalwareNumber >= requiredNumberOfMalware)
        {
            if (_timeBetweenTutorialCoroutine == null)
            {
                print("Spwaned all the malware");
                _timeBetweenTutorialCoroutine = StartCoroutine(FinishTheCurrentTutorial(3f));
            }
        }
    }
    public void SpawnMalwareButtons(int MalwareIndex)
    {
        malwareIndex = MalwareIndex;
        var malwareScript = malware[malwareIndex].GetComponent<MalwareTutorial>();
        currentMalwareCost = malwareScript.EnergyCost;
    }
    #endregion

    #region Tower Modifications Functions

    public void GetInputForTowerModifications()
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
        Ray ray = camera.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Tower") && towerModifications != null)
            {
                if (towerModifications.EnergyCost <= _currentEnergy)
                {
                    hit.transform.GetComponent<TowerTutorial>().ModifyTower(towerModifications);
                    _currentEnergy -= towerModifications.EnergyCost;
                    towerModifications = null;
                }
                towerModifications = null;
            }
        }
    }

    private void SwitchingTheModificationCardUI(bool state)
    {
        for (int i = 0; i < TowerModificationsUI.Count; i++)
        {
            TowerModificationsUI[i].SetActive(state);
        }
    }
    private void DestroyAllTheSpawnedTowers()
    {
        foreach (Transform towerInstance in towerParent.transform)
        {
            Destroy(towerInstance.gameObject);
        }
    }

    #endregion
    void ResettingTheGameVariables()
    {
        UI.SetActive(false);
        SwitchingTheModificationCardUI(false);
        towerManager.SetActive(false);
        defenderPanel.SetActive(false);
        attackerPanel.SetActive(false);
        towerCounter = 0;
        spwanedMalwareNumber = 0;
        DestroyAllTheSpawnedMalware();
        _currentEnergy = maxEnergy;
        malwareIndex = int.MaxValue;
    }

    IEnumerator FinishTheCurrentTutorial(float _timeBetweenTutorials)
    {
        yield return new WaitForSeconds(_timeBetweenTutorials);
        _currentIndex++;
        SwitchThePopupsOnAndOff();
        ResettingTheGameVariables();
        _timeBetweenTutorialCoroutine = null;
        _closedTheTutorialPanel = false;
    }
}
