using System;
using System.Collections.Generic;
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
        public float sanity;
        public List<ItemPlayer> items;
        public List<OnlineLoansPlayer> onlineLoans;
    }

    [Serializable]
    public class ItemPlayer
    {
        public string uniqueID;
        public string id;
        public string name;
        public int moneyPlus;
    }

    [Serializable]
    public class OnlineLoansPlayer
    {
        public OnlineLoansStatus status;
        public int totalRepayment;
        public int remainingDays;
        public int dailyOverduePenalty;
    }

    [Serializable]
    public class ModelMathQuestion
    {
        public string question;
        public List<int> options;
        public int correctAnswer;

        public ModelMathQuestion(string Question, List<int> Options, int CorrectAnswer)
        {
            question = Question;
            options = Options;
            correctAnswer = CorrectAnswer;
        }
    }

    [Serializable]
    public class ItemShopDatabase
    {
        public List<ItemShop> items;
    }

    [Serializable]
    public class ItemShop
    {
        public string id;
        public string name;
        public int moneyPlus;
        public int buy;
    }

    [Serializable]
    public class ModelOnlineLoans
    {
        public OnlineLoansStatus status;
        public int receivedAmount;
        public int loanTenureDays;
        public float dailyInterestRate;
        public int totalRepaymentAmount;
    }

    public enum OnlineLoansStatus
    {
        main,
        legal,
        illegal
    }
}
