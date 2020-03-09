using Assets.Scripts.MinerGame.PlayerFiles;
using Assets.Scripts.MinerGame.TimeSensitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MinerGame {
    /// <summary>
    /// All Global Values each class should have access to.
    /// </summary>
    public static class GlobalConfig {
        #region Variables
        /// <summary>
        /// Class tiers.
        /// </summary>
        public enum ClassTier : int { G = 0, F = 1, E = 2, D = 3, C = 4, B = 5, A = 6 }
        #region Save Keys
        #region Time
        /// <summary>
        /// The universal key for the saved time value.
        /// </summary>
        public const string key_Time = "Time";
        /// <summary>
        /// The universal key for the saved AM/PM value.
        /// </summary>
        public const string key_Meridiem = "Meridiem";
        /// <summary>
        /// The universal key for the saved Day of the Week value.
        /// </summary>
        public const string key_Day = "Day";
        /// <summary>
        /// The universal key for the saved Week value.
        /// </summary>
        public const string key_Week = "Week"; 
        #endregion

        /// <summary>
        /// The universal key for the saved Class value.
        /// </summary>
        public const string key_ClassTier = "Class";
        /// <summary>
        /// The universal key for the saved Class points value.
        /// </summary>
        public const string key_ClassPoints = "Points";

        #region Inventory
        /// <summary>
        /// The universal key for the saved Trash value.
        /// </summary>
        public const string key_Trash = "Trash";
        /// <summary>
        /// The universal key for the saved Weapon calue.
        /// </summary>
        public const string key_Weapon = "Weapon";
        /// <summary>
        /// The universal key for the saved Gained Money value.
        /// </summary>
        public const string key_GainedMoney = "Gained Money";
        /// <summary>
        /// The universal key for the saved Money value.
        /// </summary>
        public const string key_Money = "Money"; 
        #endregion

        #region Sleeping
        /// <summary>
        /// The universal key for the saved Last Hour Slept value.
        /// </summary>
        public const string key_Sleep_LastHour = "LastHourSlept";
        /// <summary>
        /// The universal key for the saved Last Day Slept value.
        /// </summary>
        public const string key_Sleep_LastDay = "LastDaySlept";
        /// <summary>
        /// The universal key for the saved Last Week Slept value.
        /// </summary>
        public const string key_Sleep_LastWeek = "LastWeekSlept";
        /// <summary>
        /// The universal key for the saved Sleep Death Hour value.
        /// </summary>
        public const string key_Sleep_DeathHour = "SleepDeathHour";
        /// <summary>
        /// The universal key for the saved Sleep Death Day value.
        /// </summary>
        public const string key_Sleep_DeathDay = "SleepDeathDay";
        /// <summary>
        /// The universal key for the saved Sleep Death Week value.
        /// </summary>
        public const string key_Sleep_DeathWeek = "SleepDeathWeek";
        /// <summary>
        /// The universal key for the saved Week Changed From Sleep value.
        /// </summary>
        public const string key_WeekChangedFromSleep = "WeekChangedFromSleep";
        #endregion

        #region Showering
        /// <summary>
        /// The universal key for the saved Last Hour Showered value.
        /// </summary>
        public const string key_Showering_LastHour = "LastHourShowered";
        /// <summary>
        /// The universal key for the saved Last Day Showered value.
        /// </summary>
        public const string key_Showering_LastDay = "LastDayShowered";
        /// <summary>
        /// The universal key for the saved Last Week Showered value.
        /// </summary>
        public const string key_Showering_LastWeek = "LastWeekShowered";
        /// <summary>
        /// The universal key for the saved Showered Meridiem value.
        /// </summary>
        public const string key_Showering_Meridiem = "ShoweredMeridiem";
        /// <summary>
        /// The universal key for the saved Stink Hour value.
        /// </summary>
        public const string key_Showering_StinkHour = "StinkHour";
        /// <summary>
        /// The universal key for the saved Stink Day value.
        /// </summary>
        public const string key_Showering_StinkDay = "StinkDay";
        /// <summary>
        /// The universal key for the saved Stink Week value.
        /// </summary>
        public const string key_Showering_StinkWeek = "StinkWeek";
        /// <summary>
        /// The universal key for the saved Player Stinks value.
        /// </summary>
        public const string key_Showering_PlayerStinks = "PlayerStinks";
        #endregion

        /// <summary>
        /// The universal key for the saved Previous Scene Index value.
        /// </summary>
        public const string key_PreviousSceneIndex = "PrevSceneIndex";

        /// <summary>
        /// The universal key for the saved Times Late To Work value.
        /// </summary>
        public const string key_TimesLateToWork = "TimesLateToWork";
        /// <summary>
        /// The universal key for the saved Days Worked In Week;
        /// </summary>
        public const string key_DaysWorkedInWeek = "DaysWorkedInWeek"; 
        #endregion

        /// <summary>
        /// The universal scene name for the death screen.
        /// </summary>
        public const string scene_Death = "Death Screen";
        #endregion

        /// <summary>
        /// Saves the current time.
        /// </summary>
        /// <param name="ts">
        /// The time system we're saving from.
        /// </param>
        public static void SaveTime(TimeSystem ts) {
            PlayerPrefs.SetFloat(key_Time, ts.lastTick);
            PlayerPrefs.SetInt(key_Week, ts.week);

            if (ts.amPm.text == string.Empty) {
                PlayerPrefs.SetString(key_Meridiem, "AM");
            } else {
                PlayerPrefs.SetString(key_Meridiem, ts.amPm.text);
            }

            if (ts.currentDayOfTheWeek < 1 || ts.currentDayOfTheWeek > 7) {
                Debug.LogWarning("Current day of the week was non-satisfactory.");
                PlayerPrefs.SetInt(key_Day, 1);
            } else {
                PlayerPrefs.SetInt(key_Day, ts.currentDayOfTheWeek);
            }
        }

        /// <summary>
        /// Saves certain perameters so that they reflect seven in the morning.
        /// </summary>
        /// <param name="currDay"></param>
        public static void SaveForMorning(int currDay) {
            PlayerPrefs.SetFloat(key_Time, 0.5833334f);
            PlayerPrefs.SetString(key_Meridiem, "AM");
            if (currDay >= 7) {
                PlayerPrefs.SetInt(key_WeekChangedFromSleep, 1);
                PlayerPrefs.SetInt(key_Day, 1);
                PlayerPrefs.SetInt(key_Week, PlayerPrefs.GetInt(key_Week) + 1);
            } else {
                PlayerPrefs.SetInt(key_Day, currDay + 1);
            }
        }

        /// <summary>
        /// Saves the current citizen staus.
        /// </summary>
        /// <param name="cs">
        /// The Citizen Ranking we're saving from.
        /// </param>
        public static void SaveCitizenStatus (CitizenRanking cs) {
            PlayerPrefs.SetInt(key_ClassPoints, cs.currentClassPoints);
            PlayerPrefs.SetInt(key_ClassTier, (int) cs.currentTier);
        }

        /// <summary>
        /// Saves the player's sleep health status.
        /// </summary>
        /// <param name="ph">
        /// The player health class this function reads from.
        /// </param>
        public static void SaveSleepHealth (PlayerHealth ph) {
            PlayerPrefs.SetInt(key_Sleep_LastHour, ph.lastHourSlept);
            PlayerPrefs.SetInt(key_Sleep_LastDay, ph.lastDaySlept);
            PlayerPrefs.SetInt(key_Sleep_LastWeek, ph.lastWeekSlept);
            PlayerPrefs.SetInt(key_Sleep_DeathHour, ph.sleepDeathHour);
            PlayerPrefs.SetInt(key_Sleep_DeathDay, ph.sleepDeathDay);
            PlayerPrefs.SetInt(key_Sleep_DeathWeek, ph.sleepDeathWeek);
        }

        /// <summary>
        /// Saves the current level index to a PlayerPref.
        /// </summary>
        public static void SaveLevelIndex() {
            Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetInt(key_PreviousSceneIndex, UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Saves the values of the player's weekly tasks.
        /// </summary>
        /// <param name="wt"></param>
        public static void SaveWeeklyTasks(WeeklyTasks wt) {
            PlayerPrefs.SetInt(key_TimesLateToWork, wt.timesLateToWork);
            PlayerPrefs.SetInt(key_DaysWorkedInWeek, wt.daysWorked);
        }

        /// <summary>
        /// Saves the showering data of the player.
        /// </summary>
        /// <param name="ph">
        /// The Player Health class this function will read off from.
        /// </param>
        public static void SaveShoweringData (PlayerHealth ph) {
            PlayerPrefs.SetInt(key_Showering_LastHour, ph.lastHourShowered);
            PlayerPrefs.SetInt(key_Showering_LastDay, ph.lastDayShowered);
            PlayerPrefs.SetInt(key_Showering_LastWeek, ph.lastWeekShowered);
            PlayerPrefs.SetString(key_Showering_Meridiem, ph.showeredMeridiem);
            PlayerPrefs.SetInt(key_Showering_StinkHour, ph.stinkHour);
            PlayerPrefs.SetInt(key_Showering_StinkDay, ph.stinkDay);
            PlayerPrefs.SetInt(key_Showering_StinkWeek, ph.stinkWeek);

            if (ph.playerStinks) {
                PlayerPrefs.SetInt(key_Showering_PlayerStinks, 1);
            } else {
                PlayerPrefs.SetInt(key_Showering_PlayerStinks, 0);
            }
        }
    }
}