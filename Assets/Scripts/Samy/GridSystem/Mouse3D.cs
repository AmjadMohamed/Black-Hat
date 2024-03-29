using UnityEngine;

public class Mouse3D : MonoBehaviour {

    public static Mouse3D Instance { get; private set; }
    public Camera MyCamera;
    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    private void Awake() {
        Instance = this;
        MyCamera = Camera.main;
    }

    private void Update() {
        Ray ray = MyCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) {
            transform.position = raycastHit.point;
        }
    }

    public  Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();

    private Vector3 GetMouseWorldPosition_Instance() {
        Ray ray = MyCamera.ScreenPointToRay(Input.mousePosition);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, 999f, mouseColliderLayerMask))
        {

            //print(hit.collider.tag + " v " + hit.collider.name);
            return hit.point;
        }
        else
        {
            //UtilsClass.CreateWorldTextPopup("Cannot Build Here!", new Vector3(Input.mousePosition.x - 12, Input.mousePosition.y, Input.mousePosition.z));
            //GridBuildingSystem3D.Instance.DeselectObjectType();
            //transform.position =new Vector3(0,0,0);
            return Vector3.zero;
        }
        return hit.point;
    }
    public  bool CANBUILD()
    {

        Ray ray = MyCamera.ScreenPointToRay(Input.mousePosition);
        bool found = true;
        RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f, 999f, mouseColliderLayerMask);

        foreach (RaycastHit hit in hits)
        {
            print(hit);
            if (!hit.transform.CompareTag("Buildable"))
            {
                return false;
            }
        }
        return found;
    }
}