using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySE(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
