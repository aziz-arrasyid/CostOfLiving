using System;
using UnityEngine;

namespace Game.System
{
    [Serializable]
    public class Settings
    {
        public float BGMVolume;
        public float SFXVolume;
    }

    [Serializable]
    public class CurrentDateTime
    {
        public string currentDateTime; // tahun-bulan-hari
    }

    [Serializable]
    public class PlayerData
    {
        public int money;
    }
}
