using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using DarkcupGames;

public class AdManager : MonoBehaviour
{
    public const bool SHOW_ADS = true;
    public const int MIN_LEVEL_TO_SHOW_ADS = 3;
    public const int ADS_LEVEL_COUNT_CAPPING = 1;

    public static AdManager Instance;
    public List<UnityEvent> adEvents;
    int watchAdsId;

    private void Awake()
    {
        Instance = this;
    }

    public void HandleEarnReward()
    {
        MainThreadManager.Instance.ExecuteInUpdate(() => {
            if (watchAdsId < adEvents.Count)
            {
                UnityEvent e = adEvents[watchAdsId];
                e?.Invoke();
            }
            Firebase.Analytics.FirebaseAnalytics.LogEvent("on_ad_reward_complete");
        }); 
    }

    public void WatchAds(int id)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("on_ad_reward_click");
        watchAdsId = id;
        if (SHOW_ADS)
        {
            bool success = GoogleAdmobController.Instance.ShowRewardedAd();
        } else
        {
            HandleEarnReward();
        }
    }

    public bool ShowIntertistial(System.Action onIntertistialClose = null)
    {
        try
        {
            if (GameSystem.userdata.boughtItems.Contains(IAP_ID.no_ads.ToString()))
            {
                onIntertistialClose?.Invoke();
                return false;
            }
            Firebase.Analytics.FirebaseAnalytics.LogEvent("on_ad_inter_show");
            bool success = GoogleAdmobController.Instance.ShowInterstitialAd(onIntertistialClose);
            return true;
        }
        catch (Exception e)
        {
            onIntertistialClose?.Invoke();
            return false;
        }
    }

    public void SetBannerVisible(bool visible)
    {
        GoogleAdmobController.Instance.SetBannerVisible(visible);
    }
}