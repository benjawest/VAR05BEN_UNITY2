using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;



[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class RayCastManager : MonoBehaviour
{

    public Camera cam;
    public GameObject objPrefab;

    // references to the ARRaycastManager and ARAnchorManager
    ARRaycastManager raycastManager;
    ARAnchorManager anchorManager;
    ARGameManager gameManager;

    // Prefab to instantiate on touch - Anchor prefab
    [SerializeField]
    GameObject m_Prefab;

    // Vector 3 for spawn rotation set to 0,0,0
    public Quaternion spawnRotation = Quaternion.identity;

    // Define the planar rotations
    //private Quaternion planarRotation1 = Quaternion.Euler(0f, 90f, 0f); // Facing Downwards
    private Quaternion planarRotation2 = Quaternion.Euler(0f, -90f, 0f); // Facing Upwards

    // List to hold all the hits from our raycast
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    // List to hold all the anchors we create
    List<ARAnchor> m_Anchors = new List<ARAnchor>();

    // Stores the position of the click/touch
    Vector2 pos;

    public bool isMenuOpen = false;

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
        gameManager = GetComponent<ARGameManager>();
    }

    private void Update()
    {
        // While there is no selected object and the menu is not open, perform raycast
        if (gameManager.pieceSelected == false && !isMenuOpen)
        {
            CheckForInput();
        }
        else
        {
            return;
        }

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

        Debug.Log($"Clicked: {pos}");

        Ray ray = cam.ScreenPointToRay(pos);

        // If the ray is selectable return, otherwise check for plane to spawn on
        if (CheckIfSelectable(ray))
        {
            Debug.Log("Selectable object hit");
            return;
        }
        else
        {
            Debug.Log("No Object, Checking for Plane");
            CheckIfPlane(ray);
        }
        
    }

    private void CheckIfPlane(Ray ray)
    {
        // Perform raycast using ARRaycastManager, only return hits with planes
        if (raycastManager.Raycast(ray, hits, TrackableType.AllTypes))
        {
            //ARAnchor anchor = null;

            // Get the hit  from the first hit in the hits array
            var hit = hits[0];

            if (hit.trackable is ARPlane plane)
            {
                // Check if the hitInfo is a planar surface rotated facing upwards
                if (IsPlanePlanar(hit.pose.rotation))
                {
                    // print to debug, click hit was planar
                    Debug.Log("Planar surface hit");


                    // anchor = anchorManager.AttachAnchor(plane, hit.pose);
                    GameObject newPiece = Instantiate(objPrefab, hit.pose.position, objPrefab.transform.rotation);

                    // if gameManager is not null, set the selected object
                    if (gameManager != null)
                    {
                        gameManager.SetSelectedObject(newPiece);
                    }

                    Debug.DrawLine(ray.origin, hit.pose.position, Color.red, 1);
                }
                else
                {
                    return;
                }

            }
        }
    }

    private bool CheckIfSelectable(Ray ray)
    {
        // Check if the user has clicked on a selectable object
        RaycastHit hit;
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Check if the hit object has the "Furniture" tag
            if (hitInfo.collider.CompareTag("Furniture"))
            {
                // The user has hit a GameObject with the "Furniture" tag
                Debug.Log("Hit a Furniture object!");
                GameObject furnitureObject = hitInfo.collider.gameObject;

                // Select the object
                gameManager.SetSelectedObject(furnitureObject);
                return true;
            }
            
        }
        return false;
    }

    // Function to check if a plane is planar or vertical
    private bool IsPlanePlanar(Quaternion rotation)
    {
        // Compare with planar rotations
        if (rotation.eulerAngles == planarRotation2.eulerAngles)
        {
            // Plane is planar
            return true;
        }
        else
        {
            // Plane is vertical
            return false;
        }
    }

    public void SetSelectedObject(GameObject selectedObject)
    {
        // Set the selected object prefab to the one passed in
        // You can store it in a variable or use it directly for spawning
        objPrefab = selectedObject;
    }

}