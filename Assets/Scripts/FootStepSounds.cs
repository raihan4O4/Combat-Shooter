using UnityEngine;

public class FootStepSounds : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Footstep sound clips")]
    public AudioClip[] StepAudioClips;

    private AudioClip GetAudioClip()
    {
        return StepAudioClips[Random.Range(0, StepAudioClips.Length)];
    }

    private void Step()
    {
        AudioClip clip = GetAudioClip();
        audioSource.PlayOneShot(clip);
    }
}
