using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonUtils
{
    public static bool IsOutOfScreenBorders(Vector2 position)
    {
        if (position.x < 0 || position.x > Screen.width || position.y < 0 || position.y > Screen.height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Quaternion GetRotationBetweenTwoScreenPoints(Vector2 from, Vector2 to)
    {
        var direction = to - from;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public static Vector2 GetRandomVector2(float angle, float angleMin)
    {
        float random = Random.value * angle + angleMin;
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }
}
