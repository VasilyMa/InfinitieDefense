using System.Security.Cryptography;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace Client {
    public class StartGameMB : MonoBehaviour
    {
        public int Sounds;
        public int Music;
        public int Vibration;
        public int LVL;
        public ulong AllCoin;
        public int SceneNumber;
        public string PlayerID;
        public int PlayerUpgrade;
        public string[] TowerID;
        public int[] TowersUpgrade;
        public int CurrentWave;
        public int Rock;
        public int Coin;
        public int TutorialStage;
        public SaveSettings Save = new SaveSettings();
        private string path;
        public void Awake()
        {

            path = Path.Combine(Application.dataPath, "SaveSettings.json");
            if (File.Exists(path))
            {
                Save = JsonUtility.FromJson<SaveSettings>(File.ReadAllText(path));
                Sounds = Save.Sounds;
                Music = Save.Music;
                Vibration = Save.Vibration;
                LVL = Save.LVL;
                AllCoin = Save.AllCoin;
                SceneNumber = Save.SceneNumber;
                PlayerID = Save.PlayerID;
                CurrentWave = Save.CurrentWave;
                Rock = Save.Rock;
                Coin = Save.Coin;
                PlayerUpgrade = Save.PlayerUpgrade;
                if (Save.TutorialStage == 12)
                    SceneManager.LoadScene(2);
                else
                {
                    SceneManager.LoadScene(1);
                    TutorialStage = 0;
                    Save.TutorialStage = 0;
                }
                TowerID = new string[Save.TowerID.Length];
                TowersUpgrade = new int[Save.TowersUpgrade.Length];
                for (int i = 0; i < TowerID.Length; i++)
                {
                    TowerID[i] = Save.TowerID[i];
                    TowersUpgrade[i] = Save.TowersUpgrade[i];
                }
            }
            else
                SceneManager.LoadScene(1);
        }

    }
    [Serializable]
    public class SaveSettings
    {
        public int Sounds;
        public int Music;
        public int Vibration;
        public int LVL;
        public ulong AllCoin;
        public int SceneNumber;
        public string PlayerID;
        public int PlayerUpgrade;
        public string[] TowerID;
        public int[] TowersUpgrade;
        public int CurrentWave;
        public int Circle;
        public int Rock;
        public int Coin;
        public int TutorialStage;
    }
}
