using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGameManager : MonoBehaviour
{
    public bool pieceSelected = false;
    public GameObject selectedPiece;
    public GameObject transformControls;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
}
