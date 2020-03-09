using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MinerGame.TimeSensitive {
    public class WorkDay : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The Time System this class works off of.
        /// </summary>
        public TimeSystem ts;
        /// <summary>
        /// The name of the scene the player will transition to once work is over.
        /// </summary>
        public string sceneName;

        /// <summary>
        /// The value of the previous hour.
        /// </summary>
        private int previousHour = 0;
        /// <summary>
        /// The value of the hour the player entered work in.
        /// </summary>
        private int enteredHour = 0;
        #endregion

        private void Start() {
            StartCoroutine(SetEnteredHour());
            StartCoroutine(CheckForClockOut());
        }

        private int CalculateHoursWorked() {
            int worked = 0;
            
            if (enteredHour < 4) {
                worked = 4 - enteredHour;
            }

            if (enteredHour > 4) {
                worked = 4 + (12 - enteredHour);
            }

            return worked;
        }

        IEnumerator SetEnteredHour() {
            yield return new WaitForSeconds(.5f);
            
            enteredHour = ts.currentHour;

            StopCoroutine(SetEnteredHour());
        }

        IEnumerator CheckForClockOut () {
            previousHour = ts.currentHour;

            yield return new WaitForSeconds(.5f);

            if ((previousHour == 3) && (ts.currentHour == 4)) {
                if (ts.amPm.text == "PM") {
                    LeaveWork();
                }
            }

            if ((ts.amPm.text == "PM") && (ts.currentHour > 4)) {
                LeaveWork();
            }

            StartCoroutine(CheckForClockOut());
        }

        private void LeaveWork () {
            Debug.Log("Clock out time reached");
            ts.timePaused = true;

            if (sceneName == null) {
                Debug.LogError("No Scene to load into for this door.");
            } else {
                int previousMoneyValue = PlayerPrefs.GetInt(GlobalConfig.key_Money);
                int gainedMoney = CalculateHoursWorked() * 100;
                PlayerPrefs.SetInt(GlobalConfig.key_GainedMoney, gainedMoney);
                previousMoneyValue += gainedMoney;
                PlayerPrefs.SetInt(GlobalConfig.key_Money, previousMoneyValue);

                GlobalConfig.SaveTime(ts);

                SceneTransitioner.FadeOut(sceneName);
            }

            StopCoroutine(CheckForClockOut());
        }

        //private bool TimeRepeatCheck(float insertedTime) {
        //    bool output = false;

        //    if ((Mathf.Floor(lastTick * 10) == 9) && (Mathf.Floor(insertedTime * 10) == 0)) {
        //        Debug.Log("Midday has happened.");
        //        currentHour = 12;
        //        output = true;
        //    }

        //    lastTick = insertedTime;

        //    return output;
        //}
    }
}
