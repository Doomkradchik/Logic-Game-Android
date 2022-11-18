using UnityEngine.UI;
using UnityEngine;
using System;

public class ClampedLevelWithButtons : MonoBehaviour
{
    [SerializeField]
    private Text _level;
    [SerializeField]
    private Button _reloadLevel;
    [SerializeField]
    private Button _skiplevel;

    public event Action LevelReloaded;

    private void OnEnable()
    {
        _reloadLevel.onClick.AddListener(ReloadLevel);
        _skiplevel.onClick.AddListener(AdsCore.ShowAdsVideo);
    }

    private void OnDisable()
    {
        _reloadLevel.onClick.RemoveListener(ReloadLevel);
        _skiplevel.onClick.RemoveListener(AdsCore.ShowAdsVideo);
    }

    public void UpdateLevelText(int current)
    {
        _level.text = $"Level {current}";
    }

    private void ReloadLevel() => LevelReloaded?.Invoke();
    
}
