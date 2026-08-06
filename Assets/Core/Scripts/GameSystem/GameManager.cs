using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Game.System
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Settings settings;
        // public PlayerData playerData;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            settings = LoadData<Settings>("settings");
            // playerData = LoadData<PlayerData>("playerData") ?? new();
        }

        private void NewData<T>(string file) where T : new()
        {
            TextAsset jsonAsset = Resources.Load<TextAsset>(file);

            if (jsonAsset != null)
            {
                T newData = JsonUtility.FromJson<T>(jsonAsset.text);
                SaveData(newData, file);
            }
        }

        public void NewGame()
        {
            NewData<CurrentDateTime>("dateTime");
        }

        public void SaveData<T>(T data, string file)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(FilePath(file), json);
        }

        public T LoadData<T>(string file) where T : new()
        {
            string filePath = FilePath(file);

            // Ambil data json dari persistent (local data device)
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(json);
            }

            // Ambil data json dari folder /Resources
            TextAsset jsonAsset = Resources.Load<TextAsset>(file);
            if (jsonAsset != null)
            {
                return JsonUtility.FromJson<T>(jsonAsset.text);
            }

            Debug.LogWarning($"Path: {filePath} tidak ditemukan maupun di resources, membuat objek baru {typeof(T).Name}");
            return new T();
        }

        public string FilePath(string file) { return $"{Application.persistentDataPath}/{file}.json"; }

        #region Utility
        public string CurrencyFormat(int money)
        {
            return money.ToString("N0", new CultureInfo("id-ID"));
        }
        #endregion
    }
}
