using Godot;
using System;

public static class Extensions
{
    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public static Vector3 Flatten(this Vector3 vector3)
    {
        return new Vector3(vector3.x, 0, vector3.z);
    }
}
