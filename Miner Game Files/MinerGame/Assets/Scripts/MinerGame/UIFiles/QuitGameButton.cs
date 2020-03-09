using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MinerGame.Scripts.UIFiles {
    public class QuitGameButton : MonoBehaviour {
        public void QuitGame() {
            Debug.Log("Quit Game");
            Application.Quit();
        }
    }
}