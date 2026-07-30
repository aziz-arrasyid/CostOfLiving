using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

[Serializable]
public struct GameTime
{
    [Range(1, 23)]
    public int hour;
    [Range(0, 59)]
    public int minute;

    public readonly float ToFloat()
    {
        return hour + (minute / 60f);
    }
}

namespace Game.System
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { private set; get; }

        [Header("Base Time Setting")]
        [Range(1, 60)]
        [SerializeField] private float realMinutesPerDay;
        [SerializeField] private GameTime startTime = new() { hour = 6, minute = 30 };
        [SerializeField] private GameTime endTime = new() { hour = 17, minute = 0 };

        [Space]

        [Header("Working Time Setting")]
        [SerializeField] private GameTime startWork = new() { hour = 7, minute = 30 };
        [SerializeField] private GameTime endWork = new() { hour = 13, minute = 0 };

        private bool workingHoursReadyActive;

        public bool WorkingHoursReadyActive
        {
            get => workingHoursReadyActive;
            set
            {
                if (workingHoursReadyActive != value)
                {
                    workingHoursReadyActive = value;
                }
            }
        }

        [Space]

        [Header("Debt Collector Chase Time Setting")]
        [SerializeField] private GameTime startChase = new() { hour = 3, minute = 0 };
        [SerializeField] private GameTime endChase = new() { hour = 15, minute = 16 };

        private bool debtCrisisChaseReadyActive;

        public bool DebtCrisisChaseReadyActive
        {
            get => debtCrisisChaseReadyActive;
            set
            {
                if (debtCrisisChaseReadyActive != value)
                {
                    debtCrisisChaseReadyActive = value;
                }
            }
        }

        [Header("UI")]
        #region UI Debug
        [SerializeField] private TextMeshProUGUI baseTime;
        [SerializeField] private TextMeshProUGUI workingHoursReadyStatus;
        [SerializeField] private TextMeshProUGUI debtCrisisChaseReadyStatus;
        [SerializeField] private Button SetToWorkingFinish;
        #endregion

        private float currentTime;
        private float endHourFloat;
        private float timeMultiplier;
        private bool isTimeRunning;

        #region Public Function
        public void SetTimeActive(bool status) { isTimeRunning = status; }

        public void SetCurrentTime(GameTime targetTime)
        {
            currentTime = targetTime.ToFloat();
            UpdateUI();
        }
        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetToWorkingFinish.onClick.AddListener(() => SetCurrentTime(new GameTime { hour = 13, minute = 0 }));

            TimeStart();
        }

        private void Update()
        {
            if (!isTimeRunning) return;

            currentTime += timeMultiplier * Time.deltaTime;

            if (currentTime >= endHourFloat)
            {
                currentTime = endHourFloat;
                isTimeRunning = false;
            }

            CheckWorkingHoursReadyStatus();
            CheckDebtCrisisChaseReadyStatus();
            UpdateUI();
        }

        private void UpdateUI()
        {
            int hours = Mathf.FloorToInt(currentTime);
            int minutes = Mathf.FloorToInt(currentTime % 1 * 60f);

            baseTime.text = string.Format("{0:00}:{1:00}", hours, minutes);

            workingHoursReadyStatus.text = $"Status Jam Kerja Ready: {workingHoursReadyActive}";
            debtCrisisChaseReadyStatus.text = $"Status Jam Dikejar Ready: {debtCrisisChaseReadyActive}";
        }

        // Cek Waktu Valid Jam Kerja
        private void CheckWorkingHoursReadyStatus()
        {
            float startWorking = startWork.ToFloat();
            float endWorking = endWork.ToFloat();

            workingHoursReadyActive = currentTime >= startWorking && currentTime <= endWorking;
        }

        // Cek Waktu Jam Valid Dikejar Oleh Debt Collector
        private void CheckDebtCrisisChaseReadyStatus()
        {
            float startChasing = startChase.ToFloat();
            float endChasing = endChase.ToFloat();

            debtCrisisChaseReadyActive = currentTime >= startChasing && currentTime <= endChasing;
        }

        private void TimeStart()
        {
            currentTime = startTime.ToFloat();
            endHourFloat = endTime.ToFloat();

            float totalTimeInGame = endHourFloat - currentTime;
            float totalSecondsRealWorld = realMinutesPerDay * 60f;

            timeMultiplier = totalTimeInGame / totalSecondsRealWorld;
            isTimeRunning = true;
        }
    }
}

