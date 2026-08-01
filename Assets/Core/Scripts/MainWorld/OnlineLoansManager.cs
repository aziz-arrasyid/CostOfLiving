using System.Collections.Generic;
using Game.System;
using TMPro;
using UnityEngine;

namespace Main.World
{
    public class OnlineLoansManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject loansItem;
        [Header("Online Loans Data")]
        [SerializeField] private List<ModelOnlineLoans> modelOnlineLoansLegal;
        [SerializeField] private List<ModelOnlineLoans> modelOnlineLoansIllegal;
        [SerializeField] private int availableLoanFunds;

        [Header("UI")]
        [SerializeField] private RectTransform loansLegalContent;
        [SerializeField] private RectTransform loansIllegalContent;
        [SerializeField] private List<Sprite> loansColor;

        #region Online Loans Legal
        [Header("Online Loans Legal Setting")]
        [SerializeField] private int minReceivedAmountLegal;
        [SerializeField] private int maxReceivedAmountLegal;

        [Space]

        [SerializeField] private int minLoanTenureDaysLegal;
        [SerializeField] private int maxLoanTenureDaysLegal;

        [Space]

        [SerializeField] private float minDailyInterestRateLegal;
        [SerializeField] private float maxDailyInterestRateLegal;
        #endregion

        [Space]

        #region Online Loans Illegal
        [Header("Online Loans Illegal Setting")]
        [SerializeField] private int minReceivedAmountIllegal;
        [SerializeField] private int maxReceivedAmountIllegal;

        [Space]

        [SerializeField] private int minLoanTenureDaysIllegal;
        [SerializeField] private int maxLoanTenureDaysIllegal;

        [Space]

        [SerializeField] private float minDailyInterestRateIllegal;
        [SerializeField] private float maxDailyInterestRateIllegal;
        #endregion

        private void Start()
        {
            GenerateLoanFunds(isLegal: true);
            GenerateLoanFunds(isLegal: false);
        }

        private void GenerateLoanFunds(bool isLegal)
        {
            for (int i = 0; i < availableLoanFunds; i++)
            {
                int minReceivedAmount = isLegal ? minReceivedAmountLegal : minReceivedAmountIllegal;
                int maxReceivedAmount = isLegal ? maxReceivedAmountLegal : maxReceivedAmountIllegal;

                int minLoanTenureDays = isLegal ? minLoanTenureDaysLegal : minLoanTenureDaysIllegal;
                int maxLoanTenureDays = isLegal ? maxLoanTenureDaysLegal : maxLoanTenureDaysIllegal;

                float minDailyInterestRate = isLegal ? minDailyInterestRateLegal : minDailyInterestRateIllegal;
                float maxDailyInterestRate = isLegal ? maxDailyInterestRateLegal : maxDailyInterestRateIllegal;

                int receivedAmount = Random.Range(minReceivedAmount, maxReceivedAmount) * 10000;
                int loanTenureDays = Random.Range(minLoanTenureDays, maxLoanTenureDays);
                float dailyInterestRate = Mathf.Round(Random.Range(minDailyInterestRate, maxDailyInterestRate) * 10f) / 10f;

                int totalRepaymentAmount = receivedAmount + (int)(receivedAmount * (dailyInterestRate / 100.0) * loanTenureDays);

                ModelOnlineLoans newOnlineLoans = new()
                {
                    receivedAmount = receivedAmount,
                    loanTenureDays = loanTenureDays,
                    dailyInterestRate = dailyInterestRate,
                    totalRepaymentAmount = totalRepaymentAmount
                };

                if (isLegal)
                {
                    modelOnlineLoansLegal.Add(newOnlineLoans);
                }
                else
                {
                    modelOnlineLoansIllegal.Add(newOnlineLoans);
                }
            }

            UpdateOnlineLoansUI(true);
            UpdateOnlineLoansUI(false);
        }

        private void UpdateOnlineLoansUI(bool isLegal)
        {
            if (isLegal)
            {
                for (int i = 0; i < modelOnlineLoansLegal.Count; i++)
                {
                    GameObject newLoansItems = Instantiate(loansItem);
                    newLoansItems.transform.SetParent(loansLegalContent);

                    ItemPinjol itemPinjol = newLoansItems.GetComponent<ItemPinjol>();

                    itemPinjol.modelOnlineLoans = modelOnlineLoansLegal[i];
                    itemPinjol.UpdateUI(loansColor[0]);
                }
            }
            else
            {
                for (int i = 0; i < modelOnlineLoansIllegal.Count; i++)
                {
                    GameObject newLoansItems = Instantiate(loansItem);
                    newLoansItems.transform.SetParent(loansIllegalContent);

                    ItemPinjol itemPinjol = newLoansItems.GetComponent<ItemPinjol>();

                    itemPinjol.modelOnlineLoans = modelOnlineLoansIllegal[i];
                    itemPinjol.UpdateUI(loansColor[1]);
                }
            }
        }
    }
}
