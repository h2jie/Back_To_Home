using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance
    {
        get { return instance; }
    }

    public string ResourceDir = "Audios";

    void Awake()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        //audioSourceAUdio = GetComponent<AudioSource>();
    }

    #region Music
    private AudioSource audioSource;

    public bool Mute
    {
        get { return audioSource.mute; }
        set
        {
            audioSource.mute = value;
        }
    }

    public float BGVolume //0-1
    {
        get { return audioSource.volume; }
        set { audioSource.volume = value; }
    }

    public void PlayBGM(string name)
    {
        string path = ResourceDir + "/" + name;
        AudioClip ac = Resources.Load<AudioClip>(path);
        audioSource.clip = ac;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.clip = null;
        audioSource.Stop();
    }
    #endregion

}
