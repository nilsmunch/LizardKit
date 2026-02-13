using System.Collections.Generic;
using LizardKit.DebugButton;
using LizardKit.Populator;
using UnityEngine;

namespace LizardKit.Samples.Scripts
{
    public class PopulatorExample : MonoBehaviour
    {
        [PopulatorButton]
        public List<Actor> ActorPool = new();

        [Button]
        public void Hello()
        {
            Debug.Log("Hello");
        }

    }

    [CreateAssetMenu]
    public class Actor : ScriptableObject
    {
    }
}
