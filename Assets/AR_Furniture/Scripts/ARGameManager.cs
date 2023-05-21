using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGameManager : MonoBehaviour
{
    public bool pieceSelected = false;
    public GameObject selectedPiece;
    public GameObject transformControls;
    public GameObject closeButton;
    public GameObject menuButton;
    public GameObject menuParent;

    // List to hold all the furniture we create
    List<GameObject> Furniture = new List<GameObject>();

    RayCastManager rayCastManager;
    
    // Start is called before the first frame update
    void Start()
    {
        rayCastManager = GetComponent<RayCastManager>();
        if (rayCastManager == null)
        {
            Debug.LogError("RayCastManager is null");
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
            Destroy(selectedPiece);
            ClearSelectedObject();
        }
    }

    public void ShowMenu()
    {
        menuButton.SetActive(false);
        closeButton.SetActive(true);
        menuParent.SetActive(true);
        rayCastManager.isMenuOpen = true;
    }

    public void HideMenu()
    {
        menuButton.SetActive(true);
        closeButton.SetActive(false);
        menuParent.SetActive(false);
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


}
