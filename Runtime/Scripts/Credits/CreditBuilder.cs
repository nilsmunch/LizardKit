using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LizardKit.DebugButton;
using TMPro;
using UnityEngine;

namespace TricksForTreats.Credits
{
    public class CreditBuilder : MonoBehaviour
    {
        public TMP_Text TitleLabel;
        public TMP_Text NamesLabel;
        public RectTransform ScrollSpace;

        protected virtual List<CreditEntry> GetCredits()
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
            var displayHeight = NamesLabel.preferredHeight + 70;
            ScrollSpace.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, displayHeight);
        }
        private void Awake()
        {
            BuildCredits();
        }

        [Button]
        protected virtual void BuildCredits()
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
            TitleLabel.text = creditString;
            var namesString = GetCredits().Aggregate("", (current, credit) => current + $"{credit.Name}\n");
            NamesLabel.text = namesString.Trim();
        }
    }
}