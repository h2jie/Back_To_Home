using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get { return instance; }
    }

    public string ResourceDir = "Audios";

    void Awake()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;

    }

    #region Audio
    private AudioSource audioSource;

    public bool Mute
    {
        get { return audioSource.mute; }
        set
        {
            audioSource.mute = value;
        }
    }

    public float SoundVolume //0-1
    {
        get { return audioSource.volume; }
        set { audioSource.volume = value; }
    }

    public void PlaySound(string name)
    {
        string path = ResourceDir + "/" + name;
        AudioClip ac = Resources.Load<AudioClip>(path);
        audioSource.clip = ac;
        audioSource.PlayOneShot(ac);
    }

    public void StopSound()
    {
        audioSource.clip = null;
        audioSource.Stop();
    }
    #endregion

}
