using Plugins.MonoCache;
using UnityEngine;

namespace Assistant
{
    public class BotInput : MonoCache, IBotInput
    {
        public Vector2 MovementInput { get; set; }
    }
}