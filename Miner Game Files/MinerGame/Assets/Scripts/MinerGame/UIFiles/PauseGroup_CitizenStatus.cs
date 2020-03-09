using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame.UIFiles {
    public class PauseGroup_CitizenStatus : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The Citizen Ranking attached to the player protagonist.
        /// </summary>
        public CitizenRanking playerRanking;
        /// <summary>
        /// The text component that displays the class tier value.
        /// </summary>
        public Text tierValue;
        /// <summary>
        /// The text component that displays the class points value.
        /// </summary>
        public Text classPointsValue;
        /// <summary>
        /// The slider that represents how many points the player has in relation to the max class points.
        /// </summary>
        public Slider pointsRepresentation;
        #endregion

        /// <summary>
        /// Loads the Citizen Status Data.
        /// </summary>
        public void LoadCitizenStatusData() {
            tierValue.text = playerRanking.currentTier.ToString();
            classPointsValue.text = playerRanking.currentClassPoints.ToString();
            pointsRepresentation.value = (float)playerRanking.currentClassPoints / (float)playerRanking.maxClassPoints;
        }
    }
}