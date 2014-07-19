using System;
using System.Collections;
using UnityEngine;

namespace Fangtang.Utils
{
    public static class AudioSourceExtensions
    {
        public static void PlayClip(this AudioSource audioSource, AudioClip audioClip, bool isloop = false)
        {
            if (audioSource.enabled)
            {
                audioSource.clip = audioClip;
                audioSource.loop = isloop;
                audioSource.Play();
            }
        }

        public static IEnumerator PlayClip(this AudioSource audioSource,
            AudioClip audioClip, Action onComplete)
        {
            if (audioSource.enabled)
            {
                audioSource.PlayClip(audioClip);

                while (audioSource.isPlaying)
                    yield return null;

                onComplete();
            }
        }

        public static void PlayRandomClip(this AudioSource audioSource, AudioClip[] clips)
        {
            if (audioSource.enabled)
            {
                int clipIndex = UnityEngine.Random.Range(0, clips.Length);
                audioSource.PlayClip(clips[clipIndex]);
            }
        }

        public static IEnumerator FadeOut(this AudioSource audioSource,
            AudioClip audioClip, float duration, Action onComplete)
        {
            if (audioSource.enabled)
            {
                audioSource.PlayClip(audioClip);

                var startingVolume = audioSource.volume;

                // fade out the volume
                while (audioSource.volume > 0.0f)
                {
                    audioSource.volume -= Time.deltaTime * startingVolume / duration;
                    yield return null;
                }

                if (onComplete != null)
                {
                    onComplete();
                }
            }
        }
    }
}