using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewAssistant", menuName = "Characters/Assistant", order = 1)]
    public class AssistantData : ScriptableObject
    {
        [Range(3, 6)] public int Speed = 3;

        [Range(5, 15)] public int SizeBasket = 5;

        [HideInInspector] public int IdleHash = Animator.StringToHash("Idle");
        [HideInInspector] public int RunHash = Animator.StringToHash("Run");
    }
}