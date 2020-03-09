using Assets.Scripts.MinerGame.PlayerFiles;
using Assets.Scripts.MinerGame.TimeSensitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame.Interactions {
    public class SightInteraction : MonoBehaviour {

        #region Variables
        /// <summary>
        /// Return true if camera is outdoors.
        /// </summary>
        public bool outdoorCamera;
        /// <summary>
        /// The Time system used within the scene.
        /// </summary>
        public TimeSystem ts;
        /// <summary>
        /// The Player's inventory.
        /// </summary>
        public PlayerInventory pi;
        /// <summary>
        /// The player's citizen ranking.
        /// </summary>
        public CitizenRanking cr;
        /// <summary>
        /// Beginning of curfew, and end of curfew.
        /// </summary>
        public int curfewBegin, curfewEnd;

        /// <summary>
        /// The Renderer attached to the FOV.
        /// </summary>
        private Renderer fovRenderer;
        /// <summary>
        /// The material the FOV uses.
        /// </summary>
        private Material fovMaterial;
        /// <summary>
        /// The initial alpha value of the FOV.
        /// </summary>
        private float initAlphaValue;
        #endregion

        private void Start() {
            fovRenderer = GetComponent<Renderer>();
            fovMaterial = fovRenderer.material;
            initAlphaValue = fovMaterial.color.a;
            fovMaterial.color = new Color(Color.red.r, Color.red.g, Color.red.b, initAlphaValue);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                fovMaterial.color = new Color(Color.green.r, Color.green.g, Color.green.b, initAlphaValue);
                CheckTrashStatus();
                CheckWeaponStatus();
                if (outdoorCamera) {
                    CheckCurfewStatus();
                }
            }
        }

        private void CheckWeaponStatus() {
            int a = 0;

            switch (cr.currentTier) {
                case GlobalConfig.ClassTier.A:
                    break;
                case GlobalConfig.ClassTier.B:
                    a = 4;
                    break;
                case GlobalConfig.ClassTier.C:
                    a = 6;
                    break;
                case GlobalConfig.ClassTier.D:
                    a = 6;
                    break;
                case GlobalConfig.ClassTier.E:
                    a = 8;
                    break;
                case GlobalConfig.ClassTier.F:
                    a = 8;
                    break;
                case GlobalConfig.ClassTier.G:
                    a = 10;
                    break;
            }

            if (pi.hasWeapon) {
                DockAndShow(a, "Possesion of a Deadly Weapon");
            }
        }

        private void CheckCurfewStatus() {
            int a = 0;

            switch (cr.currentTier) {
                case GlobalConfig.ClassTier.A:
                    break;
                case GlobalConfig.ClassTier.B:
                    a = 1;
                    break;
                case GlobalConfig.ClassTier.C:
                    a = 2;
                    break;
                case GlobalConfig.ClassTier.D:
                    a = 4;
                    break;
                case GlobalConfig.ClassTier.E:
                    a = 4;
                    break;
                case GlobalConfig.ClassTier.F:
                    a = 4;
                    break;
                case GlobalConfig.ClassTier.G:
                    a = 4;
                    break;
            }

            if ((ts.amPm.text == "PM" && ts.currentHour >= curfewBegin) || (ts.amPm.text == "AM" && ts.currentHour <= curfewEnd)) {
                DockAndShow(a, "Out Past Curfew");
            }

        }

        private void CheckTrashStatus() {
            if (pi.trashOnHand > 0) {
                
                int a = 0;

                switch (cr.currentTier) {
                    case GlobalConfig.ClassTier.A:
                        break;
                    case GlobalConfig.ClassTier.B:
                        a = 4;
                        break;
                    case GlobalConfig.ClassTier.C:
                        a = 2;
                        break;
                    case GlobalConfig.ClassTier.D:
                        a = 1;
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

                //NOTE: Perhaps dock points only once a day per object.
                DockAndShow(a, "Carrying Trash");
            }
        }

        private void DockAndShow (int dockingScore, string message) {
            cr.SubtractPoints(dockingScore);
            if (dockingScore != 0) {
                message = "-" + dockingScore + " " + message;
                cr.citizenCardAnimator.SetTrigger("Show");

                Color a = new Color(.263f, 0, 0, 1.0f);

                InteractionSystem.Instance.DisplayNotification(Color.red, a, message);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.tag == "Player") {
                fovMaterial.color = new Color(Color.red.r, Color.red.g, Color.red.b, initAlphaValue);
            }
        }
    }
}