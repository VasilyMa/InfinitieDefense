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
        public int TutorialState;
        public string PlayerID;
        public string[] TowerID;
        public int CurrentWave;
        public int Circle;

        public SaveSettings Save = new SaveSettings();
        public string path;
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
            Save.TutorialState = value;
            TutorialState = value;

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
        public void SaveCircle(int value)
        {
            Save.Circle = value;
            Circle = value;

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
                TutorialState = Save.TutorialState;
                PlayerID = Save.PlayerID;
                CurrentWave = Save.CurrentWave;
                Circle = Save.Circle;
            }
            //если нет, то записываем значения в SaveSettings и создаем файлик
            else
            {
                Sounds = 1;
                Music = 1;
                Vibration = 1;
                LVL = 1;
                AllCoin = 900;
                SceneNumber = 1;
                TutorialState = 1;
                PlayerID = "1level";
                CurrentWave = 0;
                Circle = 0;


                Save.Sounds = Sounds;
                Save.Music = Music;
                Save.Vibration = Vibration;
                Save.LVL = LVL;
                Save.AllCoin = AllCoin;
                Save.SceneNumber = SceneNumber;
                Save.TutorialState = TutorialState;
                Save.PlayerID = PlayerID;
                Save.CurrentWave = CurrentWave;
                Save.Circle = Circle;

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

        [Serializable]
        public class SaveSettings
        {
            public int Sounds;
            public int Music;
            public int Vibration;
            public int LVL;
            public ulong AllCoin;
            public int SceneNumber;
            public int TutorialState;
            public string PlayerID;
            public string[] TowerID;
            public int CurrentWave;
            public int Circle;
        }
    }
}
