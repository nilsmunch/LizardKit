using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace LizardKit.Credits
{
    public static class PatreonCreditsLoader
    {
        private const string Url = "https://connect.lewdlizard.com/api/connect/credits.json";
        public static List<string> PatreonSupporters = new();
        public static List<string> PatreonVipSupporters = new();
        
        public static IEnumerator Load()
        {
            using var request = UnityWebRequest.Get(Url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load Patreon credits: " + request.error);
                yield break;
            }

            var json = request.downloadHandler.text;
            var data = JsonUtility.FromJson<PatreonCreditsResponse>(json);

            PatreonSupporters = data.normals ?? new List<string>();
            PatreonVipSupporters = data.vips ?? new List<string>();
        }
    
        [System.Serializable]
        public class PatreonCreditsResponse
        {
            public List<string> normals;
            public List<string> vips;
        }
    }
}