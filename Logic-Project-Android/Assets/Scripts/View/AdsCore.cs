using UnityEngine.Advertisements;
using System;

public class AdsCore : IUnityAdsListener
{
    public event Action AdsFinished;

    private const string GAME_ID = "5025781";
    private const string REWARDED_ID = "Rewarded_Android";

    private static bool TestMode { get; set; } = true;

    public void Initialize()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(GAME_ID, TestMode);
    }

    public static void ShowAdsVideo()
    {
        if (Advertisement.IsReady() == false)
            return;

        Advertisement.Show(REWARDED_ID);
    }

    public void OnUnityAdsReady(string placementId) { }

    public void OnUnityAdsDidError(string message) { }

    public void OnUnityAdsDidStart(string placementId) { }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == REWARDED_ID && showResult == ShowResult.Finished)
            AdsFinished?.Invoke();
    }
}
