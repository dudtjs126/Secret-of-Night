using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public const string Matster = "Master";
    public const string BGM = "BGM";
    public const string Effect = "SFX";
    public const string MixerName = "AudioMixer";
    public const string ContainerName = "SoundRoot";

    //소리 Fade정보 설정

    public AudioMixer mixer = null;
    public Transform audioRoot = null;
    public AudioSource bgm_audio = null;
    public AudioSource[] effect_audios = null;

    public float[] effect_PlayStartTime = null;
    private int EffectChannelCount = 5;

    private float minVolume = -80.0f;
    private float maxVolume = 0.0f;

    private Dictionary<SoundList, SoundClip> soundDic = new Dictionary<SoundList, SoundClip>();
    public void Initialize()
    {
        if (this.mixer == null)
        {
            this.mixer = Resources.Load<AudioMixer>(MixerName);
        }
        if (this.audioRoot == null)
        {
            audioRoot = new GameObject(ContainerName).transform;
            audioRoot.SetParent(this.transform);
            audioRoot.localPosition = Vector3.zero;
        }
        if (bgm_audio == null)
        {
            GameObject bgm = new GameObject(BGM, typeof(AudioSource));
            bgm.transform.SetParent(audioRoot);
            this.bgm_audio = bgm.GetComponent<AudioSource>();
            this.bgm_audio.playOnAwake = false;
        }

        if (effect_audios == null || this.effect_audios.Length == 0)
        {
            effect_audios = new AudioSource[EffectChannelCount];
            effect_PlayStartTime = new float[EffectChannelCount];
            for (int i = 0; i < EffectChannelCount; i++)
            {
                effect_PlayStartTime[i] = 0.0f;

                GameObject effect = new GameObject("Effect_" + i, typeof(AudioSource));
                effect.transform.SetParent(audioRoot);
                effect_audios[i] = effect.GetComponent<AudioSource>();
                effect_audios[i].playOnAwake = false;
            }
        }
        if (this.mixer != null)
        {
            this.bgm_audio.outputAudioMixerGroup = this.mixer.FindMatchingGroups(BGM)[0];
            for (int i = 0; i < EffectChannelCount; i++)
            {
                effect_audios[i].outputAudioMixerGroup = this.mixer.FindMatchingGroups(Effect)[0];
            }
        }
        //init soundDic
        for (int i = 0; i < (int)SoundList.Max; i++)
        {
            SoundList sound = (SoundList)i;
            SoundClip clip = GameManager.Instance.dataManager.soundData.GetCopy(i);
            soundDic.Add(sound, clip);
        }
        VolumeInit();
    }
    //추후 오브젝트 풀링을 위해
    public SoundClip GetSoundClip(SoundList type)
    {
        if (soundDic.ContainsKey(type) == false)
        {
            SoundClip clip = GameManager.Instance.dataManager.soundData.GetCopy((int)type);
            soundDic.Add(type, clip);
        }
        return soundDic[type];
    }
    public void SetBGMVolume(float currentRatio)
    {
        currentRatio = Mathf.Clamp01(currentRatio);
        float volume = Mathf.Lerp(minVolume, maxVolume, currentRatio);
        this.mixer.SetFloat(BGM, volume);
        PlayerPrefs.SetFloat(BGM, volume);
    }
    public float GetBGMVolume()
    {
        if (PlayerPrefs.HasKey(BGM) == false)
        {
            return PlayerPrefs.GetFloat(BGM);
        }
        else
        {
            return maxVolume;
        }
    }
    public void SetEffectVolume(float currentRatio)
    {
        currentRatio = Mathf.Clamp01(currentRatio);
        float volume = Mathf.Lerp(minVolume, maxVolume, currentRatio);
        this.mixer.SetFloat(Effect, volume);
        PlayerPrefs.SetFloat(Effect, volume);
    }
    public float GetEffectVolume()
    {
        if (PlayerPrefs.HasKey(Effect) == false)
        {
            return PlayerPrefs.GetFloat(Effect);
        }
        else
        {
            return maxVolume;
        }
    }
    void VolumeInit()
    {
        if (this.mixer != null)
        {
            this.mixer.SetFloat(BGM, GetBGMVolume());
            this.mixer.SetFloat(Effect, GetEffectVolume());
        }
    }

    void PlayAudioSource(AudioSource source, SoundClip clip, float volume)
    {
        if (source == null || clip == null)
        {
            return;
        }
        source.Stop();
        source.clip = clip.GetClip();
        source.volume = volume;
        source.loop = clip.isLoop;
        source.pitch = clip.pitch;
        source.dopplerLevel = clip.dopplerLevel;
        source.rolloffMode = clip.rolloffMode;
        source.minDistance = clip.minDistance;
        source.maxDistance = clip.maxDistance;
        source.spatialBlend = clip.sparialBlend;
        Debug.Log("Play");
        source.Play();
    }

    void PlayAudioSourceAtPoint(SoundClip clip, Vector3 position, float volume)
    {
        bool isPlaySuccess = false;
        for (int i = 0; i < this.EffectChannelCount; i++)
        {
            if (this.effect_audios[i].isPlaying == false)
            {
                effect_audios[i].transform.position = position;
                PlayAudioSource(this.effect_audios[i], clip, clip.maxVolume);
                this.effect_PlayStartTime[i] = Time.realtimeSinceStartup;
                isPlaySuccess = true;
                break;
            }
            else if (this.effect_audios[i].clip == clip.GetClip())
            {
                this.effect_audios[i].Stop();
                effect_audios[i].transform.position = position;
                PlayAudioSource(effect_audios[i], clip, clip.maxVolume);
                this.effect_PlayStartTime[i] = Time.realtimeSinceStartup;
                isPlaySuccess = true;
                break;
            }
        }
        if (isPlaySuccess == false)
        {
            float maxTime = 0.0f;
            int selectIndex = 0;
            for (int i = 0; i < EffectChannelCount; i++)
            {
                if (this.effect_PlayStartTime[i] > maxTime)
                {
                    maxTime = this.effect_PlayStartTime[i];
                    selectIndex = i;
                }
            }
            effect_audios[selectIndex].transform.position = position;
            PlayAudioSource(this.effect_audios[selectIndex], clip, clip.maxVolume);
        }
    }

    public void PlayBGM(SoundClip clip)
    {
        PlayAudioSource(bgm_audio, clip, clip.maxVolume);
    }

    public void PlayBGM(int index)
    {
        SoundClip clip = GameManager.Instance.dataManager.soundData.GetCopy(index);
        PlayBGM(clip);
    }

    public void PlayEffectSound(SoundClip clip)
    {
        bool isPlaySuccess = false;
        for (int i = 0; i < this.EffectChannelCount; i++)
        {
            if (this.effect_audios[i].isPlaying == false)
            {
                PlayAudioSource(this.effect_audios[i], clip, clip.maxVolume);
                this.effect_PlayStartTime[i] = Time.realtimeSinceStartup;
                isPlaySuccess = true;
                break;
            }
            else if (this.effect_audios[i].clip == clip.GetClip())
            {
                this.effect_audios[i].Stop();
                PlayAudioSource(effect_audios[i], clip, clip.maxVolume);
                this.effect_PlayStartTime[i] = Time.realtimeSinceStartup;
                isPlaySuccess = true;
                break;
            }
        }
        if (isPlaySuccess == false)
        {
            float maxTime = 0.0f;
            int selectIndex = 0;
            for (int i = 0; i < EffectChannelCount; i++)
            {
                if (this.effect_PlayStartTime[i] > maxTime)
                {
                    maxTime = this.effect_PlayStartTime[i];
                    selectIndex = i;
                }
            }
            PlayAudioSource(this.effect_audios[selectIndex], clip, clip.maxVolume);

        }
    }

    public void PlayEffectSound(SoundClip clip, Vector3 position, float volume)
    {
        PlayAudioSourceAtPoint(clip, position, volume);
    }

    public void PlayOneShotEffect(int index, Vector3 position, float volume)
    {
        if (index == (int)SoundList.None)
        {
            return;
        }

        SoundClip clip = GameManager.Instance.dataManager.soundData.GetCopy(index);
        if (clip == null)
        {
            return;
        }
        PlayEffectSound(clip, position, volume);
    }
    public void PlayOneShot(SoundClip clip)
    {
        if (clip == null)
        {
            return;
        }
        switch (clip.playType)
        {
            case SoundPlayType.EFFECT:
                PlayEffectSound(clip);
                break;
            case SoundPlayType.BGM:
                PlayBGM(clip);
                break;
        }
    }

    public void Stop(bool allStop = true)
    {
        this.bgm_audio.Stop();
    }
}
