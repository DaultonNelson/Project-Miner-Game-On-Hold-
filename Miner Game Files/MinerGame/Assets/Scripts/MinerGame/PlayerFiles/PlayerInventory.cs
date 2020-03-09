using Assets.Scripts.MinerGame.Interactions;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame.PlayerFiles {
    public class PlayerInventory : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The instance of this class.
        /// </summary>
        public static PlayerInventory Instance { get; private set; }

        /// <summary>
        /// The trash the player has on hand;
        /// </summary>
        public int trashOnHand = 0;
        /// <summary>
        /// The money the player has on hand.
        /// </summary>
        public int money = 400;
        /// <summary>
        /// Return true if player has weapon, or false if not.
        /// </summary>
        public bool hasWeapon = false;
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
            trashOnHand = PlayerPrefs.GetInt(GlobalConfig.key_Trash);

            int gain = PlayerPrefs.GetInt(GlobalConfig.key_GainedMoney);
            if (gain > 0) {
                StartCoroutine(NotifyMoneyMadeFromWork(gain));
            }
            money = PlayerPrefs.GetInt(GlobalConfig.key_Money);

            int a = PlayerPrefs.GetInt(GlobalConfig.key_Weapon);
            switch (a) {
                case 0:
                    hasWeapon = false;
                    break;
                case 1:
                    hasWeapon = true;
                    break;
                default:
                    Debug.LogWarning("Could not decern wether the player has a weapon saved or not.");
                    break;
            }
        }

        IEnumerator NotifyMoneyMadeFromWork(int gained) {
            yield return new WaitForSeconds(2);
            Color b = new Color(0, 0.263f, 0, 1.0f);
            string c = "+" + gained + " Money";
            InteractionSystem.Instance.DisplayNotification(Color.green, b, c);
            PlayerPrefs.SetInt(GlobalConfig.key_GainedMoney, 0);
            StopCoroutine(NotifyMoneyMadeFromWork(0));
        }
    }
}