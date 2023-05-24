using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class ARGameManager : MonoBehaviour
{
    public bool pieceSelected = false;
    public GameObject selectedPiece;
    public GameObject transformControls;
    public GameObject closeButton;
    public GameObject menuButton;
    public GameObject menuParent;
    public GameObject debugManager;

    public bool isMenuOpen = false;

    // List to hold all the furniture we create
    List<GameObject> Furniture = new List<GameObject>();

    RayCastManager rayCastManager;
    
    // Start is called before the first frame update
    void Awake()
    {
        rayCastManager = GetComponent<RayCastManager>();
        if (rayCastManager == null)
        {
            Debug.LogError("RayCastManager is null");
        }

        // if menuParent is not null, check if it is active. Set isMenuOpen to true if it is
        if(menuParent != null)
        {
            if(menuParent.activeSelf)
            {
                isMenuOpen = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pieceSelected)
        {
            selectedPiece.GetComponent<BoxColliderVisualizer>().EnableVisibility(true);
        }
    }

    public void SetSelectedObject(GameObject newSelection)
    {
        // if the current selection is not null, disable the box collider visualizer
        if(selectedPiece != null)
        {
            selectedPiece.GetComponent<BoxColliderVisualizer>().EnableVisibility(false);
        }

        selectedPiece = newSelection;
        pieceSelected = true;
        selectedPiece.GetComponent<BoxColliderVisualizer>().EnableVisibility(true);

        // if the transformControls are not null, enable them
        if(transformControls != null)
        {
            transformControls.SetActive(true);
        }

        isMenuOpen = true;
    }

    public void ConfirmSelectedObject()
    {
        // check if the selected object is already in the list Furniture
        if(Furniture.Contains(selectedPiece))
        {
            // if it is, do nothing
            ClearSelectedObject();
            return;
        }
        else
        {
            // else, add it to the list
            Furniture.Add(selectedPiece);
            ClearSelectedObject();
        }
        isMenuOpen = false;
        
    }

    public void ClearSelectedObject()
    {
        // if the current selection is not null, disable the box collider visualizer
        if(selectedPiece != null)
        {
            selectedPiece.GetComponent<BoxColliderVisualizer>().EnableVisibility(false);
        }

        selectedPiece = null;
        pieceSelected = false;

        // if the transformControls are not null, disable them
        if(transformControls != null)
        {
            transformControls.SetActive(false);
        }
    }

    public void RemoveSelectedOjbect()
    {
          if(selectedPiece != null)
        {
            var toBeDeleted = selectedPiece;
            ClearSelectedObject();
            Destroy(toBeDeleted);
        }
    }

    public void ShowMenu()
    {
        menuButton.SetActive(false);
        closeButton.SetActive(true);
        menuParent.SetActive(true);
        isMenuOpen = true;
        rayCastManager.isMenuOpen = true;
    }

    public void HideMenu()
    {
        menuButton.SetActive(true);
        closeButton.SetActive(false);
        menuParent.SetActive(false);
        isMenuOpen = false;
        rayCastManager.isMenuOpen = false;
    }

    public void ClearAllObjects()
    {
        foreach(GameObject obj in Furniture)
        {
            Destroy(obj);
        }
        Furniture.Clear();

       
    }



    public void AddObject(GameObject obj)
    {
        Furniture.Add(obj);
    }

    public void ToggleDebugManager()
    {
        if(debugManager != null)
        {
            // Toggle the DebugManager's visibility
            debugManager.SetActive(!debugManager.activeSelf);
        }
    }

    
}
