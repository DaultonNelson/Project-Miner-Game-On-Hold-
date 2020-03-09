using Assets.Scripts.MinerGame.PlayerFiles;
using Assets.Scripts.MinerGame.TimeSensitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MinerGame.Interactions {
    public class BedObject : MonoBehaviour, IInteractable {
        public bool nullifyOnInteract {
            get { return true; }

            set { }
        }

        public string notification {
            get { return "Sleeping..."; }

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

        public void InteractionFunction() {
            TimeSystem.Instance.timePaused = true;
            PlayerMovement.Instance.ableToMove = false;
            InteractionSystem.Instance.ableToAction = false;

            GlobalConfig.SaveForMorning(TimeSystem.Instance.currentDayOfTheWeek);
            PlayerHealth.Instance.SetLastTimePlayerSlept();

            bool stinkBool = PlayerHealth.Instance.playerStinks;

            if (stinkBool) {
                PlayerPrefs.SetInt(GlobalConfig.key_Showering_PlayerStinks, 1);
            }
            else {
                PlayerPrefs.SetInt(GlobalConfig.key_Showering_PlayerStinks, 0);
            }

            GlobalConfig.SaveSleepHealth(PlayerHealth.Instance);

            SceneTransitioner.FadeOut(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
