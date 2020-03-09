using Assets.Scripts.MinerGame.PlayerFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame.UIFiles {
    public class PauseGroup_Inventory : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The Player's Inventory.
        /// </summary>
        public PlayerInventory playerPossessions;
        /// <summary>
        /// The text component that displays how much money the player has.
        /// </summary>
        public Text moneyValue;
        /// <summary>
        /// The text component that displays how much trash the player has.
        /// </summary>
        public Text trashValue;
        /// <summary>
        /// The toggle component that displays whether the player has a weapon or not.
        /// </summary>
        public Toggle weaponValue;
        #endregion

        /// <summary>
        /// Loads the Inventory Data.
        /// </summary>
        public void LoadInventoryData () {
            moneyValue.text = playerPossessions.money.ToString();
            trashValue.text = playerPossessions.trashOnHand.ToString();
            weaponValue.isOn = playerPossessions.hasWeapon;
        }
    }
}