using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame {
    public class CitizenRanking : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The animator attached to the Citizen Rank Card.
        /// </summary>
        public Animator citizenCardAnimator;
        /// <summary>
        /// The citizen's point card.
        /// </summary>
        public Transform citizenCard;
        /// <summary>
        /// The image representing how many points the citizen has.
        /// </summary>
        public Image fillbar;
        /// <summary>
        /// The text saying how many points the citizen has.
        /// </summary>
        public Text pointsValueText;
        /// <summary>
        /// The text displaying what class tier the citizen is in.
        /// </summary>
        public Text classLetterText;
        /// <summary>
        /// The current class tier of the citizen.
        /// </summary>
        public GlobalConfig.ClassTier currentTier = GlobalConfig.ClassTier.D;

        /// <summary>
        /// The current amount of points in the class of this citizen.
        /// </summary>
        public int currentClassPoints { get; private set; }
        /// <summary>
        /// The maximum amount of points you can have in a class.
        /// </summary>
        public int maxClassPoints { get; private set; }
        #endregion

        private void Start() {
            maxClassPoints = 100;
            LoadRanking();
        }

        private void LoadRanking() {
            currentTier = (GlobalConfig.ClassTier)PlayerPrefs.GetInt(GlobalConfig.key_ClassTier);
            currentClassPoints = PlayerPrefs.GetInt(GlobalConfig.key_ClassPoints);
        }

        private void Update() {
            citizenCard.LookAt(Camera.main.transform);

            DisplayClassPoints();
        }

        private void DisplayClassPoints() {
            pointsValueText.text = currentClassPoints + "/" + maxClassPoints;

            float percentage = (float)currentClassPoints / (float)maxClassPoints;
            fillbar.fillAmount = percentage;

            classLetterText.text = currentTier.ToString();
        }

        /// <summary>
        /// Adds to the current class points.
        /// </summary>
        /// <param name="addition">
        /// The points being added.
        /// </param>
        public void AddPoints (int addition) {
            currentClassPoints += addition;
            CheckForTierHeightening();
        }

        /// <summary>
        /// Subtracts from the current class points.
        /// </summary>
        /// <param name="subtraction">
        /// The points being subtracted.
        /// </param>
        public void SubtractPoints(int subtraction) {
            currentClassPoints -= subtraction;
            CheckForTierLowering();
        }

        private void CheckForTierHeightening() {
            if (currentClassPoints < maxClassPoints) {
                Debug.Log("Go a tier higher");
                bool wasAToBeginWith = false;

                switch (currentTier) {
                    case GlobalConfig.ClassTier.A:
                        Debug.Log("Can't go any tiers higher.");
                        wasAToBeginWith = true;
                        break;
                    case GlobalConfig.ClassTier.B:
                        currentTier = GlobalConfig.ClassTier.A;
                        break;
                    case GlobalConfig.ClassTier.C:
                        currentTier = GlobalConfig.ClassTier.B;
                        break;
                    case GlobalConfig.ClassTier.D:
                        currentTier = GlobalConfig.ClassTier.C;
                        break;
                    case GlobalConfig.ClassTier.E:
                        currentTier = GlobalConfig.ClassTier.D;
                        break;
                    case GlobalConfig.ClassTier.F:
                        currentTier = GlobalConfig.ClassTier.E;
                        break;
                    case GlobalConfig.ClassTier.G:
                        currentTier = GlobalConfig.ClassTier.F;
                        break;
                }

                if (wasAToBeginWith) {
                    currentClassPoints = 100;
                } else {
                    int a = currentClassPoints;

                    a -= 100;

                    currentClassPoints = 0;
                    
                    currentClassPoints += a;

                    CheckForTierHeightening();
                }
            }
        }

        private void CheckForTierLowering() {
            if (currentClassPoints < 0) {
                Debug.Log("Go a tier lower");
                bool wasGToBeginWith = false;

                switch (currentTier) {
                    case GlobalConfig.ClassTier.A:
                        currentTier = GlobalConfig.ClassTier.B;
                        break;
                    case GlobalConfig.ClassTier.B:
                        currentTier = GlobalConfig.ClassTier.C;
                        break;
                    case GlobalConfig.ClassTier.C:
                        currentTier = GlobalConfig.ClassTier.D;
                        break;
                    case GlobalConfig.ClassTier.D:
                        currentTier = GlobalConfig.ClassTier.E;
                        break;
                    case GlobalConfig.ClassTier.E:
                        currentTier = GlobalConfig.ClassTier.F;
                        break;
                    case GlobalConfig.ClassTier.F:
                        currentTier = GlobalConfig.ClassTier.G;
                        break;
                    case GlobalConfig.ClassTier.G:
                        wasGToBeginWith = true;
                        Debug.Log("Can't go any tiers lower");
                        break;
                }

                if (wasGToBeginWith) {
                    currentClassPoints = 0;
                } else {
                    int a = currentClassPoints;

                    currentClassPoints = 100;

                    currentClassPoints += a;

                    CheckForTierLowering();
                }
            }
        }
    }
}