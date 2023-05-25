using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Unity.VisualScripting;


[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARGameManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class RayCastManager : MonoBehaviour
{

    public Camera cam;
    ARGameManager gameManager;
    ARRaycastManager raycastManager;
    ARPlaneManager planeManager;

    // Prefab to instantiate on touch
    public GameObject objPrefab;

    // Vector 3 for spawn rotation set to 0,0,0
    public Quaternion spawnRotation = Quaternion.identity;

    // Stores the position of the click/touch
    Vector2 pos;

    // List to hold ARRaycast hits
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool isMenuOpen = true;
    private bool isTouchBegan = false; // tracks if the touch has already been registered

    private void Awake()
    {
        gameManager = GetComponent<ARGameManager>();
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void Start()
    {
        if (gameManager != null)
        {
            isMenuOpen = gameManager.menuParent.activeSelf;
        }
    }

    private void Update()
    {
       
        // Check if the user has touched the screen
        #if UNITY_EDITOR
        CheckForInput();
        #else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Retrieve the first touch

            if (touch.phase == TouchPhase.Began)
            {
                if (!isTouchBegan)
                {
                    // Code to run only when a touch has begun for the first time
                    CheckForInput();
                    Debug.Log("Touch has begun");
                    isTouchBegan = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (isTouchBegan)
                {
                    // Reset the touch state when the touch ends
                    isTouchBegan = false;
                }
            }
        }
        #endif
    }

    private void CheckForInput()
    {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) == false)
                return;

            pos = Input.mousePosition;
#else
            if (Input.touchCount == 0)
                return;

            pos = Input.GetTouch(0).position;
#endif  
        // Check if the pointer is over any UI elements
        bool isOverUI = pos.IsPointOverUIObject();
        if (isOverUI)
        {
            // Debug line
            Debug.Log("Pointer is over UI element");
            // UI element is being interacted with, so block further actions
            return;
        }

        Debug.Log($"Clicked: {pos}");
        Ray ray = cam.ScreenPointToRay(pos);
        Pose hitPose; // varaible to store hit position and rotation

        // If a piece is selected, check if the user has clicked on a plane to move it to
        if (gameManager.pieceSelected)
        {
            Debug.Log("Piece Selected, Checking Input");
            // Check if the user has clicked on a selectable object, switch to that object if so
            if (CheckIfSelectable(ray))
            {
                Debug.Log("Selectable object hit");
                return;
            }
            // Check if the user has clicked on a plane
            else if(isARPlane(ray, out hitPose))
            {
                Debug.Log("Plane Hit, Moving Piece");
                gameManager.selectedPiece.transform.position = hitPose.position;
            }
        }
        // If the ray is selectable, otherwise check for plane to spawn on
        else if (CheckIfSelectable(ray))
        {
            Debug.Log("Selectable object hit");
            return;
        }
        else
        {
            Debug.Log("No Selectable Obj, Checking for Plane");
            SpawnOnPlane(ray);
        }
        
    }

    // Spawns the selected object on the plane, if possible
    private void SpawnOnPlane(Ray ray)
    {
        Pose hitPose;
        if (isARPlane(ray, out hitPose))
        {
            Debug.Log("Spawning on Plane");

            GameObject newPiece = Instantiate(objPrefab, hitPose.position, objPrefab.transform.rotation);

            // Hide the menu if it's open
            if (gameManager.isMenuOpen == true)
            {
                gameManager.HideMenu();
            }

            // if gameManager is not null, set the selected object
            if (gameManager != null)
            {
                gameManager.SetSelectedObject(newPiece);
            }
        }
    }

    // Checks if the raycast hit an ARPlane, returns true if it did, and the hitInfo
    private bool isARPlane(Ray ray, out Pose hitInfo)
    {
        hitInfo = new Pose();
        
        // Check if the user has clicked on a plane
        if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            hitInfo = hits[0].pose;

            if (hitInfo != null)
            {
                // Get the plane that was hit
                ARPlane hitPlane = planeManager.GetPlane(hits[0].trackableId);
                if (hitPlane.normal == Vector3.up)
                {
                    Debug.Log("Raycast hit upwards facing plane: " + hitPlane);
                    return true;
                }
                else
                {
                    Debug.Log("Raycast hit non-upwards facing plane: " + hitPlane);
                    return false;
                }

            }
        }
        return false;
    }

    private bool CheckIfSelectable(Ray ray)
    {
        // Check if the user has clicked on a selectable object
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Check if the hit object has the "Spawned" tag
            if (hitInfo.collider.CompareTag("Spawned"))
            {
                // The user has hit a GameObject with the "Spawned" tag
                GameObject objectToSelect = hitInfo.collider.gameObject;

                // Select the object
                Debug.Log("Selecting New Object");
                gameManager.SetSelectedObject(objectToSelect);
                return true;
            }
            
        }
        return false;
    }


    public void LoadPrefabToSpawn(GameObject obj)
    {
        objPrefab = obj;
    }
}