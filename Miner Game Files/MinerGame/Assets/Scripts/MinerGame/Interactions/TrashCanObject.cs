using Assets.Scripts.MinerGame.PlayerFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MinerGame.Interactions {
    public class TrashCanObject : MonoBehaviour, IInteractable {
        #region Variables
        public bool nullifyOnInteract {
            get { return false; }

            set { }
        }

        public string notification {
            get {
                if (!playerInventory.hasWeapon) {
                    if (IncomingPoints() != 0) {
                        return "+" + IncomingPoints().ToString() + " Points";
                    }
                    else if ((playerInventory.trashOnHand != 0) && (IncomingPoints() == 0)) {
                        return notification = "Trash Dispensed";
                    }
                    else {
                        return string.Empty;
                    } 
                } else {
                    if (IncomingPoints() != 0) {
                        return "+" + IncomingPoints().ToString() + " Points & Weapon Trashed";
                    }
                    else if ((playerInventory.trashOnHand != 0) && (IncomingPoints() == 0)) {
                        return notification = "Trash Dispensed & Weapon Trashed";
                    }
                    else {
                        return string.Empty;
                    }
                }
            }

            set { }
        }

        public Color notificationCharacterColor {
            get { return Color.green; }

            set { }
        }

        public Color notificationOutlineColor {
            get { return new Color(0, .263f, 0, 1.0f); }

            set { }
        }

        /// <summary>
        /// The citizen ranking attached to the player.
        /// </summary>
        public CitizenRanking playerRanking;
        /// <summary>
        /// The inventory attached to the player.
        /// </summary>
        public PlayerInventory playerInventory;
        /// <summary>
        /// The Animator attached to the citizen card.
        /// </summary>
        public Animator citizenCardAnimator;
        #endregion

        private int IncomingPoints() {
            int output = 0;

            int a = 0;

            switch (playerRanking.currentTier) {
                case GlobalConfig.ClassTier.A:
                    break;
                case GlobalConfig.ClassTier.B:
                    a = 0;
                    break;
                case GlobalConfig.ClassTier.C:
                    a = 1;
                    break;
                case GlobalConfig.ClassTier.D:
                    a = 2;
                    break;
                case GlobalConfig.ClassTier.E:
                    a = 2;
                    break;
                case GlobalConfig.ClassTier.F:
                    a = 4;
                    break;
                case GlobalConfig.ClassTier.G:
                    break;
            }

            if (playerInventory.hasWeapon) {
                playerInventory.trashOnHand += 1;
                playerInventory.hasWeapon = false;
            }

            output = playerInventory.trashOnHand * a;

            return output;
        }

        public void InteractionFunction() {
            playerRanking.AddPoints(IncomingPoints());

            if (IncomingPoints() != 0) {
                citizenCardAnimator.SetTrigger("Show");
            }

            playerInventory.trashOnHand = 0;
        }
    }
}