using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARRaycastManager))]
public class RayCastManager : MonoBehaviour
{

    public Camera cam;
    

    // references to the gameManager
    ARGameManager gameManager;

    // Prefab to instantiate on touch
    public GameObject objPrefab;

    // Vector 3 for spawn rotation set to 0,0,0
    public Quaternion spawnRotation = Quaternion.identity;

    // Stores the position of the click/touch
    Vector2 pos;

    public bool isMenuOpen = false;
    private bool isTouchBegan = false; // tracks if the touch has already been registered

    private void Start()
    {
        gameManager = GetComponent<ARGameManager>();
    }

    private void Update()
    {
        // While there is no selected object and the menu is not open, perform raycast
        if (gameManager.pieceSelected == false && !isMenuOpen)
        {
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
                        // Example code:
                        Debug.Log("Touch has begun");

                        // Trigger your event or perform custom logic here
                        // ...

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

        // If the ray is selectable, otherwise check for plane to spawn on
        if (CheckIfSelectable(ray))
        {
            Debug.Log("Selectable object hit");
            return;
        }
        else
        {
            Debug.Log("No Selectable Obj, Checking for Plane");
            CheckIfPlane(ray);
        }
        
    }

    private void CheckIfPlane(Ray ray)
    {
        // Perform raycast using physics raycast
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
        Debug.Log("Raycast hit something");       
            
                // Check if the hitInfo is a planar surface rotated facing upwards
                if (hitInfo.normal == Vector3.up)
                {
                    // print to debug, click hit was planar
                    Debug.Log("Spawning");

                    GameObject newPiece = Instantiate(objPrefab, hitInfo.point, objPrefab.transform.rotation);

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

                    return;
                }
                else
                {
                    return;
                }

            
        }
    }

    private bool CheckIfSelectable(Ray ray)
    {
        // Check if the user has clicked on a selectable object
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Check if the hit object has the "Furniture" tag
            if (hitInfo.collider.CompareTag("Furniture"))
            {
                // The user has hit a GameObject with the "Furniture" tag
                //Debug.Log("Hit a Furniture object!");
                GameObject furnitureObject = hitInfo.collider.gameObject;

                // Select the object
                gameManager.SetSelectedObject(furnitureObject);
                return true;
            }
            
        }
        return false;
    }


    public void SetSelectedObject(GameObject selectedObject)
    {
        // Set the selected object prefab to the one passed in
        objPrefab = selectedObject;
    }

}