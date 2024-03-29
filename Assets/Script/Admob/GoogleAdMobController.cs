using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Ump.Api;
using Firebase.Analytics;
using TMPro;
using DeepTrackSDK;

public class GoogleAdmobController : MonoBehaviour
{
    public static GoogleAdmobController Instance;
    public static bool isAdmobReady = false;

    public const string ID_ADS_BANNER = "";
    public const string ID_ADS_INTER = "";
    public const string ID_ADS_REWARD = "";
    public const string ID_ADS_APPOPEN = "";

    public Action onAppOpenLoaded;
    public Action onAppOpenClosed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        var debugSettings = new ConsentDebugSettings
        {
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds =
                new List<string>
                {
                    "cc647ae4-691e-4529-bdf1-9abcc2c9fac3"
                }
        };
        ConsentRequestParameters request = new ConsentRequestParameters()
        {
            ConsentDebugSettings = debugSettings,
        };
        ConsentInformation.Update(request, ConsentAndInitAds);
    }

    void ConsentAndInitAds(FormError consentError)
    {
        if (consentError != null)
        {
            UnityEngine.Debug.LogError(consentError);
            if (DeepTrack.isFirebaseReady)
            {
                FirebaseAnalytics.LogEvent("consent_error");
            }
            return;
        }
        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
        {
            if (formError != null)
            {
                UnityEngine.Debug.LogError(consentError);
                if (DeepTrack.isFirebaseReady)
                {
                    FirebaseAnalytics.LogEvent("consent_show_error");
                }
                return;
            }
            if (ConsentInformation.CanRequestAds())
            {
                MobileAds.Initialize((InitializationStatus initstatus) =>
                {
                    isAdmobReady = true;
                    RequestAllAds();
                });
            } else
            {
                if (DeepTrack.isFirebaseReady)
                {
                    FirebaseAnalytics.LogEvent("consent_refused");
                }
            }
        });
    }

    public void RequestAllAds()
    {
        LoadAppOpenAds();
        LoadInterstitialAd();
        LoadRewardedAd();
        LoadBanner(AdPosition.Bottom);
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    //INTERTISTIAL
    private InterstitialAd _interstitialAd;
    private Action onIntertistialClose;
    private long lastLoadIntertistial;
    public static bool isShowingInter;

    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        var adRequest = new AdRequest();
        InterstitialAd.Load(ID_ADS_INTER, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad with error : " + error);
                    return;
                }
                _interstitialAd = ad;
            });
    }

    public bool ShowInterstitialAd(Action onIntertistialClose)
    {
        this.onIntertistialClose = onIntertistialClose;
        bool canShow = _interstitialAd != null && _interstitialAd.CanShowAd();

        if (canShow == false || isAdmobReady == false)
        {
            onIntertistialClose?.Invoke();
            Debug.LogError("Interstitial ad is not ready yet.");
            float seconds = (DateTime.Now.Ticks - lastLoadIntertistial) / 10000;
            if (seconds > 5) LoadInterstitialAd();
            return false;
        }

        _interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            MainThreadManager.Instance.ExecuteInUpdate(() =>
            {
                this.onIntertistialClose?.Invoke();
                isShowingInter = false;
            });
        };
        _interstitialAd.Show();
        lastLoadIntertistial = DateTime.Now.Ticks;
        isShowingInter = true;
        return true;
    }

    //BANNER
    BannerView _bannerView;
    public static bool haveBannerAds;
    public void LoadBanner(AdPosition position)
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
        _bannerView = new BannerView(ID_ADS_BANNER, AdSize.Banner, position);
        _bannerView.OnBannerAdLoaded += () =>
        {
            haveBannerAds = true;
        };
        _bannerView.OnBannerAdLoadFailed += (error) =>
        {
            haveBannerAds = false;
        };
    }
    public void SetBannerVisible(bool visible)
    {
        if (visible) _bannerView?.Show();
        else _bannerView?.Hide();
    }

    //REWARD
    private RewardedAd _rewardedAd;
    private long lastLoadRewarded;
    public static bool isShowingRewarded;

    public void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        var adRequest = new AdRequest();
        RewardedAd.Load(ID_ADS_REWARD, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                    return;
                }
                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                _rewardedAd = ad;
            });
    }

    public bool ShowRewardedAd()
    {
        const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";
        bool canShowAds = _rewardedAd != null && _rewardedAd.CanShowAd();
        if (canShowAds)
        {
            _rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                LoadRewardedAd();
            };
            _rewardedAd.Show((Reward reward) =>
            {
                MainThreadManager.Instance.ExecuteInUpdate(() =>
                {
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                    AdManager.Instance.HandleEarnReward();
                    isShowingRewarded = false;
                });
            });
            lastLoadRewarded = DateTime.Now.Ticks;
            isShowingRewarded = true;
            return true;
        }
        else
        {
            float seconds = (DateTime.Now.Ticks - lastLoadRewarded) / 10000;
            if (seconds > 5) LoadRewardedAd();
            return false;
        }
    }

    //APPOPEN
    public static bool isShowingAppOpenAds;
    AppOpenAd appOpenAd;
    public void LoadAppOpenAds()
    {
        var adRequest = new AdRequest();
        AppOpenAd.Load(ID_ADS_APPOPEN, adRequest, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }
            appOpenAd = ad;
            MainThreadManager.Instance.ExecuteInUpdate(() =>
            {
                onAppOpenLoaded?.Invoke();
            });
        });
    }

    public void ShowAppOpen()
    {
        Debug.Log($"calling show app open, app open ads = {appOpenAd}, canshowads = {appOpenAd.CanShowAd()}, isshowingappopenads = {isShowingAppOpenAds}");
        if (appOpenAd == null || appOpenAd.CanShowAd() == false || isShowingAppOpenAds) return;
        appOpenAd.OnAdFullScreenContentClosed += () =>
        {
            MainThreadManager.Instance.ExecuteInUpdate(() =>
            {
                isShowingAppOpenAds = false;
                LoadAppOpenAds();
                onAppOpenClosed?.Invoke();
            });
        };
        appOpenAd.Show();
        isShowingAppOpenAds = true;
    }

    public void OnAppStateChanged(AppState state)
    {
        try
        {
            if (isShowingInter || isShowingRewarded) return;
            if (state == AppState.Foreground)
            {
                Debug.Log("calling show app open");
                ShowAppOpen();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}