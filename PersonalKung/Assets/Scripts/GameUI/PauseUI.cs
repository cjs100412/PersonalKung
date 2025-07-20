using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("Pause UI 에 배치된 슬라이더/토글")]
    public Slider bgmSlider;
    public Toggle bgmToggle;
    public Slider sfxSlider;
    public Toggle sfxToggle;

    void Start()
    {
        var soundManager = SoundManager.Instance;
        if (soundManager == null)
        {
            Debug.LogWarning("PauseUI: SoundManager.Instance가 null 입니다. SoundManager가 씬에 존재하는지 확인해 주세요.");
            return;
        }

        if (soundManager.bgm_slider != null)
            bgmSlider.value = soundManager.bgm_slider.value;
        if (soundManager.bgm_toggle != null)
            bgmToggle.isOn = soundManager.bgm_toggle.isOn;

        if (soundManager.sfx_slider != null)
            sfxSlider.value = soundManager.sfx_slider.value;
        if (soundManager.sfx_toggle != null)
            sfxToggle.isOn = soundManager.sfx_toggle.isOn;

        bgmSlider.onValueChanged.AddListener(soundManager.SetBGMVolume);
        bgmToggle.onValueChanged.AddListener(soundManager.MuteBGM);

        sfxSlider.onValueChanged.AddListener(soundManager.SetSFXVolume);
        sfxToggle.onValueChanged.AddListener(soundManager.MuteSFX);

        bgmSlider.onValueChanged.AddListener(_ => { if (bgmToggle != null) bgmToggle.isOn = false; });
        sfxSlider.onValueChanged.AddListener(_ => { if (sfxToggle != null) sfxToggle.isOn = false; });

        AttachPointerUpEventToSlider(sfxSlider, () =>
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(SFX.Bomb);
            }
        });
    }

    void OnDestroy()
    {
        var sm = SoundManager.Instance;
        if (sm != null)
        {
            bgmSlider.onValueChanged.RemoveListener(sm.SetBGMVolume);
            bgmToggle.onValueChanged.RemoveListener(sm.MuteBGM);
            sfxSlider.onValueChanged.RemoveListener(sm.SetSFXVolume);
            sfxToggle.onValueChanged.RemoveListener(sm.MuteSFX);
        }
        bgmSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
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
