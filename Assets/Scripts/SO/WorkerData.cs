using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewAssistant", menuName = "Characters/Worker", order = 1)]
    public class WorkerData : ScriptableObject
    {
        [Range(1f, 4f)] public float Speed = 1f;

        [HideInInspector] public int IdleHash = Animator.StringToHash("SitingIdle");
        [HideInInspector] public int RunHash = Animator.StringToHash("RunToWorkplace");
    }
}