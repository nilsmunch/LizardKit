using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LizardKit.DebugButton;
using TMPro;
using UnityEngine;

namespace LizardKit.Credits
{
    public class CreditBuilder : MonoBehaviour
    {
        public TMP_Text titleLabel;
        public TMP_Text namesLabel;
        public RectTransform scrollSpace;

        private List<CreditEntry> GetCredits()
        {
            var credits = new List<CreditEntry>();
            return credits;
        }

        private void OnEnable()
        {
            StartCoroutine(BuildRect());
        }

        private IEnumerator BuildRect()
        {
            yield return new WaitForEndOfFrame();
            var displayHeight = namesLabel.preferredHeight + 70;
            scrollSpace.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, displayHeight);
        }
        private void Awake()
        {
            BuildCredits();
        }

        [Button]
        private void BuildCredits()
        {
            var creditString = "";
            foreach (var credit in GetCredits())
            {
                creditString += $"{credit.Title}\n";
                for (var i = 1; i < credit.Count; i++)
                {
                    creditString += "\n";
                }
            }
            titleLabel.text = creditString;
            var namesString = GetCredits().Aggregate("", (current, credit) => current + $"{credit.Name}\n");
            namesLabel.text = namesString.Trim();
        }
    }
}