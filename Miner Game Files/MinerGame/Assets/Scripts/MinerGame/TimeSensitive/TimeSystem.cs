using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame.TimeSensitive {
    public class TimeSystem : MonoBehaviour {
        #region Variables
        /// <summary>
        /// The instance of this class.
        /// </summary>
        public static TimeSystem Instance { get; private set; }

        /// <summary>
        /// The slider that represents time passing by.
        /// </summary>
        public Slider timeSlider;
        /// <summary>
        /// The text component that represents the current day of the week.
        /// </summary>
        public Text dayOfTheWeek;
        /// <summary>
        /// The text component that tells the player whether it's AM or PM.
        /// </summary>
        public Text amPm;
        /// <summary>
        /// The Text component that tells the player the hour.
        /// </summary>
        public Text hourDisplay;
        /// <summary>
        /// The Text component that tells the player how many weeks have gone by.
        /// </summary>
        public Text weekText;
        /// <summary>
        /// The current day of the week the player is on.
        /// </summary>
        [Tooltip("Make sure this is a number between 1 and 7, both inclusive.")]
        public int currentDayOfTheWeek;
        /// <summary>
        /// The rate at which Time accelerates.
        /// </summary>
        public float timeAcceleration;
        /// <summary>
        /// Return true if you want to pause time, or false if not.
        /// </summary>
        public bool timePaused;
        /// <summary>
        /// Is called when the week changes in the time system.
        /// </summary>
        public event Action OnWeekChange;

        /// <summary>
        /// Words representing the days of the week.
        /// </summary>
        private List<string> weekdays = new List<string>(7) {
            "Sun.", "Mon.", "Tues.", "Wed.", "Thurs.", "Fri.", "Sat."
        };
        /// <summary>
        /// Numbers representing the values needed to calculated hours of the day.
        /// </summary>
        private List<float> hourValues = new List<float>();

        /// <summary>
        /// The value of the last tick.
        /// </summary>
        public float lastTick { get; private set; }
        /// <summary>
        /// The value of the last hour.
        /// </summary>
        public int currentHour { get; private set; }
        /// <summary>
        /// The week the player is on.
        /// </summary>
        public int week { get; private set; }

        /// <summary>
        /// The current time.
        /// </summary>
        private float currTime;
        #endregion

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Debug.LogError("There is more than one " + this + "in the scene!");
                Destroy(gameObject);
            }
        }

        private List<float> CalculateHourValues() {
            List<float> output = new List<float>();

            float first = 1f / 12f;

            for (float i = 0f; i < 12f; i++) {
                output.Add(first * i);
            }

            return output;
        }

        private void Start() {
            timePaused = true;
            currentHour = 12;
            hourValues = CalculateHourValues();
            
            LoadTime();

            hourDisplay.text = TheHour(KeepTime()) + ":00 " + amPm.text;
            weekText.text = "Week: " + week;

            if (currentDayOfTheWeek < 1 || currentDayOfTheWeek > 7) {
                Debug.LogError("The current day of the week was not set to an acceptible number.");
            }

            dayOfTheWeek.text = weekdays[currentDayOfTheWeek - 1];
        }

        private void LoadTime() {
            currTime =  PlayerPrefs.GetFloat(GlobalConfig.key_Time);
            amPm.text = PlayerPrefs.GetString(GlobalConfig.key_Meridiem);
            currentDayOfTheWeek = PlayerPrefs.GetInt(GlobalConfig.key_Day);
            week = PlayerPrefs.GetInt(GlobalConfig.key_Week);
        }

        private void Update() {
            if (!timePaused) {
                hourDisplay.text = TheHour(KeepTime()) + ":00 " + amPm.text;
            }
        }

        /// <summary>
        /// Gets the current hour based on the value you give it.
        /// </summary>
        /// <param name="currentTime">
        /// The current time.
        /// </param>
        /// <returns>
        /// The hour.
        /// </returns>
        public int TheHour(float currentTime) {
            int output = currentHour;
            int nextHour;

            if (currentHour != 12) {
                nextHour = currentHour + 1;
            } else {
                currentHour = 0;
                nextHour = 1;
            }

            float currHrValue = hourValues[currentHour];
            float nxtHrValue;

            if (nextHour != 12) {
                nxtHrValue = hourValues[nextHour];
                if (currentTime > nxtHrValue) {
                    currentHour += 1;
                }
            }

            output = currentHour;

            if (currentHour == 0) {
                output = 12;
            }

            return output;
        }

        private float KeepTime() {
            //float output = startingTime;
            float output = 0;

            TimeRepeatCheck(lastTick);

            //output = Mathf.Repeat(output + (Time.timeSinceLevelLoad / 60), 1);
            currTime = Mathf.Repeat(currTime + (timeAcceleration * Time.deltaTime), 1);

            if (TimeRepeatCheck(currTime)) {
                RecognizeMeridiem();
            }

            timeSlider.value = currTime;
            output = currTime;

            return output;
        }

        private bool TimeRepeatCheck(float insertedTime) {
            bool output = false;

            if ((Mathf.Floor(lastTick * 10) == 9) && (Mathf.Floor(insertedTime * 10) == 0)) {
                Debug.Log("Midday has happened.");
                currentHour = 12;
                output = true;
            }

            lastTick = insertedTime;

            return output;
        }

        public void RecognizeMeridiem() {
            if (amPm.text == "AM") {
                amPm.text = "PM";
            } else if (amPm.text == "PM") {
                amPm.text = "AM";

                if (currentDayOfTheWeek != 7) {
                    dayOfTheWeek.text = weekdays[currentDayOfTheWeek];
                    currentDayOfTheWeek += 1;
                    weekText.text = "Week: " + week;
                } else {
                    Debug.Log("Week is changing.");
                    if (OnWeekChange != null) {
                        OnWeekChange();
                    }
                    week++;
                    dayOfTheWeek.text = weekdays[0];
                    currentDayOfTheWeek = 1;
                    weekText.text = "Week: " + week;
                }
            } else {
                Debug.LogError("Something has gone terribly wrong with the Meridiem");
            }
        }
    }
}
