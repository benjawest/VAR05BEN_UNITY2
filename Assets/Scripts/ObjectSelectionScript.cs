using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is attached to buttons in the menu that allow the user to select an object to place in the scene

public class ObjectSelectionScript : MonoBehaviour
{
    public GameObject objectPrefab;
    public RayCastManager raycastmanager;
    private ARGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Find the ARGameManager script
        gameManager = GameObject.Find("XR Origin").GetComponent<ARGameManager>();
        raycastmanager = GameObject.Find("XR Origin").GetComponent<RayCastManager>();
        
        // Get the Button component attached to the TMP button element
        Button button = GetComponent<Button>();

        // Add a click event listener
        button.onClick.AddListener(selectObject);
    }

    void selectObject()
    {
        raycastmanager.LoadPrefabToSpawn(objectPrefab);
        gameManager.HideMenu();
    }
}
