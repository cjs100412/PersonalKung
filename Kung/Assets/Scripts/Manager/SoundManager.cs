using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum SOUND_TYPE
{
    BGM, SFX
}

public enum BGM
{
    Title, InGame, bossbgm, Shop
}

public enum SFX
{
    PlayerDamaged, PlayerDead, NormalMonsterDead, BossMonsterDead, Bomb, Dynamite, InventoryOpen, InventoryClose,
    Drilling, Mineral, Buyitem, Medicbox, Special,Airgage,Exchange,Error,
    Warning,Bombinstall
}

[Serializable]
public class BGMClip
{
    public BGM type;
    public AudioClip clip;
}

[Serializable]
public class SFXClip
{
    public SFX type;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    [Header("오디오 믹서")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGM";
    public string sfxParameter = "SFX";

    [Header("오디오 소스")]
    public AudioSource bgm;
    public AudioSource sfx;

    [Header("오디오 클립")]
    public List<BGMClip> bgm_list;
    public List<SFXClip> sfx_list;

    private Dictionary<BGM, AudioClip> bgm_dict; // BGM 유형에 따른 오디오 클립
    private Dictionary<SFX, AudioClip> sfx_dict; // SFX 유형에 따른 오디오 클립

    private float bgm_value;
    private float sfx_value;

    [Header("UI 슬라이더 & 토글")]
    public Slider bgm_slider;
    public Slider sfx_slider;
    public Toggle bgm_toggle;
    public Toggle sfx_toggle;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgm_dict = new Dictionary<BGM, AudioClip>();
            sfx_dict = new Dictionary<SFX, AudioClip>();

            foreach (var bgmItem in bgm_list)
            {
                if (!bgm_dict.ContainsKey(bgmItem.type))
                    bgm_dict[bgmItem.type] = bgmItem.clip;
            }
            foreach (var sfxItem in sfx_list)
            {
                if (!sfx_dict.ContainsKey(sfxItem.type))
                    sfx_dict[sfxItem.type] = sfxItem.clip;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (bgm_slider != null)
        {
            bgm_value = Mathf.Log10(Mathf.Max(bgm_slider.value, 0.0001f)) * 20f;
            audioMixer.SetFloat(bgmParameter, bgm_value);
            bgm_slider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfx_slider != null)
        {
            sfx_value = Mathf.Log10(Mathf.Max(sfx_slider.value, 0.0001f)) * 20f;
            audioMixer.SetFloat(sfxParameter, sfx_value);
            sfx_slider.onValueChanged.AddListener(SetSFXVolume);

            AttachPointerUpEventToSlider(sfx_slider, () =>
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySFX(SFX.Bomb);
                }
            });
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "TitleScene":
                PlayBGM(BGM.Title);
                break;

            case "KungGameScene":
                PlayBGM(BGM.InGame);
                break;

            default:
                PlayBGM(BGM.InGame);
                break;
        }
    }

    public void PlayBGM(BGM bgm_type)
    {
        if (bgm_dict.TryGetValue(bgm_type, out var clip))
        {
            if (bgm.clip == clip)
                return;

            bgm.clip = clip;
            bgm.loop = true;
            bgm.Play();
        }
    }

    public void PlaySFX(SFX sfx_type)
    {
        if (sfx_dict.TryGetValue(sfx_type, out var clip))
        {
            sfx.PlayOneShot(clip);
        }
    }

    public void SetBGMVolume(float volume)
    {
        if (bgm_toggle != null) bgm_toggle.isOn = false;

        float v = Mathf.Max(volume, 0.0001f);
        bgm_value = Mathf.Log10(v) * 20f;
        audioMixer.SetFloat(bgmParameter, bgm_value);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfx_toggle != null) sfx_toggle.isOn = false;

        float v = Mathf.Max(volume, 0.0001f);
        sfx_value = Mathf.Log10(v) * 20f;
        audioMixer.SetFloat(sfxParameter, sfx_value);

    }

    public void MuteBGM(bool mute)
    {
        audioMixer.SetFloat(bgmParameter, mute ? -80f : bgm_value);
    }

    public void MuteSFX(bool mute)
    {
        audioMixer.SetFloat(sfxParameter, mute ? -80f : sfx_value);
    }

    private void AttachPointerUpEventToSlider(Slider slider, Action onPointerUpCallback)
    {
        if (slider == null || onPointerUpCallback == null)
            return;

        EventTrigger trigger = slider.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = slider.gameObject.AddComponent<EventTrigger>();
        }

        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };

        entry.callback.AddListener((data) =>
        {
            onPointerUpCallback.Invoke();
        });

        trigger.triggers.Add(entry);
    }
}
