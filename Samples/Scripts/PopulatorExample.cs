using System.Collections.Generic;
using LizardKit.DebugButton;
using LizardKit.Populator;
using UnityEngine;

namespace LizardKit.Samples.Scripts
{
    public class PopulatorExample : MonoBehaviour
    {
        [PopulatorButton]
        public List<PopulatorExampleObject> ItemPool = new();

        [Button]
        public void Hello()
        {
            Debug.Log("Hello");
        }

    }

    [CreateAssetMenu]
    public class PopulatorExampleObject : ScriptableObject
    {
    }
}
