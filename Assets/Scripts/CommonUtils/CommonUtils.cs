using UnityEngine;

public static class CommonUtils
{
    public static float Epsilon = 0.001f;

    public static Vector2 GetMouseWorldPosition(Camera camera, Vector2 mousePosition)
    {
        Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(mousePosition);
        return new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
    }

    public static void CheckFieldNotNullAndTryToSet<T>(ref T field, string fieldName) where T : Object
    {
        if (field == null)
        {
            Debug.LogWarning(fieldName + " isn't set, will be tried to set automatically");
            field = Object.FindObjectOfType<T>();

            Debug.Assert(field, fieldName + " cannot be set automatically");
        }
    }

    public static void CheckMainCameraNotNullAndTryToSet(ref Camera mainCamera)
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera isn't set, will be tried to set automatically");
            mainCamera = Camera.main;

            Debug.Assert(mainCamera, "Main Camera cannot be set automatically");
        }
    }

    public static void CheckFieldNotNull<T>(T field, string fieldName) where T : Object
    {
        Debug.Assert(field, fieldName + " isn't set");
    }
}
