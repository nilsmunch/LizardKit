using System.Collections.Generic;
using UnityEngine;

namespace GeckoPatreonKit
{
    [CreateAssetMenu(fileName = "PatreonAuthPackage", menuName = "LizardKit/Patreon/AuthPackage")]

public class PatreonAuthPackage : ScriptableObject
    {
        public string gameSlug;
        public List<string> patreonFeatures;
        public string safetyKey;
    }
}