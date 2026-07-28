using UnityEngine;

namespace Player.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerAudio : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxSource;      // one-shot: jump, land
        [SerializeField] private AudioSource footstepSource; // looping: footstep walk/run

        [Header("One-shot Clips")]
        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private AudioClip landClip;

        [Header("Footstep Clips")]
        [SerializeField] private AudioClip walkFootstepClip;
        [SerializeField] private AudioClip runFootstepClip;

        private void Reset()
        {
            sfxSource = GetComponent<AudioSource>();
        }

        public void PlayJumpSound() => PlayOneShot(jumpClip);

        public void PlayLandSound() => PlayOneShot(landClip);

        public void PlayWalkFootsteps() => StartFootstepLoop(walkFootstepClip);

        public void PlayRunFootsteps() => StartFootstepLoop(runFootstepClip);

        public void StopFootsteps()
        {
            if (footstepSource != null && footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }

        private void PlayOneShot(AudioClip clip)
        {
            if (sfxSource == null || clip == null) return;
            sfxSource.PlayOneShot(clip);
        }

        private void StartFootstepLoop(AudioClip clip)
        {
            if (footstepSource == null || clip == null) return;

            if (footstepSource.clip == clip && footstepSource.isPlaying) return;

            footstepSource.clip = clip;
            footstepSource.loop = true;
            footstepSource.Play();
        }
    }
}
