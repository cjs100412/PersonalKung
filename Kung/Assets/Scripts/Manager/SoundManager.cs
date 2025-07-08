using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // EventTrigger를 사용하기 위해 필요

// 1. 사운드 구분을 위한 enum을 설계합니다.
public enum SOUND_TYPE
{
    BGM, SFX
}

// 세부적으로 나누는 enum (이 프로젝트에서는 이거 사용)
public enum BGM
{
    Title, InGame, bossbgm
}

public enum SFX
{
    PlayerDamaged, PlayerDead, NormalMonsterDead, BossMonsterDead, Bomb, Bigbomb, InventoryOpen, InventoryClose,
    Drilling, Mineral
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
    // 2. 클래스에 필요한 필드 값 설계
    [Header("오디오 믹서")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGM"; // 오디오 믹서에 만들어둔 이름
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

    // 3. 사운드 매니저는 전체 게임에서 1개만 필요하다.(싱글톤)
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 딕셔너리 생성
            bgm_dict = new Dictionary<BGM, AudioClip>();
            sfx_dict = new Dictionary<SFX, AudioClip>();

            // 유형 별로 등록(BGM)
            foreach (var bgmItem in bgm_list)
            {
                if (!bgm_dict.ContainsKey(bgmItem.type))
                    bgm_dict[bgmItem.type] = bgmItem.clip;
            }
            // 유형 별로 등록(SFX)
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
        // 씬이 로드될 때마다 OnSceneLoaded 호출
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // 시작 시 슬라이더가 Inspector에 설정된 값(보통 0.0001 ~ 1)을 읽어서 믹서 볼륨으로 세팅
        if (bgm_slider != null)
        {
            bgm_value = Mathf.Log10(Mathf.Max(bgm_slider.value, 0.0001f)) * 20f;
            audioMixer.SetFloat(bgmParameter, bgm_value);
            // BGM 슬라이더 값이 바뀔 때마다 SetBGMVolume이 호출되도록 연결
            bgm_slider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfx_slider != null)
        {
            sfx_value = Mathf.Log10(Mathf.Max(sfx_slider.value, 0.0001f)) * 20f;
            audioMixer.SetFloat(sfxParameter, sfx_value);
            // SFX 슬라이더 값이 바뀔 때마다 SetSFXVolume이 호출되도록 연결
            sfx_slider.onValueChanged.AddListener(SetSFXVolume);

            // sfx_slider에 Pointer Up 이벤트(마우스 떼는 순간)를 걸어서
            //    그때 공격 효과음을 한 번만 재생하도록 설정
            AttachPointerUpEventToSlider(sfx_slider, () =>
            {
                // 마우스 버튼을 떼는 순간 호출될 콜백
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySFX(SFX.Bomb);
                }
            });
        }
    }

    // 씬이 로드될 때 호출되는 콜백
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

    // BGM 재생
    public void PlayBGM(BGM bgm_type)
    {
        if (bgm_dict.TryGetValue(bgm_type, out var clip))
        {
            // 이미 같은 클립을 틀고 있으면 리턴
            if (bgm.clip == clip)
                return;

            bgm.clip = clip;
            bgm.loop = true;
            bgm.Play();
        }
    }

    // SFX 재생
    public void PlaySFX(SFX sfx_type)
    {
        if (sfx_dict.TryGetValue(sfx_type, out var clip))
        {
            sfx.PlayOneShot(clip);
        }
    }

    // UI의 Slider의 onValueChanged 이벤트 쪽에서 적용할 함수 구현
    // Audio Mixer의 볼륨 단위는 0 db ~ -80 db까지(0.0001 ~ 1 영역에 매핑)로 설정되어 있습니다.
    public void SetBGMVolume(float volume)
    {
        // 토글을 꺼둔 상태로 강제 변경
        if (bgm_toggle != null) bgm_toggle.isOn = false;

        // volume이 0이면 음소거(-80db)로 클램프
        float v = Mathf.Max(volume, 0.0001f);
        bgm_value = Mathf.Log10(v) * 20f;
        audioMixer.SetFloat(bgmParameter, bgm_value);
    }

    public void SetSFXVolume(float volume)
    {
        // 토글을 꺼둔 상태로 강제 변경
        if (sfx_toggle != null) sfx_toggle.isOn = false;

        // volume이 0이면 음소거(-80db)로 클램프
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

    // 슬라이더(GameObject)에 EventTrigger를 붙이고, PointerUp 이벤트를 추가하는 헬퍼 메서드
    private void AttachPointerUpEventToSlider(Slider slider, Action onPointerUpCallback)
    {
        if (slider == null || onPointerUpCallback == null)
            return;

        // Slider GameObject에 EventTrigger 컴포넌트가 있는지 확인하고, 없으면 추가
        EventTrigger trigger = slider.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = slider.gameObject.AddComponent<EventTrigger>();
        }

        // PointerUp용 Entry를 생성
        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };

        // 이벤트가 발생했을 때 호출될 콜백 등록
        entry.callback.AddListener((data) =>
        {
            onPointerUpCallback.Invoke();
        });

        // 트리거 리스트에 추가
        trigger.triggers.Add(entry);
    }
}
