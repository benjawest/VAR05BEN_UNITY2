using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCreateObject : MonoBehaviour
{
    public Camera cam;
    public GameObject objPrefab;

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
            Instantiate(objPrefab, hitInfo.point + new Vector3(0, 0.2f, 0), Random.rotation);

            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1);
        }
    }
}
