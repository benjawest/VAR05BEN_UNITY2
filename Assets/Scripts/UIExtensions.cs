using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class UIExtensions
{
    public static bool IsPointOverUIObject(this Vector2 pos)
    {
        Debug.Log("IsPointOverUIObject?");
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return true;
        //}

        PointerEventData eventPosition = new PointerEventData(EventSystem.current);
        eventPosition.position = new Vector2(pos.x, pos.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventPosition, results);

        return results.Count > 0;
    }
}