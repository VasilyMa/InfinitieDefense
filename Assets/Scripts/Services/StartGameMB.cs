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
        public string VersionGame;
        public SaveSettings Save = new SaveSettings();
        private string path;
        public void Start()
        {
            VersionGame = Application.version;
            path = Path.Combine(Application.dataPath, "SaveSettings.json");
            if (File.Exists(path))
            {
                Save = JsonUtility.FromJson<SaveSettings>(File.ReadAllText(path));
                if (Save.VersionGame == VersionGame)
                {
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
                        SceneManager.LoadScene(Save.SceneNumber);
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
                {
                    File.Delete(path);
                    Sounds = 1;
                    Music = 1;
                    Vibration = 1;
                    LVL = 1;
                    AllCoin = 0;
                    SceneNumber = 1;
                    TutorialStage = 1;
                    PlayerID = "1level";
                    CurrentWave = -1;
                    Rock = 0;
                    Coin = Save.Coin;
                    PlayerUpgrade = 0;
                    TutorialStage = 0;
                    VersionGame = Application.version;

                    Save.Sounds = Sounds;
                    Save.Music = Music;
                    Save.Vibration = Vibration;
                    Save.LVL = LVL;
                    Save.AllCoin = AllCoin;
                    Save.SceneNumber = SceneNumber;
                    Save.CurrentWave = CurrentWave;
                    Save.Rock = Rock;
                    Save.Coin = Coin;
                    Save.TutorialStage = TutorialStage;
                    Save.VersionGame = VersionGame;
                    Save.TutorialStage = TutorialStage;
                    Save.PlayerUpgrade = PlayerUpgrade;
                    File.WriteAllText(path, JsonUtility.ToJson(Save));
                    SceneManager.LoadScene(Save.SceneNumber);
                }
            }
            else
            {
                Sounds = 1;
                Music = 1;
                Vibration = 1;
                LVL = 1;
                AllCoin = 0;
                SceneNumber = 1;
                TutorialStage = 1;
                PlayerID = "1level";
                CurrentWave = -1;
                Rock = 0;
                Coin = 0;
                PlayerUpgrade = 0;
                TutorialStage = 0;
                VersionGame = Application.version;

                Save.Sounds = Sounds;
                Save.Music = Music;
                Save.Vibration = Vibration;
                Save.LVL = LVL;
                Save.AllCoin = AllCoin;
                Save.SceneNumber = SceneNumber;
                Save.PlayerID = PlayerID;
                Save.PlayerUpgrade = PlayerUpgrade;
                Save.CurrentWave = CurrentWave;
                Save.Rock = Rock;
                Save.Coin = Coin;
                Save.TutorialStage = TutorialStage;
                Save.VersionGame = VersionGame;

                File.WriteAllText(path, JsonUtility.ToJson(Save));
                SceneManager.LoadScene(Save.SceneNumber);
            }    
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
        public string VersionGame;
    }
}
