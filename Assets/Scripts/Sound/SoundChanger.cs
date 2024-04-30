using UnityEngine;

public class SoundChanger : MonoBehaviour
{
    public SoundList _enterBgm;
    public SoundList _exitBgm;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.soundManager.PlayBGM(GameManager.Instance.soundManager.GetSoundClip(_enterBgm));

    }
    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.soundManager.PlayBGM(GameManager.Instance.soundManager.GetSoundClip(_exitBgm));
    }

}
