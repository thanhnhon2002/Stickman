using Firebase.Analytics;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeepTrackSDK
{
    public class DeepTrack : MonoBehaviour
    {
        public static DeepTrack Instance { get; private set; }
        private static DeepTrackUser userData;
        private static List<string> actions;
        private float lastAnalytics;
        private float previousPlayTime;
        public static bool isFirebaseReady = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (isFirebaseReady == false) return;
            if (userData == null) return;
            if (Time.time - lastAnalytics > DeepTrackConstants.MIN_SECONDS_ANALYTICS)
            {
                lastAnalytics = Time.time;
                userData.totalPlayTime = previousPlayTime + Time.time;
                SaveAllData();
            }
        }

        private void Init()
        {
            userData = DeepTrackSaveLoadData.LoadUserData();
            actions = new List<string>();

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    isFirebaseReady = true;
                    Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                    DeepTrackSaveLoadData.databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
                    TrackUserData();
                    TrackRetention();
                    SaveAllData();
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
                Firebase.Analytics.FirebaseAnalytics.LogEvent("loading_start");
            });
        }

        private void SaveAllData()
        {
            DeepTrackSaveLoadData.SaveLocal(userData);
            DeepTrackSaveLoadData.SaveServer(userData);
            DeepTrackSaveLoadData.SaveActions(actions);
        }

        private void TrackUserData()
        {
            if (string.IsNullOrEmpty(userData.originalAppVersion))
            {
                userData.originalAppVersion = Application.version;
            }
            if (userData.originalAppVersion != Application.version && DeepTrackConstants.CLEAR_USER_DATA_WHEN_UPDATE)
            {
                string originalVersion = userData.originalAppVersion;
                userData = new DeepTrackUser();
                userData.originalAppVersion = originalVersion;
                userData.startPlayDay = new TimeSpan(DateTime.UtcNow.Ticks).TotalDays;
                userData.firstOpen = false;
            }
            userData.appVersion = Application.version;
            userData.language = Application.systemLanguage.ToString();
            userData.sessionCount++;
            userData.installerName = Application.installerName;
            if (userData.actionChecker == null) userData.actionChecker = new List<string>();
            if (userData.firstOpen == false)
            {
                userData.firstOpen = true;
                FirebaseAnalytics.LogEvent(DeepTrackConstants.FIRST_OPEN_EVENT_NAME);
            }
            previousPlayTime = userData.totalPlayTime;
        }

        private void TrackRetention()
        {
            double today = new TimeSpan(DateTime.Now.Ticks).TotalDays;
            if (userData.startPlayDay == 0)
            {
                userData.startPlayDay = today;
            }
            if (userData.lastPlayDay == "")
            {
                userData.lastPlayDay = DateTime.Now.ToString("yyMMdd");
            }
            string todayCode = DateTime.Now.ToString("yyMMdd");
            if (todayCode != userData.lastPlayDay)
            {
                userData.lastPlayDay = todayCode;
                userData.totalPlayDay++;
            }
            userData.retentionType = (int)(today - userData.startPlayDay);
        }

        private static void SetCurrentLevel(int level)
        {
            userData.level = level;
            if (userData.maxLevel < level)
            {
                userData.maxLevel = level;
            }
            DeepTrackSaveLoadData.SaveLocal(userData);
        }

        public static void LogLevelStart(int level)
        {
            SetCurrentLevel(level);
            actions.Add("play level" + level);
        }

        public static void LogLevelWin(int level)
        {
            SetCurrentLevel(level);
            CountWinLose(level.ToString(), false);
            actions.Add("win level " + level);
        }

        public static void LogLevelLose(int level)
        {
            SetCurrentLevel(level);
            CountWinLose(level.ToString(), false);
            actions.Add("lose level " + level);
        }
        public static void LogEvent(DeepTrackEvent e, params object[] messages)
        {
            if (!isFirebaseReady) return;
            string s = e.ToString() + ' ' + string.Join(' ', messages);
            if (e == DeepTrackEvent.inter_fail || e == DeepTrackEvent.reward_fail || e == DeepTrackEvent.appopen_fail)
            {
                s += " internet = " + Application.internetReachability;
            }
            FirebaseAnalytics.LogEvent(e.ToString());
            actions.Add(s);
            userData.CheckValid();

            switch (e)
            {
                case DeepTrackEvent.inter_success:
                    userData.revenue.interSuccess++;
                    break;
                case DeepTrackEvent.reward_success:
                    userData.revenue.rewardSuccess++;
                    break;
                case DeepTrackEvent.appopen_success:
                    userData.revenue.appOpenSuccess++;
                    break;
                case DeepTrackEvent.banner_success:
                    userData.revenue.bannerSuccess++;
                    break;
            }
        }

        public static void Log(params object[] messages)
        {
            if (!isFirebaseReady) return;
            string s = string.Join(' ', messages);
            actions.Add(s);
        }

        private static void CountWinLose(string levelName, bool isWin)
        {
            if (userData.winloseDatas == null)
            {
                userData.winloseDatas = new Dictionary<string, DeepTrackWinLoseData>();
            }
            if (userData.winloseDatas.ContainsKey(levelName) == false)
            {
                userData.winloseDatas.Add(levelName, new DeepTrackWinLoseData());
            }
            if (isWin) userData.winloseDatas[levelName].winCount++;
            else userData.winloseDatas[levelName].loseCount++;
            DeepTrackSaveLoadData.SaveLocal(userData);
        }
    }
}