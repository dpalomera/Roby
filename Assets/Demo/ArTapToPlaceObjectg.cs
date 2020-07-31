using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.EventSystems;
using UnityEngine.Android;

public class ArTapToPlaceObjectg : MonoBehaviour
{
    public delegate void EventoInstanciacion();
    public static event EventoInstanciacion OnAvatarInstanciado;

    public delegate void EventoPlace();
    public static event EventoPlace OnAvatarMovido;

    public GameObject objectToPlace;
    public GameObject placementIndicator;
    private ARRaycastManager raycastManager;
    private Pose placementPos;
    private bool placementPoseIsValid = false;
    private GameObject instance = null;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.XR.ARCore.ARCorePermissionManager.RequestPermission(Permission.Microphone, (string foo, bool bar) =>
        {

        });
        

        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (placementPoseIsValid && Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                PlaceObject();
            }

        }
    }

    private void PlaceObject()
    {
        
        if(instance is null)
        {
            instance = Instantiate(objectToPlace, placementPos.position, placementPos.rotation);
            OnAvatarInstanciado?.Invoke();
        }
        else
        {
            
            instance.transform.SetPositionAndRotation(placementIndicator.transform.position, placementIndicator.transform.rotation);
        }
        instance.transform.Rotate(0, 180, 0);
        OnAvatarMovido?.Invoke();


    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            //placementPos.rotation.y += 180;
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPos.position, placementPos.rotation);
            
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;

        //light.transform.position = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        if (placementPoseIsValid)
        {
            placementPos = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearring = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPos.rotation = Quaternion.LookRotation(cameraBearring);

        }
    }
}
