using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public const string AUDIO_MANAGER_GAME_OBJECT_TAG = "AudioManager";

    [Header("AudioMixer")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("AudioSources")]
    [SerializeField] private AudioSource _sFXSource;

    [Header("Sounds")]
    [SerializeField] public AudioClip PopSound;
    [SerializeField] public AudioClip SwapSound;
    [SerializeField] public AudioClip ShootSound;

    private void Awake()
    {
        _audioMixer.SetFloat("sfx", Mathf.Log10(1)*20);
    }

    public void PlaySFX(AudioClip clip)
    {
        _sFXSource.PlayOneShot(clip, 1f);
    }

    public void MuteSFX()
    {
        _sFXSource.mute = true;
    }

    public void UnMuteSFX()
    {
        _sFXSource.mute = false;
    }
}
