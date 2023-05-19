using UnityEngine;

public class ClickCreateObject : MonoBehaviour
{
    public Camera cam;
    public GameObject objPrefab;

    // Vector 3 for spawn rotation set to 0,0,0
    public Quaternion spawnRotation = Quaternion.identity;


    //private Quaternion planarRotation1 = Quaternion.Euler(0f, 90f, 0f);

    // Define the planar rotations
    private Quaternion planarRotation2 = Quaternion.Euler(0f, -90f, 0f);

    private void Update()
    {
        Vector2 pos;

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

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Check if the hitInfo is a planar surface
            if (IsPlanePlanar(hitInfo.transform.rotation))
            {
                // print to debug, click hit was planar
                Debug.Log("Planar surface hit");

                Instantiate(objPrefab, hitInfo.point + new Vector3(0, 0.2f, 0), spawnRotation);

                Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1);
            }
            else
            {
                return;
            }

            
        }
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
}