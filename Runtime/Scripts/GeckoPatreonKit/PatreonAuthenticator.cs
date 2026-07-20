using System;
using System.Collections;
using GeckoPatreonKit;
using LizardKit.Scaffolding;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LizardKit.GeckoPatreonKit
{
    public class PatreonAuthenticator : BaseManager<PatreonAuthenticator>
    {
        public PatreonAuthPackage package; 
        private static Payload _lastPlayerPayload;
        private string _playerId;
        private const string PatreonCheckArmedKey = "PatreonCheckArmed";
        private const string PatreonPlayerIdKey = "PlayerId";

        private bool PatreonCheckArmed
        {
            get => PlayerPrefs.GetInt(PatreonCheckArmedKey, 0) == 1;
            set
            {
                PlayerPrefs.SetInt(PatreonCheckArmedKey, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        [Header("Connect UI")]
        public Button connectButton;
        public TMP_Text connectLabel;

        public UnityEvent onConnected;
        
        protected override void Awake()
        {
            base.Awake();
            _playerId = GetPlayerIdentifier();
            connectButton.onClick.AddListener(ProcessConnect);
            StartCoroutine(CheckConnectData());
        }

        private Coroutine _checkCoroutine;

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
                TryCheckAfterReturn();
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (!isPaused)
                TryCheckAfterReturn();
        }

        private void TryCheckAfterReturn()
        {
            if (!PatreonCheckArmed) return;

            if (_checkCoroutine != null)
                StopCoroutine(_checkCoroutine);

            _checkCoroutine = StartCoroutine(CheckConnectData());
        }

        public static string GetPlayerIdentifier()
        {
            if (!PlayerPrefs.HasKey(PatreonPlayerIdKey))
            {
                PlayerPrefs.SetString(PatreonPlayerIdKey, Guid.NewGuid().ToString());
                PlayerPrefs.Save();
            }

            return PlayerPrefs.GetString(PatreonPlayerIdKey, Guid.NewGuid().ToString());
        }

        public void TogglePanel()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        #region USER_CHECK
        private IEnumerator CheckConnectData()
        {
            connectLabel.text = "Checking user data...";
            yield return new WaitForSecondsRealtime(0.5f);

            Log($"client_uuid={_playerId}");
            // STEP 1: Get nonce
            string nonce;
            long timestamp;
            
            using (var nonceRequest = UnityWebRequest.Get($"https://lewdlizard.com/api/connect/challenge/{_playerId}"))
            {
                nonceRequest.downloadHandler = new DownloadHandlerBuffer();
                yield return nonceRequest.SendWebRequest();

                if (nonceRequest.result != UnityWebRequest.Result.Success)
                {
                    Log("Failed to get nonce");
                    UpdateWithResult(-1, null);
                    yield break;
                }

                Log(nonceRequest.downloadHandler.text);
                var nonceResponse = JsonUtility.FromJson<NonceResponse>(nonceRequest.downloadHandler.text);
                nonce = nonceResponse?.nonce;
                timestamp = nonceResponse?.timestamp ?? DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                if (string.IsNullOrEmpty(nonce))
                {
                    Log("Nonce missing");
                    UpdateWithResult(-1, null);
                    yield break;
                }
            }

            // STEP 2: Sign request
            string signature =
                SubscriptionSigner.CreateSignature(
                    _playerId,
                    timestamp,
                    nonce);

            // STEP 3: Verify request
            Log($"Using nonce {nonce}");
            var verifyUri = VerifyUri(_playerId, timestamp, nonce, signature);
            using var request = UnityWebRequest.Get(verifyUri);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Log($"Request failed: {request.error} {request.downloadHandler.text}");
                UpdateWithResult(-1, null);
                yield break;
            }
            Log($"{verifyUri}:: {request.downloadHandler.text}");
            if (request.downloadHandler.text.Contains("No check found"))
            {
                UpdateWithResult(-1, null);
                yield break;
            }

            var data = JsonUtility.FromJson<Root>(request.downloadHandler.text);
            if (data?.user == null)
            {
                UpdateWithResult(-1, null);
                yield break;
            }
            
            
            if (data?.payload == null)
            {
                UpdateWithResult(-1, null);
                connectLabel.text = "<color=red>BAD AUTHENTIFICATION</color>\nResponse signature invalid - possible tampering - IP logged";
                Log("Invalid response format");
                yield break;
            }

            if (!PatreonResponseVerifier.IsValidSignature(data))
            {
                UpdateWithResult(-1, null);
                connectLabel.text = "<color=red>BAD AUTHENTIFICATION</color>\nResponse signature invalid - possible tampering - IP logged";
                Log("Response signature invalid - possible tampering");
                yield break;
            }

            if (data.payload.client_uuid != _playerId)
            {
                UpdateWithResult(-1, null);
                connectLabel.text = "<color=red>BAD AUTHENTIFICATION</color>\nResponse signature invalid - possible tampering - IP logged";
                Log("Response client UUID mismatch");
                yield break;
            }

            _lastPlayerPayload = data.payload;
            UpdateWithResult(_lastPlayerPayload.tier_level, _lastPlayerPayload.user_name);
            PatreonCheckArmed = false;
            onConnected.Invoke();
        }
        #endregion

        private void UpdateWithResult(int tierLevel, string userName)
        {
            var message = tierLevel switch
            {
                -1 =>
                    "If you wish to unlock more content and support the developers, you can do so through our Patreon. "+package.unlockBenefitDescription,
                0 =>
                    "<color=green>Free User Detected</color>\nThank you for connecting your Patreon Account.\n"+package.freeUserBenefits,
                > 0 =>
                    $"<color=orange>Supporter Detected</color>\nThank you for connecting your Patreon Account.\nYou are a tier {tierLevel} supporter - Thank you so much!\n"+package.paidUserBenefits,
                _ => ""
            };
            connectLabel.text = message;
        }

        private static string ConnectUri(string gameSlug, string playerId)
        {
            return
                $"https://lewdlizard.com/connect/game/" +
                $"{UnityWebRequest.EscapeURL(gameSlug)}/" +
                $"{UnityWebRequest.EscapeURL(playerId)}";
        }

        private static string VerifyUri(
            string playerId,
            long timestamp,
            string nonce,
            string signature)
        {
            return
                $"https://lewdlizard.com/api/connect/verify/" +
                $"{UnityWebRequest.EscapeURL(playerId)}" +
                $"?timestamp={timestamp}" +
                $"&nonce={UnityWebRequest.EscapeURL(nonce)}" +
                $"&signature={UnityWebRequest.EscapeURL(signature)}";
        }

        private void ProcessConnect()
        {
            PatreonCheckArmed = true;
            Application.OpenURL(ConnectUri(package.gameSlug, _playerId));
        }

        #region JSON_PARSING
        [Serializable]
        public class Root
        {
            public User user;
            public Payload payload;
            public string signature;
        }

        [Serializable]
        public class User
        {
            public int tier_level;
            public string user_name;
        }
        
        [Serializable]
        public class SignedPayloadResponse
        {
            public Payload payload;
            public string signature;
        }

        [Serializable]
        public class Payload
        {
            public int tier_level;
            public string user_name;
            public string game;
            public string client_uuid;
            public string patreon_id;
            public long expires_at;
        }
        
        
        [Serializable]
        public class NonceResponse
        {
            public string nonce;
            public long timestamp;
        }
        #endregion

        public static bool IsPaying()
        {
            return _lastPlayerPayload is { tier_level: >= 1 };
        }
        public static bool IsMidTierOrAbove()
        {
            return _lastPlayerPayload is { tier_level: >= 2 };
        }
        public static bool IsTopTier()
        {
            return _lastPlayerPayload is { tier_level: >= 3 };
        }

        public static string GetPatreonId()
        {
            return _lastPlayerPayload?.patreon_id;
        }

        public static bool Connected()
        {
            return _lastPlayerPayload != null &&
                   !string.IsNullOrEmpty(_lastPlayerPayload.patreon_id);
        }
    }
}