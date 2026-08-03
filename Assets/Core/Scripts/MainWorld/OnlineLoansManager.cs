using System.Collections.Generic;
using Game.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        [SerializeField] private List<GameObject> legalDisplay;
        [SerializeField] private List<GameObject> illegalDisplay;

        [Header("UI")]
        [SerializeField] private RectTransform legalPanel;
        [SerializeField] private RectTransform IllegalPanel;
        [SerializeField] private List<Sprite> navigationBtnColor; // 0 = legal, 1 = illegal, 3 = clicked
        [SerializeField] private Button activeLegalBtn;
        [SerializeField] private Button activeIllegalBtn;
        [SerializeField] private Button searchLegalBtn;
        [SerializeField] private Button searchIllegalBtn;
        [SerializeField] private RectTransform loanLegalContentActive;
        [SerializeField] private RectTransform loanIllegalContentActive;
        [SerializeField] private RectTransform loansLegalContentSearch;
        [SerializeField] private RectTransform loansIllegalContentSearch;
        [SerializeField] private RectTransform loansLegalContentDisplay;
        [SerializeField] private RectTransform loansIllegalContentDisplay;
        [SerializeField] private List<Sprite> loansColor;
        [SerializeField] private List<RectTransform> loansIllegalContent; // 0 = search, 1 = active
        [SerializeField] private List<RectTransform> loansLegalContent; // 0 = search, 1 = active

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

            loansLegalContent.ForEach(item => { if(item != null) item.gameObject.SetActive(false); } );
            loansIllegalContent.ForEach(item => { if(item != null) item.gameObject.SetActive(false); } );

            loansLegalContent[0].gameObject.SetActive(true);
            loansIllegalContent[0].gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            TimeManager.OnDateChange += () => GenerateLoanFunds(true);
            TimeManager.OnDateChange += () => GenerateLoanFunds(false);

            AnimatedSmartphone.OnAPKOpened += PanelOpened;

            activeLegalBtn.onClick.AddListener(() => OpenContent(loansLegalContent, 1));
            searchLegalBtn.onClick.AddListener(() => OpenContent(loansLegalContent, 0));

            activeIllegalBtn.onClick.AddListener(() => OpenContent(loansIllegalContent, 1));
            searchIllegalBtn.onClick.AddListener(() => OpenContent(loansIllegalContent, 0));
        }

        private void OnDisable()
        {
            TimeManager.OnDateChange -= () => GenerateLoanFunds(true);
            TimeManager.OnDateChange -= () => GenerateLoanFunds(false);

            AnimatedSmartphone.OnAPKOpened -= PanelOpened;

            activeLegalBtn.onClick.RemoveListener(() => OpenContent(loansLegalContent, 1));
            searchLegalBtn.onClick.RemoveListener(() => OpenContent(loansLegalContent, 0));

            activeIllegalBtn.onClick.RemoveListener(() => OpenContent(loansIllegalContent, 1));
            searchIllegalBtn.onClick.RemoveListener(() => OpenContent(loansIllegalContent, 0));
        }

        private void PanelOpened(RectTransform panel)
        {
            if (panel == legalPanel)
            {
                loansLegalContent[0].gameObject.SetActive(true); 
                loansLegalContent[1].gameObject.SetActive(false); 
            }
            else if (panel == IllegalPanel)
            {
                loansIllegalContent[0].gameObject.SetActive(true);
                loansIllegalContent[1].gameObject.SetActive(false);
            }
        }

        private void OpenContent(List<RectTransform> content, int index)
        {
            int anotherIndex = index == 1 ? 0 : 1;
            content[index].gameObject.SetActive(true);
            content[anotherIndex].gameObject.SetActive(false);
        }

        private void GenerateLoanFunds(bool isLegal)
        {
            if (isLegal)
            {
                if (modelOnlineLoansLegal.Count > 0)
                {
                    modelOnlineLoansLegal.Clear();
                }
            }
            else
            {
                if (modelOnlineLoansIllegal.Count > 0)
                {
                    modelOnlineLoansIllegal.Clear();
                }
            }

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
                if (legalDisplay.Count > 0)
                {
                    for (int i = 0; i < legalDisplay.Count; i++)
                    {
                        Destroy(legalDisplay[i]);
                    }

                    legalDisplay.Clear();
                }

                for (int i = 0; i < modelOnlineLoansLegal.Count; i++)
                {
                    GameObject newLoansItems = Instantiate(loansItem, loansLegalContentDisplay);

                    ItemPinjol itemPinjol = newLoansItems.GetComponent<ItemPinjol>();

                    itemPinjol.modelOnlineLoans = modelOnlineLoansLegal[i];
                    itemPinjol.UpdateUI(loansColor[0]);

                    legalDisplay.Add(newLoansItems);
                }
            }
            else
            {
                if (illegalDisplay.Count > 0)
                {
                    for (int i = 0; i < illegalDisplay.Count; i++)
                    {
                        Destroy(illegalDisplay[i]);
                    }

                    illegalDisplay.Clear();
                }

                for (int i = 0; i < modelOnlineLoansIllegal.Count; i++)
                {
                    GameObject newLoansItems = Instantiate(loansItem, loansIllegalContentDisplay);

                    ItemPinjol itemPinjol = newLoansItems.GetComponent<ItemPinjol>();

                    itemPinjol.modelOnlineLoans = modelOnlineLoansIllegal[i];
                    itemPinjol.UpdateUI(loansColor[1]);

                    illegalDisplay.Add(newLoansItems);
                }
            }
        }
    }
}
