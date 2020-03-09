using Assets.Scripts.MinerGame.Interactions;
using Assets.Scripts.MinerGame.TimeSensitive;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame {
    public class WeeklyTasks : MonoBehaviour {
        #region Variables
        /// <summary>
        /// The instance of this class.
        /// </summary>
        public static WeeklyTasks Instance { get; private set; }

        /// <summary>
        /// The Time System the weekly tasks reads off from.
        /// </summary>
        public TimeSystem ts;
        /// <summary>
        /// The Citizen Ranking Class that is in the scene.
        /// </summary>
        public CitizenRanking cr;

        /// <summary>
        /// How many times the player has been late to work this week.
        /// </summary>
        public int timesLateToWork { get; set; }
        /// <summary>
        /// How many times the player has been to work this week.
        /// </summary>
        public int daysWorked { get; set; }
        /// <summary>
        /// Return true if the week changed from sleep, or false if not.
        /// </summary>
        public bool weekChangedFromSleep { get; set; }
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
            ts.OnWeekChange += OnWeekEnd;
            timesLateToWork = PlayerPrefs.GetInt(GlobalConfig.key_TimesLateToWork);
            daysWorked = PlayerPrefs.GetInt(GlobalConfig.key_DaysWorkedInWeek);

            int a = PlayerPrefs.GetInt(GlobalConfig.key_WeekChangedFromSleep);
            switch(a) {
                case 0:
                    weekChangedFromSleep = false;
                    break;
                case 1:
                    weekChangedFromSleep = true;
                    break;
                default:
                    Debug.LogError("Week Changed From Sleep loaded in weird.");
                    break;
            }

            if (weekChangedFromSleep) {
                OnWeekEnd();
                PlayerPrefs.SetInt(GlobalConfig.key_WeekChangedFromSleep, 0);
                weekChangedFromSleep = false;
            }
        }
        
        public void OnWeekEnd() {
            StartCoroutine(LookOverWorkWeek());
        }

        IEnumerator LookOverWorkWeek () {
            //There are 5 days in the week the player will work
            yield return new WaitForSeconds(2);
            //Times Late
            if (timesLateToWork > 0) {
                PointsAndShow(CalculateLatePunishment(timesLateToWork), "Late Work Days Reported", true);
            }
            yield return new WaitForSeconds(0.5f);
            //Days Missed
            if (daysWorked < 5) {
                int missed = 5 - daysWorked;

                PointsAndShow(CalculateAbsencePunishment(missed), "Work Absences Reported", true);
            }
            yield return new WaitForSeconds(0.5f);
            //Perfect Attendance
            //NOTE: Has not been tested
            if (timesLateToWork == 0 && daysWorked <= 5) {
                PointsAndShow(CalculatePerfectAttendance(), "Perfect Attendance Rewared", false);
            }

            timesLateToWork = 0;
            daysWorked = 0;

            StopCoroutine(LookOverWorkWeek());
        }

        private int CalculateLatePunishment(int daysLate) {
            int output = 0;

            if (daysLate == 5) {
                //Late Everyday of the week.
                switch (cr.currentTier) {
                    case GlobalConfig.ClassTier.A:
                        break;
                    case GlobalConfig.ClassTier.B:
                        output = 7;
                        break;
                    case GlobalConfig.ClassTier.C:
                        output = 14;
                        break;
                    case GlobalConfig.ClassTier.D:
                        output = 20;
                        break;
                    case GlobalConfig.ClassTier.E:
                        output = 30;
                        break;
                    case GlobalConfig.ClassTier.F:
                        break;
                    case GlobalConfig.ClassTier.G:
                        break;
                }

                return output;
            }
            else {
                //Late for a portion of the week.

                int a = 0;

                switch (cr.currentTier) {

                    case GlobalConfig.ClassTier.A:
                        break;
                    case GlobalConfig.ClassTier.B:
                        a = 1;
                        break;
                    case GlobalConfig.ClassTier.C:
                        a = 2;
                        break;
                    case GlobalConfig.ClassTier.D:
                        a = 3;
                        break;
                    case GlobalConfig.ClassTier.E:
                        a = 4;
                        break;
                    case GlobalConfig.ClassTier.F:
                        break;
                    case GlobalConfig.ClassTier.G:
                        break;
                }

                output = daysLate * a;

                return output;
            }
        }

        private int CalculateAbsencePunishment(int daysMissed) {
            int output = 0;

            if (daysMissed == 5) {
                //Missed Everyday of the week.
                switch (cr.currentTier) {
                    case GlobalConfig.ClassTier.A:
                        break;
                    case GlobalConfig.ClassTier.B:
                        output = 14;
                        break;
                    case GlobalConfig.ClassTier.C:
                        output = 20;
                        break;
                    case GlobalConfig.ClassTier.D:
                        output = 30;
                        break;
                    case GlobalConfig.ClassTier.E:
                        output = 50;
                        break;
                    case GlobalConfig.ClassTier.F:
                        break;
                    case GlobalConfig.ClassTier.G:
                        break;
                }

                return output;
            } else {
                //Missed Work for a portion of the week.

                int a = 0;

                switch (cr.currentTier) {

                    case GlobalConfig.ClassTier.A:
                        break;
                    case GlobalConfig.ClassTier.B:
                        a = 2;
                        break;
                    case GlobalConfig.ClassTier.C:
                        a = 3;
                        break;
                    case GlobalConfig.ClassTier.D:
                        a = 4;
                        break;
                    case GlobalConfig.ClassTier.E:
                        a = 5;
                        break;
                    case GlobalConfig.ClassTier.F:
                        break;
                    case GlobalConfig.ClassTier.G:
                        break;
                }

                output = daysMissed * a;

                return output;
            }
        }

        private int CalculatePerfectAttendance () {
            int output = 0;

            switch (cr.currentTier) {

                case GlobalConfig.ClassTier.A:
                    output = 100;
                    break;
                case GlobalConfig.ClassTier.B:
                    output = 10;
                    break;
                case GlobalConfig.ClassTier.C:
                    output = 20;
                    break;
                case GlobalConfig.ClassTier.D:
                    output = 30;
                    break;
                case GlobalConfig.ClassTier.E:
                    output = 40;
                    break;
                case GlobalConfig.ClassTier.F:
                    break;
                case GlobalConfig.ClassTier.G:
                    break;
            }

            return output;
        }

        private void PointsAndShow(int incomingScore, string message, bool punishing) {
            if (punishing) {
                cr.SubtractPoints(incomingScore); 
            } else {
                cr.AddPoints(incomingScore);
            }

            if (incomingScore != 0) {
                cr.citizenCardAnimator.SetTrigger("Show");

                Color a = Color.white;
                Color b = Color.black;

                if (punishing) {
                    a = Color.red;
                    b = new Color(.263f, 0, 0, 1.0f); 
                } else {
                    a = Color.green;
                    b = new Color(0, .263f, 0, 1.0f);
                }

                InteractionSystem.Instance.DisplayNotification(a, b, message);
            }

            //Send Notification
        }
    }
}