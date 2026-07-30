using UnityEngine;
using TMPro;
using System;

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
        [Header("Time Setting")]
        [Range(1, 60)]
        [SerializeField] private float realMinutesPerDay;
        [SerializeField] private GameTime startTime = new() { hour = 7, minute = 30};
        [SerializeField] private GameTime endTime = new() {hour = 17, minute = 0};

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI display;

        private float currentTime;
        private float endHourFloat;
        private float timeMultiplier;
        private bool isTimeRunning;

        #region Fungsi Public
        public void SetTimeActive(bool status) { isTimeRunning = status; }
        #endregion

        private void Start()
        {
            TimeStart();
        }

        private void Update()
        {
            if (!isTimeRunning) return;

            currentTime += timeMultiplier * Time.deltaTime;

            if(currentTime >= endHourFloat)
            {
                currentTime = endHourFloat;
                isTimeRunning = false;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            int hours = Mathf.FloorToInt(currentTime);
            int minutes = Mathf.FloorToInt(currentTime % 1 * 60f);

            display.text = string.Format("{0:00}:{1:00}", hours, minutes);
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

