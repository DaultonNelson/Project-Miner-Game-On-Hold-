using Assets.Scripts.MinerGame.Interactions;
using Assets.Scripts.MinerGame.TimeSensitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame.PlayerFiles {
    public class PlayerHealth : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The instance of this class.
        /// </summary>
        public static PlayerHealth Instance { get; private set; }

        /// <summary>
        /// The time system the player's health is based off of.
        /// </summary>
        public TimeSystem ts;
        /// <summary>
        /// The particle system that generates stink clouds.
        /// </summary>
        public ParticleSystem stinkParticles;

        /// <summary>
        /// The Hour value the player last slept.
        /// </summary>
        public int lastHourSlept { get; private set; }
        /// <summary>
        /// The Day value the player last slept.
        /// </summary>
        public int lastDaySlept { get; private set; }
        /// <summary>
        /// The Week value the player last slept.
        /// </summary>
        public int lastWeekSlept { get; private set; }

        /// <summary>
        /// The Hour value the player last showered.
        /// </summary>
        public int lastHourShowered { get; set; }
        /// <summary>
        /// The Day value the player last showered.
        /// </summary>
        public int lastDayShowered { get; set; }
        /// <summary>
        /// The Week value the player last showered.
        /// </summary>
        public int lastWeekShowered { get; set; }
        /// <summary>
        /// The Meridiem the player last showered in.
        /// </summary>
        public string showeredMeridiem { get; set; }

        /// <summary>
        /// The hour value the player will die.
        /// </summary>
        public int sleepDeathHour { get; private set; }
        /// <summary>
        /// The Day value the player will die.
        /// </summary>
        public int sleepDeathDay { get; private set; }
        /// <summary>
        /// The week value the player will die.
        /// </summary>
        public int sleepDeathWeek { get; private set; }

        /// <summary>
        /// The Hour value the player will start to stink.
        /// </summary>
        public int stinkHour { get; set; }
        /// <summary>
        /// The Day value the player will start to stink.
        /// </summary>
        public int stinkDay { get; set; }
        /// <summary>
        /// The Week value the player will start to stink.
        /// </summary>
        public int stinkWeek { get; set; }

        /// <summary>
        /// The Citizen Ranking on the player.
        /// </summary>
        private CitizenRanking cr;
        /// <summary>
        /// Return true if script should check for death, or false if not. 
        /// </summary>
        private bool checkForDeath = true;
        /// <summary>
        /// Return true if script should check for stink, or false if not.
        /// </summary>
        private bool checkForStink = true;
        /// <summary>
        /// Return true if player stinks, or false if not.
        /// </summary>
        public bool playerStinks = false;
        #endregion

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Debug.LogError("There is more than one " + this + "in the scene!");
                Destroy(gameObject);
            }
        }

        private void Start() {
            cr = GetComponent<CitizenRanking>();
            LoadSleepHealth();
            LoadShoweringData();
            Debug.Log("Stink Day: Hour[" + stinkHour + showeredMeridiem + "], Day[" + stinkDay + "], Week[" + stinkWeek + "]");
            CheckIfPlayerShouldStink();
        }

        /// <summary>
        /// Checks if the player should stink.
        /// </summary>
        private void CheckIfPlayerShouldStink() {
            if (ts.week > (stinkWeek + 1)) {
                Debug.Log("It's been two weeks since the player has showered.");
                MakePlayerStink();
                return;
            }
            if (ts.week >= stinkWeek) {
                if (ts.currentDayOfTheWeek > stinkDay) {
                    Debug.Log("It's definitely been a couple days since the player showered.");
                    MakePlayerStink();
                    return;
                }
                else if (ts.currentDayOfTheWeek == stinkDay) {
                    if (showeredMeridiem == "AM") {
                        if (ts.amPm.text == "PM") {
                            Debug.Log("Player should have showered this morning");
                            MakePlayerStink();
                            return;
                        }
                        if (ts.currentHour >= stinkHour) {
                            Debug.Log("Player slept past the hour they should have showered.");
                            MakePlayerStink();
                            return;
                        }
                    }
                }
            }
            Debug.Log("If you made it this far, it means you're clean...FOR NOW!");
        }

        /// <summary>
        /// Loads sleep health.
        /// </summary>
        private void LoadSleepHealth() {
            lastHourSlept = PlayerPrefs.GetInt(GlobalConfig.key_Sleep_LastHour);
            lastDaySlept = PlayerPrefs.GetInt(GlobalConfig.key_Sleep_LastDay);
            lastWeekSlept = PlayerPrefs.GetInt(GlobalConfig.key_Sleep_LastWeek);
            sleepDeathHour = PlayerPrefs.GetInt(GlobalConfig.key_Sleep_DeathHour);
            sleepDeathDay = PlayerPrefs.GetInt(GlobalConfig.key_Sleep_DeathDay);
            sleepDeathWeek = PlayerPrefs.GetInt(GlobalConfig.key_Sleep_DeathWeek);
        }

        /// <summary>
        /// Loads Showering Data.
        /// </summary>
        private void LoadShoweringData() {
            lastHourShowered = PlayerPrefs.GetInt(GlobalConfig.key_Showering_LastHour);
            lastDayShowered = PlayerPrefs.GetInt(GlobalConfig.key_Showering_LastDay);
            lastWeekShowered = PlayerPrefs.GetInt(GlobalConfig.key_Showering_LastWeek);
            showeredMeridiem = PlayerPrefs.GetString(GlobalConfig.key_Showering_Meridiem);
            stinkHour = PlayerPrefs.GetInt(GlobalConfig.key_Showering_StinkHour);
            stinkDay = PlayerPrefs.GetInt(GlobalConfig.key_Showering_StinkDay);
            stinkWeek = PlayerPrefs.GetInt(GlobalConfig.key_Showering_StinkWeek);

            int stinkInt = PlayerPrefs.GetInt(GlobalConfig.key_Showering_PlayerStinks);

            switch (stinkInt) {
                case 0:
                    playerStinks = false;
                    break;
                case 1:
                    playerStinks = true;
                    break;
                default:
                    Debug.LogError("StinkInt was not 1 or 0!");
                    break;
            }
        }

        private void Update() {
            if (checkForDeath) {
                SleepDeprivation();
            }
            if (checkForStink) {
                if ((ts.amPm.text == showeredMeridiem) && (ts.currentHour == stinkHour) && (ts.currentDayOfTheWeek == stinkDay) && (ts.week == stinkWeek)) {
                    MakePlayerStink();
                }
            }
        }

        /// <summary>
        /// Checks for player sleep deprivation.
        /// </summary>
        private void SleepDeprivation() {
            if ((ts.week == sleepDeathWeek) && (ts.currentDayOfTheWeek == sleepDeathDay) && (ts.currentHour == sleepDeathHour)) {
                ts.timePaused = true;

                PlayerMovement.Instance.ableToMove = false;
                InteractionSystem.Instance.ableToAction = false;

                SceneTransitioner.FadeOut(GlobalConfig.scene_Death);

                checkForDeath = false;
            }
        }
        
        /// <summary>
        /// Makes the player stink.
        /// </summary>
        private void MakePlayerStink() {
            Debug.Log("Player should stink");
            stinkParticles.Play();
            playerStinks = true;
            checkForStink = false;
        }

        /// <summary>
        /// Sets the last day the player slept.
        /// </summary>
        public void SetLastTimePlayerSlept() {
            lastHourSlept = 7;
            lastDaySlept = PlayerPrefs.GetInt(GlobalConfig.key_Day);
            lastWeekSlept = PlayerPrefs.GetInt(GlobalConfig.key_Week);

            CalculateFourDaysFromNowForSleep();
            Debug.Log("Sleep Death: Hour[" + sleepDeathHour + "], Day[" + sleepDeathDay + "], Week[" + sleepDeathWeek + "]");
        }

        /// <summary>
        /// Sets the last time the player showered.
        /// </summary>
        public void SetLastTimePlayerShowered() {
            stinkParticles.Stop();
            checkForStink = true;
            playerStinks = false;
            lastHourShowered = ts.currentHour;

            lastDayShowered = ts.currentDayOfTheWeek;
            lastWeekShowered = ts.week;
            showeredMeridiem = ts.amPm.text;

            CalculateTwoDaysFromNowForShowering();
            Debug.Log("Stink Day: Hour[" + stinkHour + showeredMeridiem + "], Day[" + stinkDay + "], Week[" + stinkWeek + "]");
        }

        /// <summary>
        /// Calculates a date four days later for sleeping.
        /// </summary>
        private void CalculateFourDaysFromNowForSleep() {
            sleepDeathHour = lastHourSlept;
            if (lastDaySlept > 3) {
                switch (lastDaySlept) {
                    case 4:
                        sleepDeathDay = 1;
                        break;
                    case 5:
                        sleepDeathDay = 2;
                        break;
                    case 6:
                        sleepDeathDay = 3;
                        break;
                    case 7:
                        sleepDeathDay = 4;
                        break;
                    default:
                        Debug.LogError("Last Day Slept was greater than 7.");
                        sleepDeathDay = 1;
                        break;
                }
                sleepDeathWeek = lastWeekSlept + 1;
            }
            else {
                sleepDeathDay = lastDaySlept + 4;
                sleepDeathWeek = lastWeekSlept;
            }
        }

        /// <summary>
        /// Calculates a date two days later for showering.
        /// </summary>
        private void CalculateTwoDaysFromNowForShowering() {
            stinkHour = lastHourShowered;
            if (lastDayShowered > 5) {
                switch (lastDayShowered) {
                    case 6:
                        stinkDay = 1;
                        break;
                    case 7:
                        stinkDay = 1;
                        break;
                    default:
                        Debug.LogError("Last Day Showered was greater then 7");
                        break;
                }
                stinkWeek = lastWeekShowered + 1;
            }
            else {
                stinkDay = lastDayShowered + 2;
                stinkWeek = lastWeekShowered;
            }
        }

        private void DockAndShow(int dockingScore, string message) {
            cr.SubtractPoints(dockingScore);
            if (dockingScore != 0) {
                //Prehaps change notification
                cr.citizenCardAnimator.SetTrigger("Show");

                Color a = new Color(.263f, 0, 0, 1.0f);

                InteractionSystem.Instance.DisplayNotification(Color.red, a, message);
            }

            //Send Notification
        }

        private int CalculateStinkPunishment() {
            int output = 0;

            switch (cr.currentTier) {
                case GlobalConfig.ClassTier.A:
                    break;
                case GlobalConfig.ClassTier.B:
                    output = 1;
                    break;
                case GlobalConfig.ClassTier.C:
                    output = 2;
                    break;
                case GlobalConfig.ClassTier.D:
                    output = 3;
                    break;
                case GlobalConfig.ClassTier.E:
                    output = 4;
                    break;
                case GlobalConfig.ClassTier.F:
                    output = 5;
                    break;
                case GlobalConfig.ClassTier.G:
                    output = 6;
                    break;
            }

            return output;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Smeller") {
                if (playerStinks) {
                    DockAndShow(CalculateStinkPunishment(), "Smell Reported");
                }
            }
        }
    }
}