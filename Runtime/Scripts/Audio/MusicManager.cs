using System.Collections.Generic;
using System.Linq;
using LizardKit.DebugButton;
using LizardKit.Scaffolding;
using LizardKit.Settings;
using UnityEngine;

namespace GeckoKit.AudioKit
{
    public class MusicManager : BaseManager<MusicManager>
    {
        public List<AudioClip> Tracks = new();
        public AudioSource Source;

        protected override void Awake()
        {
            base.Awake();
            Source.volume = MusicVolumeSlider.Preload();
            PlayNewTrack();
        }

        [Button]
        public void PlayNewTrack()
        {
            var candidates = Tracks.Where(a => a != Source.clip).ToArray();
            var winner = candidates[Random.Range(0,candidates.Length)];
            Source.clip = winner;
            Source.loop = true;
            Source.Play();
        }
    }
}
