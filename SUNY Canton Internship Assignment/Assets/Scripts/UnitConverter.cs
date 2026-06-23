using UnityEngine;

public static class UnitConverter
{
    private const float MPH_TO_UNITY = 0.2f; //30MPH = 6 Unity units/second
    private const float UNITY_TO_MPH = 5f; //6 Unity units/second = 30 MPH
    private const float FEET_TO_UNITY = 1.215f; //Feet to Unity units (distance)
    private const float UNITY_TO_FEET = 0.823f; //Unity units to feet

    public static float MphToUnityVelocity(float mph)
    {
        return mph * MPH_TO_UNITY;
    }

    public static float UnityVelocityToMph(float unityVelocity)
    {
        return unityVelocity * UNITY_TO_MPH;
    }

    public static Vector3 UnityVelocityToMph(Vector3 unityVelocity)
    {
        return unityVelocity * UNITY_TO_MPH;
    }
    public static float FeetToUnity(float feet)
    {
        return feet * FEET_TO_UNITY;
    }

    public static float UnityToFeet(float unityDistance)
    {
        return unityDistance * UNITY_TO_FEET;
    }

    public static Vector3 UnityToFeet(Vector3 unityPosition)
    {
        return unityPosition * UNITY_TO_FEET;
    }
    public static float NewtonsToPoundForce(float newtons)
    {
        return newtons * 0.224809f;
    }

    public static Vector3 NewtonsToPoundForce(Vector3 newtons)
    {
        return newtons * 0.224809f;
    }

    public static float KgToPounds(float kg)
    {
        return kg * 2.20462f;
    }

    public static float PoundsToKg(float pounds)
    {
        return pounds * 0.453592f;
    }
}
