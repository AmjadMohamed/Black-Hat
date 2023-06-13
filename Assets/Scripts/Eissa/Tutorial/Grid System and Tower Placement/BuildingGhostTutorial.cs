﻿using UnityEngine;


 public class BuildingGhostTutorial : MonoBehaviour
 {
     private Transform visual;
     private PlacedObjectTypeSO placedObjectTypeSO;
     
     private void Start()
     {
         RefreshVisual();
     
         GridBuildingSystem3DTutorial.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
     }
     
     private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
     {
         RefreshVisual();
     }
     
     private void LateUpdate()
     {
         Vector3 targetPosition = GridBuildingSystem3DTutorial.Instance.GetMouseWorldSnappedPosition();
         if (TouchInputManager.Instance.HasTouchInput() || Input.GetMouseButton(0))
         {
             targetPosition.y = 3f;
             transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
     
         }
         else
         {
             targetPosition.y = -13f;
         }
     
     }
     
     private void RefreshVisual()
     {
         if (visual != null)
         {
             Destroy(visual.gameObject);
             visual = null;
         }
     
         PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem3DTutorial.Instance.GetPlacedObjectTypeSO();
     
         if (placedObjectTypeSO != null )
         {
             visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
             visual.parent = transform;
             visual.localPosition = Vector3.zero;
             visual.localEulerAngles = Vector3.zero;
             SetLayerRecursive(visual.gameObject, 11);
         }
     }
     
     private void SetLayerRecursive(GameObject targetGameObject, int layer)
     {
         targetGameObject.layer = layer;
         foreach (Transform child in targetGameObject.transform)
         {
             SetLayerRecursive(child.gameObject, layer);
         }
     }
 }
