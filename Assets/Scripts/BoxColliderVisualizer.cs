using UnityEngine;

// Requires a BoxCollider component to be attached to the same object
[RequireComponent(typeof(BoxCollider))]
public class BoxColliderVisualizer : MonoBehaviour
{
    public Mesh customMesh;
    public Material customMaterial;

    private BoxCollider boxCollider;
    private GameObject visualizer;

   

    void Awake()
    {
        // Get the BoxCollider component attached to the object
        boxCollider = GetComponent<BoxCollider>();

        // Create a visualizer game object
        visualizer = new GameObject("BoxColliderVisualizer");
        // Make the visualizer a child of the object
        visualizer.transform.parent = transform;

        // Add a mesh filter component
        MeshFilter meshFilter = visualizer.AddComponent<MeshFilter>();

        // Assign the custom mesh to the mesh filter
        meshFilter.mesh = customMesh;

        // Add a mesh renderer component and assign the custom material
        MeshRenderer meshRenderer = visualizer.AddComponent<MeshRenderer>();
        meshRenderer.material = customMaterial;

        // Position and scale the visualizer to match the box collider
        visualizer.transform.position = boxCollider.center + transform.position;
        visualizer.transform.rotation = transform.rotation;
        visualizer.transform.localScale = boxCollider.size;
    }

    public void EnableVisibility(bool isVisible)
    {
        visualizer.SetActive(isVisible);
    }

}
