using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOUND = CESCO.SOUND;
using SCREEN = CESCO.SCREEN;
using TOOL = CESCO.TOOL;
using CESCO;
using Unity.VisualScripting;
using UnityEngine.Networking.Types;

public class AudioData
{
    public float BGMVolume;
    public float EffectVolume;
    public float BugVolume;
}

public class SoundManager : MonoBehaviour
{
    private AudioSource BGMSound;
    public AudioSource BGM
    {
        get { return BGMSound; }
    }
    private AudioSource EffectSound;
    public AudioSource Effect
    {
        get { return EffectSound; }
    }
    private AudioSource UIEffectSound;
    public AudioSource UIEffect
    {
        get { return UIEffectSound; }
    }

    public AudioClip[] BGMSounds;
    public AudioClip[] EffectSounds;
    public AudioClip[] UIEffectSounds;
    public AudioClip ruleBGM;

    private List<AudioSource> Sounds;

    private void Awake()
    {
        // 오디오 재생 AudioSource
        BGMSound = GameObject.Find("BGM").GetComponent<AudioSource>();
        EffectSound = GameObject.Find("Effect").GetComponent<AudioSource>();
        UIEffectSound = GameObject.Find("UIEffect").GetComponent<AudioSource>();

        Sounds = new List<AudioSource>();
        Sounds.Insert((int)SOUND.BGM, BGMSound);
        Sounds.Insert((int)SOUND.EFFECT, EffectSound);
        Sounds.Insert((int)SOUND.UI_EFFECT, EffectSound);

        BGMPlay();
    }

    public void BGMPlay()
    {
        BGMSound.clip = BGMSounds[0];
        BGMSound.loop = true;
        BGMSound.Play();
    }

    public void BGMPlay(SCREEN screen)
    {
        BGMSound.clip = BGMSounds[(int)screen];
        BGMSound.loop = true;
        BGMSound.Play();
    }
    public void BGMPlay(SCREEN screen, bool isLoop)
    {
        BGMSound.clip = BGMSounds[(int)screen];
        BGMSound.loop = isLoop;
        BGMSound.Play();
    }
    public void BGMPlay(SCREEN screen, float delay)
    {
        BGMSound.clip = BGMSounds[(int)screen];
        BGMSound.loop = true;
        BGMSound.Play();
        BGMSound.PlayDelayed(delay);
    }
    public void BGMPlay(SCREEN screen, bool isLoop, float delay)
    {
        BGMSound.clip = BGMSounds[(int)screen];
        BGMSound.loop = isLoop;
        BGMSound.Play();
        BGMSound.PlayDelayed(delay);
    }
    public void RandomBGMPlay()
    {
        int bgmIndex = Random.Range(1, BGMSounds.Length);
        BGMSound.clip = BGMSounds[bgmIndex];
        BGMSound.loop = true;
        BGMSound.Play();
    }

    public void BGMStop()
    {
        BGMSound.Stop();
    }

    public void PlayRule()
    {
        BGMSound.clip = ruleBGM;
        BGMSound.loop = true;
        BGMSound.Play();
    }

    public void ChangeVolume(SOUND sound, float volume)
    {
        if (sound == SOUND.BUG_EFFECT)
        {
            List<Bug>[] bugs = GameManager.instance.prefabManager.BugPool;
            for (int i = 0; i < bugs.Length; ++i)
            {
                foreach (Bug bug in bugs[i])
                {
                    bug.SetAudioVolume(volume);
                }
            }
            return;
        }
        else if (sound == SOUND.BGM)
        {
            Sounds[(int)sound].volume = volume;
        }
        else
        {
            // Effect or UI Effect
            Sounds[(int)SOUND.EFFECT].volume = volume;
            Sounds[(int)SOUND.UI_EFFECT].volume = volume;
        }
    }

    public void AudioSetting(AudioData data)
    {
        ChangeVolume(SOUND.BGM, data.BGMVolume);
        ChangeVolume(SOUND.EFFECT, data.EffectVolume);
        ChangeVolume(SOUND.BUG_EFFECT, data.BugVolume);
    }

    public void EffectPlay(TOOL tool)
    {
        EffectSound.clip = EffectSounds[(int)tool];
        EffectSound.Play();
    }

    public void UIEffectPlay(UI_SOUND sound)
    {
        UIEffectSound.clip = UIEffectSounds[(int)sound];
        UIEffectSound.Play();
    }

    public void UIEffectPlay(int sound)
    {
        /*
            0: START Menu Sound
            1: PAUSE Menu Sound
            2: APPLY Sound
            3: CANCEL Sound
            4: Buy Sound
        */

        UIEffectSound.clip = UIEffectSounds[sound];
        UIEffectSound.Play();
    }
}
