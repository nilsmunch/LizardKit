using System.Collections.Generic;
using System.Linq;
using LizardKit.DebugButton;
using LizardKit.Scaffolding;
using LizardKit.Settings;
using UnityEngine;

namespace LizardKit.Audio
{
    public class SoundEffectHandler : BaseManager<SoundEffectHandler>
    {
        public List<AudioClip> clips;
        private static List<AudioSource> _sources;
        public static float Volume;

        protected override void Awake()
        {
            base.Awake();
            Volume = SfxVolumeSlider.Preload();
            _sources = GetComponents<AudioSource>().ToList();
        }

        [Button]
        public void PlayTestEffect()
        {
            PlayClipLocally("Guitar");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _sources = null;
        }

        public void PlayClipLocally(string clipname, float jiggle = 0.1f)
        {
            var clip = clips.FirstOrDefault(a => a.name == clipname);
            var jigglePitch = 1f + (Random.Range(-100, 100) * (jiggle / 100f));
            foreach (var player in _sources)
            {
                if (player.isPlaying) continue;
                player.pitch = jigglePitch;
                player.volume = Volume;
                player.clip = clip;
                player.Play();
                return;
            }
        }

        public static void PlayClip(AudioClip clip, float jiggle = 0.1f)
        {
            if (_sources == null) return;
            var jigglePitch = 1f + (Random.Range(-100, 100) * (jiggle / 100f));
            foreach (var player in _sources)
            {
                if (player.isPlaying) continue;
                player.pitch = jigglePitch;
                player.clip = clip;
                player.volume = Volume;
                player.Play();
                return;
            }
        }
        public static void Play(string clipname)
        {
            if (!Instance) return;
            Instance.PlayClipLocally(clipname);
        }
        public static void PlayClean(string clipname)
        {
            if (!Instance) return;
            var clip = Instance.clips.FirstOrDefault(a => a.name == clipname);
            foreach (var player in _sources)
            {
                if (player.isPlaying) continue;
                player.clip = clip;
                player.volume = Volume;
                player.pitch = 1f;
                player.Play();
                return;
            }
        }

    }
}
