using Assets.Scripts.MinerGame.PlayerFiles;
using Assets.Scripts.MinerGame.TimeSensitive;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.MinerGame.Interactions {

    public enum Meridiem { AM, PM }

    public class DoorObject : MonoBehaviour, IInteractable {
        #region Variables

        /// <summary>
        /// The name of the scene this door leads to.
        /// </summary>
        public string sceneName;
        /// <summary>
        /// Return true if door is time operative, or false if not.
        /// </summary>
        public bool timeOperative;
        /// <summary>
        /// Return true if this a door that leads to work, or false if not.
        /// </summary>
        public bool workDoor = false;

        /// <summary>
        /// This door's opening hour.
        /// </summary>
        public int openingHour;
        /// <summary>
        /// This door's opening meridiem.
        /// </summary>
        public Meridiem openingMeridiem;
        /// <summary>
        /// This door's closing hour.
        /// </summary>
        public int closingHour;
        /// <summary>
        /// This door's closing meridiem.
        /// </summary>
        public Meridiem closingMeridiem;
        /// <summary>
        /// The list of days this door will be closed no matter what.
        /// </summary>
        public List<int> daysClosed = new List<int>();

        /// <summary>
        /// The player's inventory.
        /// </summary>
        private PlayerInventory pi;
        /// <summary>
        /// The Animator attached to this door.
        /// </summary>
        private Animator attachedAnimator;
        /// <summary>
        /// The Time System in this scene this door will read from.
        /// </summary>
        private TimeSystem ts;
        /// <summary>
        /// Return true if door is openable, or false if not.
        /// </summary>
        private bool openable = true;

        public bool nullifyOnInteract {
            get { return true; }

            set { }
        }

        public string notification {
            get {
                if (openable) {
                    return "Used Door";
                } else {
                    return "Door Locked";
                }
            }

            set { }
        }

        public Color notificationCharacterColor {
            get { return Color.grey; }

            set { }
        }

        public Color notificationOutlineColor {
            get { return new Color(.263f, .263f, .263f, 1.0f); }

            set { }
        }
        #endregion

        private void Start() {
            ts = TimeSystem.Instance;

            pi = FindObjectOfType<PlayerInventory>();
            Debug.Log(pi.name);

            attachedAnimator = GetComponent<Animator>();
            if (attachedAnimator == null) {
                Debug.LogError("No Animator found attached to " + name, gameObject);
            }
        }

        private void Update() {
            if (daysClosed.Contains(ts.currentDayOfTheWeek)) {
                openable = false;
                return;
            }
            TimedDoor();
        }

        private void TimedDoor() {
            if (timeOperative) {
                if (ts.currentHour == openingHour && ts.amPm.text == openingMeridiem.ToString()) {
                    openable = true;
                }
                if (ts.currentHour == closingHour && ts.amPm.text == closingMeridiem.ToString()) {
                    openable = false;
                }
            } else {
                openable = true;
            }
        }

        public void InteractionFunction() {
            if (openable) {
                PlayerMovement.Instance.ableToMove = false;
                InteractionSystem.Instance.ableToAction = false;

                if (sceneName == null) {
                    Debug.LogError("No Scene to load into for this door.");
                } else {
                    ts.timePaused = true;

                    if (workDoor) {
                        WeeklyTasks wt = WeeklyTasks.Instance;
                        if (ts.currentHour > 8) {
                            Debug.LogWarning("Player is late to work.");
                            wt.timesLateToWork += 1;
                        }
                        wt.daysWorked += 1;

                        GlobalConfig.SaveWeeklyTasks(wt);
                    }

                    GlobalConfig.SaveTime(ts);
                    
                    GlobalConfig.SaveCitizenStatus(FindObjectOfType<CitizenRanking>());

                    bool stinkBool = PlayerHealth.Instance.playerStinks;

                    if (stinkBool) {
                        PlayerPrefs.SetInt(GlobalConfig.key_Showering_PlayerStinks, 1);
                    } else {
                        PlayerPrefs.SetInt(GlobalConfig.key_Showering_PlayerStinks, 0);
                    }

                    #region Player Inventory
                    PlayerPrefs.SetInt(GlobalConfig.key_Trash, pi.trashOnHand);
                    PlayerPrefs.SetInt(GlobalConfig.key_Money, pi.money);
                    if (pi.hasWeapon) {
                        PlayerPrefs.SetInt(GlobalConfig.key_Weapon, 1);
                    }
                    else {
                        PlayerPrefs.SetInt(GlobalConfig.key_Weapon, 0);
                    } 
                    #endregion

                    attachedAnimator.SetTrigger("Open");

                    SceneTransitioner.FadeOut(sceneName); 
                }
            }
        }
    }

    [CustomEditor(typeof(DoorObject))]
    public class EditorForDoorObject : Editor {
        public override void OnInspectorGUI() {
            var doorObj = target as DoorObject;

            doorObj.sceneName = EditorGUILayout.TextField("Scene Name:", doorObj.sceneName);

            doorObj.timeOperative = GUILayout.Toggle(doorObj.timeOperative, "Time Operative");

            if (doorObj.timeOperative) {
                doorObj.openingHour = EditorGUILayout.IntField("OP Hour:", doorObj.openingHour);
                doorObj.openingMeridiem = (Meridiem)EditorGUILayout.EnumPopup("OP Midday:", doorObj.openingMeridiem);
                doorObj.closingHour = EditorGUILayout.IntField("CLOSE Hour:", doorObj.closingHour);
                doorObj.closingMeridiem = (Meridiem)EditorGUILayout.EnumPopup("CLOSE Midday:", doorObj.closingMeridiem);

                var list = doorObj.daysClosed;
                int newCount = Mathf.Max(0, EditorGUILayout.IntField("Size:", list.Count));
                while (newCount < list.Count)
                    list.RemoveAt(list.Count - 1);
                while (newCount > list.Count)
                    list.Add(0);

                for (int i = 0; i < list.Count; i++) {
                    list[i] = EditorGUILayout.IntField("Element " + i + ":", list[i]);
                }
            }
        }
    }
}