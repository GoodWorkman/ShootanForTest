using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;  
    private const float MinPitch = 0.85f; 
    private const float MaxPitch = 1.14f;
    
    public void PlaySfx(AudioData audioData) 
    {
        sFXPlayer.PlayOneShot(audioData.AudioClip, audioData.Volume);
    }
 
    public void PlayRandomSfx(AudioData audioData) 
    {
        sFXPlayer.pitch = Random.Range(MinPitch, MaxPitch);
        PlaySfx(audioData);
    }
    
    public void PlayRandomSfx(AudioData[] audioData) 
    {
        PlayRandomSfx(audioData[Random.Range(0, audioData.Length)]);
    }
}

[System.Serializable]  
public class AudioData 
{
    public AudioClip AudioClip;
    public float Volume;
}