using System.Collections.Generic;
using System.Linq;
using LizardKit.DebugButton;
using LizardKit.Scaffolding;
using LizardKit.Settings;
using UnityEngine;

namespace LizardKit.Audio
{
    public class MusicManager : BaseManager<MusicManager>
    {
        public List<AudioClip> Tracks = new();
        public AudioSource Source;

        protected override void Awake()
        {
            base.Awake();
            Source.loop = true;
            MusicVolumeSlider.GlobalPreload();
            PlayNewTrack();
        }

        [Button]
        public void PlayNewTrack()
        {
            var candidates = Tracks.Where(a => a != Source.clip).ToArray();
            if (candidates.Length == 0)
            {
                Source.Play();
                return;
            }

            var winner = candidates[UnityEngine.Random.Range(0,candidates.Length)];
            Source.clip = winner;
            Source.Play();
        }
    }
}
