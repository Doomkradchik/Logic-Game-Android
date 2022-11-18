using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    private ClampedLevelWithButtons _clampedLevel;
    private LevelLoader _loader;

    private LevelPresenter _presenter;
    private AdsCore _adsCore;

    private void Awake()
    {
        _clampedLevel = FindObjectOfType<ClampedLevelWithButtons>();
        _loader = FindObjectOfType<LevelLoader>();
        _adsCore = new AdsCore();
        _presenter = new LevelPresenter(_clampedLevel, _loader, _adsCore);

        _adsCore.Initialize();
    }

    private void OnEnable()
    {
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}
