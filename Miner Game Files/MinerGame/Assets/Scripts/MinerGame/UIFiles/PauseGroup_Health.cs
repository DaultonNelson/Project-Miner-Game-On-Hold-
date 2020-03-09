using Assets.Scripts.MinerGame.PlayerFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame.UIFiles {
    public class PauseGroup_Health : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The player's health.
        /// </summary>
        public PlayerHealth health;
        /// <summary>
        /// The text component that displays the last slept value.
        /// </summary>
        public Text lastSleptValue;

        /// <summary>
        /// The text component that displays the last week showered value.
        /// </summary>
        public Text lastShoweredValue;
        #endregion

        /// <summary>
        /// Loads the Health data.
        /// </summary>
        public void LoadHealthData() {
            lastSleptValue.text = "week: <b>" + health.lastWeekSlept + "</b>  day: <b>" + GetWeekdayName(health.lastDaySlept) + "</b>  hour: <b>" + health.lastHourSlept + " AM</b>";

            lastShoweredValue.text = "week: <b>" + health.lastWeekShowered + "</b>  day: <b>" + GetWeekdayName(health.lastDayShowered) + "</b>  hour: <b>" + health.lastHourShowered + " " + health.showeredMeridiem + "</b>";
        }

        /// <summary>
        /// Gets the Weekday name based off the value given.
        /// </summary>
        /// <param name="dayValue">
        /// The weekday value.
        /// </param>
        /// <returns>
        /// The name of the day.
        /// </returns>
        private string GetWeekdayName (int dayValue) {
            string output = string.Empty;

            switch (dayValue) {
                case 1:
                    output = "Sunday";
                    break;
                case 2:
                    output = "Monday";
                    break;
                case 3:
                    output = "Tuesday";
                    break;
                case 4:
                    output = "Wednesday";
                    break;
                case 5:
                    output = "Thursday";
                    break;
                case 6:
                    output = "Friday";
                    break;
                case 7:
                    output = "Saturday";
                    break;
                default:
                    Debug.LogError("Something has gone wrong when trying to find the day of the week.");
                    break;
            }

            return output;
        }
    }
}