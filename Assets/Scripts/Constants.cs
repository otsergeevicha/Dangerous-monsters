using UnityEngine;

public static class Constants
{
    //saveLoad
    public const string Progress = "Progress";

    //paths
    public const string HeroPath = "Player/Hero";
    public const string CameraPath = "Camera/MainCamera";
    
    //hero
    public const int HeroSpeed = 3;
    
    //hero animator
    public static readonly int HeroIdleHash = Animator.StringToHash("Idle");
    public static readonly int HeroRunHash = Animator.StringToHash("Run");
    public static readonly int HeroRunGunHash = Animator.StringToHash("RunGun");

    //check device
    public const string KeyboardMouse = "KeyboardMouse";
}