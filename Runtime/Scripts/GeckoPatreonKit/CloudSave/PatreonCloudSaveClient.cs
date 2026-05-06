using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using LizardKit.Scaffolding;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace GeckoPatreonKit.CloudSave
{
    public class PatreonCloudSaveClient : BaseManager<PatreonCloudSaveClient>
    {
        private const string BaseUrl = "https://connect.lewdlizard.com";
        [Header("API")]
        public PatreonAuthPackage package;
        
        public UnityEvent<string> onSaveFileFound;

        #region DATA_SCHEMAS

        [Serializable]
        public class CloudSaveResponse
        {
            public bool success;
            public string save_data;
        }


        [Serializable]
        private class StoreRequest
        {
            public string patreon_id;
            public string client_uuid;
            public long timestamp;
            public string nonce;
            public string signature;
            public string save_data;
        }

        [Serializable]
        private class RecoverRequest
        {
            public string patreon_id;
            public string client_uuid;
            public long timestamp;
            public string nonce;
            public string signature;
        }

        [Serializable]
        private class ChallengeResponse
        {
            public string nonce;
            public string cache_key;
            public bool stored;
        }
        

        #endregion

        public void StoreSave(string saveData)
        {
            if (!PatreonAuthenticator.Connected()) return;
            StartCoroutine(StoreSaveRoutine(saveData));
        }

        public void RecoverSave(Action<string> onLoaded)
        {
            if (!PatreonAuthenticator.Connected()) return;
            StartCoroutine(RecoverSaveRoutine(onLoaded));
        }


        private IEnumerator StoreSaveRoutine(string saveData)
        {
            var clientUuid = PatreonAuthenticator.GetPlayerIdentifier();
            var patreonId = PatreonAuthenticator.GetPatreonId();
            yield return GetNonce(clientUuid, nonce =>
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                var payload = new StoreRequest
                {
                    patreon_id = patreonId,
                    client_uuid = clientUuid,
                    timestamp = timestamp,
                    nonce = nonce,
                    signature = BuildSignature(clientUuid, patreonId, package.gameSlug, timestamp, nonce,saveData),
                    save_data = saveData
                };

                StartCoroutine(PostJson(
                    $"{BaseUrl}/connect/cloud/store/{package.gameSlug}",
                    JsonUtility.ToJson(payload),
                    response => { Log("Cloud save stored: " + response); }
                ));
            });
        }
        
        public void CheckForSaveFile()
        {
            StartCoroutine(RecoverSaveRoutine(s => onSaveFileFound.Invoke(s)));
        }

        private IEnumerator RecoverSaveRoutine(Action<string> onLoaded)
        {
            var clientUuid = PatreonAuthenticator.GetPlayerIdentifier();
            var patreonId = PatreonAuthenticator.GetPatreonId();
            yield return GetNonce(clientUuid, nonce =>
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                var payload = new RecoverRequest
                {
                    patreon_id = patreonId,
                    client_uuid = clientUuid,
                    timestamp = timestamp,
                    nonce = nonce,
                    signature = BuildSignature(clientUuid, patreonId, package.gameSlug, timestamp, nonce,null)
                };

                StartCoroutine(PostJson(
                    $"{BaseUrl}/connect/cloud/recover/{package.gameSlug}",
                    JsonUtility.ToJson(payload),
                    response =>
                    {
                        var result = JsonUtility.FromJson<CloudSaveResponse>(response);
                        Log("Response: "+response);
                        onLoaded?.Invoke(result.save_data);
                    }
                ));
            });
        }

        private IEnumerator GetNonce(string uuid, Action<string> onNonce)
        {
            var url = $"{BaseUrl}/api/connect/challenge/{uuid}";

            using var request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                LogError("Nonce request failed: " + request.error);
                yield break;
            }

            var response = JsonUtility.FromJson<ChallengeResponse>(request.downloadHandler.text);

            if (response == null || string.IsNullOrEmpty(response.nonce))
            {
                LogError("Invalid nonce response: " + request.downloadHandler.text);
                yield break;
            }

            onNonce?.Invoke(response.nonce);
        }

        private IEnumerator PostJson(string url, string json, Action<string> onSuccess)
        {
            using var request = new UnityWebRequest(url, "POST");

            var bodyRaw = Encoding.UTF8.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                LogError($"Request failed: {request.responseCode} {request.error}\n{request.downloadHandler.text}");
                yield break;
            }

            onSuccess?.Invoke(request.downloadHandler.text);
        }

        private string BuildSignature(
            string clientUuid,
            string patreonId,
            string gameKey,
            long timestamp,
            string nonce,
            string gamedata
        )
        {
            var payload = $"{clientUuid}|{patreonId}|{gameKey}|{timestamp}|{nonce}|{gamedata}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(package.safetyKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));

            var builder = new StringBuilder();

            foreach (var b in hash) builder.Append(b.ToString("x2"));

            return builder.ToString();
        }
    }
}