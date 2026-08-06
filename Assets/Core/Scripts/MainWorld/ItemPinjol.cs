using Game.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.World
{
    public class ItemPinjol : MonoBehaviour
    {
        [Header("UI")]
        public Button takeBtn;
        public TextMeshProUGUI loanMoney;
        public TextMeshProUGUI loanDuration;
        public TextMeshProUGUI loanInterestRate;
        public TextMeshProUGUI loanRepayment;
        public Image background;

        public ModelOnlineLoans modelOnlineLoans;

        private void Start()
        {
            takeBtn.onClick.AddListener(() => OnlineLoansManager.Instance.OnTakeBtnClicked(modelOnlineLoans, this));
        }

        public void UpdateUI(Sprite img)
        {
            if (modelOnlineLoans == null) return;

            loanMoney.text = $"+ RP. {GameManager.Instance.CurrencyFormat(modelOnlineLoans.receivedAmount)}";
            loanDuration.text = $"Loan Terms: {modelOnlineLoans.loanTenureDays} Days";
            loanInterestRate.text = $"Daily Interest Rate: {modelOnlineLoans.dailyInterestRate}%";
            loanRepayment.text = $"Total Repayment On The Due Date: {GameManager.Instance.CurrencyFormat(modelOnlineLoans.totalRepaymentAmount)}";
            background.sprite = img;
        }
    }
}
