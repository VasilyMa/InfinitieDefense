using System.Security.Cryptography;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;
using Client;
using System;
using System.IO;

namespace Client
{
    public class Saves
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
        public string path;
        public string VersionGame;
        //методы сохранений...
        #region
        public void SaveSounds(int value)
        {
            Sounds = value;
            Save.Sounds = Sounds;
            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }

        public void SaveMusic(int value)
        {
            Music = value;
            Save.Music = Music;
            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }

        public void SaveVibration(int value)
        {
            Vibration = value;
            Save.Vibration = Vibration;
            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }

        public void SaveLevel(int value)
        {
            LVL = value;
            Save.LVL = LVL;
            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SaveCoin(ulong value)
        {
            AllCoin = value;
            Save.AllCoin = AllCoin;
            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SaveSceneNumber(int value)
        {
            Save.SceneNumber = value;
            SceneNumber = value;

            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SaveTutorial(int value)
        {
            Save.TutorialStage = value;
            TutorialStage = value;

            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SavePlayerID(string value)
        {
            Save.PlayerID = value;
            PlayerID = value;

            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SaveTowerID(string[] value)
        {
            Save.TowerID = new string[value.Length];
            TowerID = new string[value.Length];
            for (int i = 0; i < TowerID.Length;i++)
            {
                TowerID[i] = value[i];
                Save.TowerID[i] = value[i];
            }
            
            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SaveCurrentWave(int value)
        {
            Save.CurrentWave = value;
            CurrentWave = value;

            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SaveRock(int value)
        {
            Rock = value;
            Save.Rock = value;

            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SaveCoin(int value)
        {
            Coin = value;
            Save.Coin = value;

            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SaveUpgrades(int[] value)
        {
            TowersUpgrade = new int[value.Length];
            Save.TowersUpgrade = new int[value.Length];
            for (int i = 0; i < value.Length;i++)
            {
                TowersUpgrade[i] = value[i];
                Save.TowersUpgrade[i] = value[i];
            }

            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
        public void SavePlayerUpgrade(int value)
        {
            PlayerUpgrade = value;
            Save.PlayerUpgrade = value;

            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }

        #endregion
        public void InitSave()
        {
            //в зависимости от того где запустили игру находим путь до json файла
#if UNITY_ANDROID && !UNITY_EDITOR
            path = Path.Combine(Application.persistentDataPath, "SaveSettings.json");
#else
            path = Path.Combine(Application.dataPath, "SaveSettings.json");
#endif
            //Если файл есть по заданому пути получаем значения из него
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
                    TutorialStage = Save.TutorialStage;
                else
                {
                    TutorialStage = 0;
                    Save.TutorialStage = 0;
                }
                TowerID = new string[Save.TowerID.Length];
                TowersUpgrade = new int[Save.TowersUpgrade.Length];
                for (int i = 0; i < TowerID.Length;i++)
                {
                    TowerID[i] = Save.TowerID[i];
                    TowersUpgrade[i] = Save.TowersUpgrade[i];
                }
            }
            //если нет, то записываем значения в SaveSettings и создаем файлик
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
                CreateTowerID();

                File.WriteAllText(path, JsonUtility.ToJson(Save));
            }
        }
        public int LoadSceneNumber()
        {
            int sceneNumber = 0;
#if UNITY_ANDROID && !UNITY_EDITOR
            path = Path.Combine(Application.persistentDataPath, "SaveSettings.json");
#else
            path = Path.Combine(Application.dataPath, "SaveSettings.json");
#endif
            if (File.Exists(path))
            {
                //File.Delete(path);
                Save = JsonUtility.FromJson<SaveSettings>(File.ReadAllText(path));
                sceneNumber = Save.SceneNumber;
            }
            else
            {
                sceneNumber = 1;
            }
            return sceneNumber;
        }
        public void CreateTowerID()
        {
            TowerID = new string[55];
            Save.TowerID = new string[55];
            TowersUpgrade = new int[55];
            Save.TowersUpgrade = new int[55];
            for (int i = 0; i < TowerID.Length;i++)
            {
                if(i == 0)
                {
                    TowerID[i] = "1tower";
                    Save.TowerID[i] = "1tower";
                }
                else
                {
                    TowerID[i] = "empty";
                    Save.TowerID[i] = "empty";
                }
                TowersUpgrade[i] = 0;
                Save.TowersUpgrade[i] = 0;
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
}
