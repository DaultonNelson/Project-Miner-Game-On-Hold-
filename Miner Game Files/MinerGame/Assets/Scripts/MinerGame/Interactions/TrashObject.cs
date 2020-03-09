using Assets.Scripts.MinerGame.PlayerFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MinerGame.Interactions {
    public class TrashObject : MonoBehaviour, IInteractable {
        #region Variables
        public bool nullifyOnInteract {
            get { return true; }

            set { }
        }

        public string notification {
            get { return "+1 Trash"; }

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

        public void InteractionFunction() {
            PlayerInventory pi = FindObjectOfType<PlayerInventory>();
            if (pi != null) {
                pi.trashOnHand++;
            }
            Destroy(gameObject);
        }
    }
}
