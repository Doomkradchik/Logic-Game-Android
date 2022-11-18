
public class LevelPresenter
{
    public LevelPresenter(ClampedLevelWithButtons clampedLevel, 
        LevelLoader loader, AdsCore adsCore)
    {
        _clampedLevel = clampedLevel;
        _loader = loader;
        _adsCore = adsCore;
    }
    private readonly ClampedLevelWithButtons _clampedLevel;
    private readonly LevelLoader _loader;
    private readonly AdsCore _adsCore;

    public void Enable()
    {
        _loader.SceneLoaded += _clampedLevel.UpdateLevelText;
        _clampedLevel.LevelReloaded += _loader.ReloadLevel;
        _adsCore.AdsFinished += _loader.LoadNextLevel;
    }
    
    public void Disable()
    {
        _loader.SceneLoaded -= _clampedLevel.UpdateLevelText;
        _clampedLevel.LevelReloaded -= _loader.ReloadLevel;
        _adsCore.AdsFinished -= _loader.LoadNextLevel;
    }
}
