using System.Collections.Generic;
using UnityEngine;

namespace LizardKit.GeckoPatreonKit
{
    [CreateAssetMenu(fileName = "PatreonAuthPackage", menuName = "LizardKit/Patreon/AuthPackage")]

public class PatreonAuthPackage : ScriptableObject
    {
        public string gameSlug;
        public List<string> patreonFeatures;
        public string safetyKey;
        [TextArea(5,30)]
        public string unlockBenefitDescription = "This unlocks bonus content across all our games.";
        public string freeUserBenefits = "It seems you are not currently supporting the LizardFactory team, so there are no benefits.";
        public string paidUserBenefits = "Your bonus content has been unlocked.";
    }
}