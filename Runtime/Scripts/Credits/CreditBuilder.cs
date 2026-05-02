using System.Collections;
using System.Collections.Generic;
using System.Text;
using LizardKit.DebugButton;
using TMPro;
using UnityEngine;

namespace LizardKit.Credits
{
    public enum CreditLayoutStyle
    {
        TwoColumn, // Age of Empires style
        CenterStacked // Alba style
    }

    public abstract class CreditBuilder : MonoBehaviour
    {
        [Header("References")] public TMP_Text creditsLabel;
        public RectTransform scrollSpace;

        [Header("Layout")] public CreditLayoutStyle layoutStyle = CreditLayoutStyle.TwoColumn;

        [Header("Text Styling")] public string sectionTitleColor = "#D6A84A";
        public string roleColor = "#FFFFFF";
        public string nameColor = "#FFFFFF";

        public int sectionTitleSize = 32;
        public int roleSize = 24;
        public int nameSize = 26;

        [Header("Two Column")] public float roleColumnWidth = 520f;
        public float nameColumnWidth = 520f;

        [Header("Spacing")] public int linesAfterEntry = 1;
        public int linesAfterSectionTitle = 1;

        protected virtual List<CreditEntry> GetCredits()
        {
            return new List<CreditEntry>();
        }

        private void Awake()
        {
            BuildCredits();
        }

        private void OnEnable()
        {
            BuildCredits();
            StartCoroutine(BuildRect());
        }

        private IEnumerator BuildRect()
        {
            yield return new WaitForEndOfFrame();

            if (!creditsLabel || !scrollSpace)
                yield break;

            var displayHeight = creditsLabel.preferredHeight + 70;
            scrollSpace.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, displayHeight);
        }

        [Button]
        protected virtual void BuildCredits()
        {
            if (!creditsLabel)
                return;

            creditsLabel.richText = true;

            var credits = GetCredits();
            creditsLabel.text = layoutStyle switch
            {
                CreditLayoutStyle.TwoColumn => BuildTwoColumnCredits(credits),
                CreditLayoutStyle.CenterStacked => BuildCenterStackedCredits(credits),
                _ => BuildTwoColumnCredits(credits)
            };
        }

        private string BuildTwoColumnCredits(List<CreditEntry> credits)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<align=center>");
            sb.AppendLine($"<color={sectionTitleColor}><size={sectionTitleSize}>CREDITS</size></color>");
            AppendBlankLines(sb, linesAfterSectionTitle);
            sb.AppendLine("</align>");

            foreach (var credit in credits)
            {
                sb.Append("<align=center>");

                sb.Append($"<size={roleSize}><color={roleColor}>");
                sb.Append($"<pos=0%><width={roleColumnWidth}>{credit.Title}</width>");
                sb.Append("</color></size>");

                sb.Append($"<size={nameSize}><color={nameColor}>");
                sb.Append($"<pos=52%><width={nameColumnWidth}>{credit.Name}</width>");
                sb.Append("</color></size>");

                sb.AppendLine("</align>");

                AppendBlankLines(sb, Mathf.Max(credit.Count - 1, linesAfterEntry));
            }

            return sb.ToString().Trim();
        }

        private string BuildCenterStackedCredits(List<CreditEntry> credits)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<align=center>");

            foreach (var credit in credits)
            {
                if (string.IsNullOrEmpty(credit.Name.Trim())) continue;
                sb.AppendLine($"<size={roleSize}><color={roleColor}>{credit.Title}</color></size>");
                sb.AppendLine($"<size={nameSize}><color={nameColor}><b>{credit.Name}</b></color></size>");

                AppendBlankLines(sb, 1);
            }

            sb.AppendLine("</align>");

            return sb.ToString().Trim();
        }

        private static void AppendBlankLines(StringBuilder sb, int count)
        {
            for (var i = 0; i < count; i++)
                sb.AppendLine();
        }
    }
}