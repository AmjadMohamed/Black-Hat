using UnityEngine;
using System.Collections.Generic;

public class PlacedObject_DoneTutorial : MonoBehaviour
    {
        public static PlacedObject_DoneTutorial Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO) {
            GameObject placedObjectTransform = Instantiate(placedObjectTypeSO.prefab.gameObject, worldPosition, Quaternion.Euler(0, 0, 0),GridBuildingSystem3DTutorial.Instance.towerParent.transform);
            PlacedObject_DoneTutorial placedObject = placedObjectTransform.GetComponent<PlacedObject_DoneTutorial>();
            placedObject.Setup(placedObjectTypeSO, origin, dir);
            return placedObject;
        }




        private PlacedObjectTypeSO placedObjectTypeSO;
        private Vector2Int origin;
        private PlacedObjectTypeSO.Dir dir;

        private void Setup(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin, PlacedObjectTypeSO.Dir dir)
        {
            this.placedObjectTypeSO = placedObjectTypeSO;
            this.origin = origin;
            this.dir = dir;
        }

        public List<Vector2Int> GetGridPositionList()
        {
            return placedObjectTypeSO.GetGridPositionList(origin, dir);
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public override string ToString()
        {
            return placedObjectTypeSO.nameString;
        }

    }
