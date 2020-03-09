using Assets.Scripts.MinerGame.Interactions;
using Assets.Scripts.MinerGame.PlayerFiles;
using Assets.Scripts.MinerGame.TimeSensitive;
using Assets.Scripts.MinerGame.UIFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MinerGame {
    public class SceneTransitioner : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The instance of this class.
        /// </summary>
        public static SceneTransitioner Instance { get; private set; }

        /// <summary>
        /// Return true if scene has multiple spawn points.
        /// </summary>
        public bool multiSpawnScene = false;
        /// <summary>
        /// The list of indexes this manager should expect.
        /// </summary>
        public List<int> expectedIndexes = new List<int>();
        /// <summary>
        /// The list of spawn point GameObjects in the scene.
        /// </summary>
        public List<Transform> spawnPoints = new List<Transform>();
        /// <summary>
        /// The GameObject holding the protagonist information.
        /// </summary>
        public Transform protagonistObject;

        /// <summary>
        /// The Animator attached to this object.
        /// </summary>
        private static Animator attachedAnimator;
        /// <summary>
        /// The next scene the transitioner is loading.
        /// </summary>
        private static string nextScene = string.Empty;
        #endregion

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Debug.LogError("There is more than one " + this + "in the scene!");
                Destroy(gameObject);
            }

            if (multiSpawnScene) {
                SpawnSomewhere(); 
            }
        }

        private void SpawnSomewhere() {
            if (expectedIndexes.Count <= 0 || spawnPoints.Count<= 0 || protagonistObject == null) {
                Debug.LogError("One of the variables for the multi point spawner wasn't filled.");
                return;
            }

            int prevInd = PlayerPrefs.GetInt(GlobalConfig.key_PreviousSceneIndex);

            if (expectedIndexes.Contains(prevInd)) {
                protagonistObject.position = spawnPoints[expectedIndexes.IndexOf(prevInd)].position;
            } else {
                Debug.LogWarning("Could not find previous index value in expected indexes list.");
                protagonistObject.position = spawnPoints.Last().position;
            }
        }

        private void Start() {
            attachedAnimator = GetComponent<Animator>();
            if (attachedAnimator == null) {
                Debug.LogError("No Animator found attached to " + name, gameObject);
            }
        }

        /// <summary>
        /// Fades the scene out with a transition.
        /// </summary>
        /// <param name="comingScene">
        /// The next scene the transitioner will load into.
        /// </param>
        public static void FadeOut (string comingScene) {
            nextScene = comingScene;
            attachedAnimator.SetTrigger("FadeOut");
        }

        /// <summary>
        /// Loads the next scene.
        /// </summary>
        public void LoadNextScene () {
            if (!multiSpawnScene) {
                GlobalConfig.SaveLevelIndex();
            }
            SceneManager.LoadScene(nextScene);
        }

        public void SceneFullyTransitionedTo () {
            PauseMenu.Instance.ableToPause = true;
            TimeSystem.Instance.timePaused = false;
            PlayerMovement.Instance.ableToMove = true;
            InteractionSystem.Instance.ableToAction = true;
        }
    }
}