using Assets.Scripts.MinerGame.PlayerFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MinerGame.Interactions {
    public class WeaponObject : MonoBehaviour, IInteractable {
        #region Variables
        public bool nullifyOnInteract {
            get { return true; }

            set { }
        }

        public string notification {
            get { return "Got Weapon"; }

            set { }
        }

        public Color notificationCharacterColor {
            get { return Color.grey; }

            set { }
        }

        public Color notificationOutlineColor {
            get { return new Color (.25f, .25f, .25f, 1.0f); }

            set { }
        }
        #endregion

        public void InteractionFunction() {
            PlayerInventory pi = FindObjectOfType<PlayerInventory>();
            if (pi != null) {
                if (!pi.hasWeapon) {
                    pi.hasWeapon = true;
                }
                Destroy(gameObject);
            }
        }
    }
}