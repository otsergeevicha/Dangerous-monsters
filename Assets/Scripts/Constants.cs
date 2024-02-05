using UnityEngine;

public static class Constants
{
    //saveLoad
    public const string Progress = "Progress";

    //hero
    public const int HeroMaxHealth = 100;
    public const int HeroSpeed = 4;
    
    //hero animator
    public static readonly int HeroRunHash = Animator.StringToHash("Run");
    public static readonly int HeroIdleHash = Animator.StringToHash("Idle");

    //check device
    public const string KeyboardMouse = "KeyboardMouse";
}