using Game.System;
using TMPro;
using UnityEngine;

namespace Main.World
{
    public class ItemMyPinjol : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI totalRepaymentText;
        [SerializeField] private TextMeshProUGUI remainingDaysText;
        [SerializeField] private TextMeshProUGUI penaltyText;
        public OnlineLoansPlayer onlineLoansPlayer;

        private void Start()
        {
            UpdateUI();
        }

        private void OnEnable()
        {
            onlineLoansPlayer.OnDataChanged += UpdateUI;
        }

        private void OnDisable()
        {
            onlineLoansPlayer.OnDataChanged -= UpdateUI;
        }

        private void UpdateUI()
        {
            totalRepaymentText.text = $"Total Repayment: RP. {GameManager.Instance.CurrencyFormat(onlineLoansPlayer.TotalRepayment)}";
            remainingDaysText.text = $"Remaining Days: {onlineLoansPlayer.RemainingDays}";
            penaltyText.text = $"+RP. {GameManager.Instance.CurrencyFormat(onlineLoansPlayer.DailyOverduePenalty)}/Day If Past Due";
        }
    }
}
