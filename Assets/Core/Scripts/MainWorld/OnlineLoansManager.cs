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
        public static OnlineLoansManager Instance { private set; get; }
        [Header("Prefabs")]
        [SerializeField] private GameObject loansItem;
        [SerializeField] private GameObject myLoansItem;
        [Header("Online Loans Data")]
        [SerializeField] private List<ModelOnlineLoans> modelOnlineLoansLegal;
        [SerializeField] private List<ModelOnlineLoans> modelOnlineLoansIllegal;
        [SerializeField] private int availableLoanFunds;
        [SerializeField] private List<GameObject> legalDisplay;
        [SerializeField] private List<GameObject> illegalDisplay;
        [SerializeField] private List<GameObject> myPinjolLegalDisplay;
        [SerializeField] private List<GameObject> myPinjolIllegalDisplay;

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
        [SerializeField] private RectTransform loansLegalContentActiveDisplay;
        [SerializeField] private RectTransform loansIllegalContentActiveDisplay;
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
            GenerateLoanFunds(isLegal: true);
            GenerateLoanFunds(isLegal: false);

            loansLegalContent.ForEach(item => { if (item != null) item.gameObject.SetActive(false); });
            loansIllegalContent.ForEach(item => { if (item != null) item.gameObject.SetActive(false); });

            loansLegalContent[0].gameObject.SetActive(true);
            loansIllegalContent[0].gameObject.SetActive(true);

            UpdateMyLoansUI();
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

            TimeManager.OnDateChange += UpdateDataMyLoansPerDay;
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

            TimeManager.OnDateChange -= UpdateDataMyLoansPerDay;
        }

        public void OnTakeBtnClicked(ModelOnlineLoans onlineLoans, ItemPinjol loans)
        {
            int totalPenalty = 0;

            switch (onlineLoans.status)
            {
                case OnlineLoansStatus.legal:
                    totalPenalty = Mathf.RoundToInt(onlineLoans.receivedAmount * (onlineLoans.dailyInterestRate / 100f)) + 15000;
                    break;
                case OnlineLoansStatus.illegal:
                    totalPenalty = Mathf.RoundToInt(onlineLoans.receivedAmount * (onlineLoans.dailyInterestRate / 100f)) * 2;
                    break;
            }

            OnlineLoansPlayer newOnlineLoans = new()
            {
                Status = onlineLoans.status,
                TotalRepayment = onlineLoans.totalRepaymentAmount,
                RemainingDays = onlineLoans.loanTenureDays,
                DailyOverduePenalty = totalPenalty
            };

            PlayerData playerData = GameManager.Instance.LoadData<PlayerData>("playerData");
            playerData.money += onlineLoans.receivedAmount;
            playerData.onlineLoans.Add(newOnlineLoans);

            UpdateAddMyPinjolUI(newOnlineLoans, loans);

            GameManager.Instance.SaveData(playerData, "playerData");
        }

        private void UpdateDataMyLoansPerDay()
        {
            PlayerData playerData = GameManager.Instance.LoadData<PlayerData>("playerData");

            for (int i = 0; i < playerData.onlineLoans.Count; i++)
            {
                playerData.onlineLoans[i].RemainingDays -= 1;
            }

            GameManager.Instance.SaveData(playerData, "playerData");

            UpdateMyLoansUI();
        }

        private void UpdateAddMyPinjolUI(OnlineLoansPlayer onlineLoansPlayer, ItemPinjol loans)
        {
            ModelOnlineLoans onlineLoans = loans.modelOnlineLoans;

            switch (onlineLoansPlayer.Status)
            {
                case OnlineLoansStatus.legal:
                    GameObject myLegalObj = Instantiate(myLoansItem, loansLegalContentActiveDisplay);
                    myLegalObj.GetComponent<ItemMyPinjol>().onlineLoansPlayer = onlineLoansPlayer;
                    myPinjolLegalDisplay.Add(myLegalObj);

                    modelOnlineLoansLegal.Remove(onlineLoans);

                    if (legalDisplay.Contains(loans.gameObject))
                    {
                        Destroy(loans.gameObject);
                        legalDisplay.Remove(loans.gameObject);
                    }
                    break;
                case OnlineLoansStatus.illegal:
                    GameObject myIllegalObj = Instantiate(myLoansItem, loansIllegalContentActiveDisplay);
                    myIllegalObj.GetComponent<ItemMyPinjol>().onlineLoansPlayer = onlineLoansPlayer;
                    myPinjolIllegalDisplay.Add(myIllegalObj);

                    modelOnlineLoansIllegal.Remove(onlineLoans);

                    if (illegalDisplay.Contains(loans.gameObject))
                    {
                        Destroy(loans.gameObject);
                        illegalDisplay.Remove(loans.gameObject);
                    }
                    break;
            }
        }

        private void UpdateMyLoansUI()
        {
            if (myPinjolLegalDisplay.Count > 0)
            {
                for (int i = 0; i < myPinjolLegalDisplay.Count; i++)
                {
                    Destroy(myPinjolLegalDisplay[i]);
                }

                myPinjolLegalDisplay.Clear();
            }

            if (myPinjolIllegalDisplay.Count > 0)
            {
                for (int i = 0; i < myPinjolIllegalDisplay.Count; i++)
                {
                    Destroy(myPinjolIllegalDisplay[i]);
                }

                myPinjolIllegalDisplay.Clear();
            }

            PlayerData playerData = GameManager.Instance.LoadData<PlayerData>("playerData");
            List<OnlineLoansPlayer> onlineLoansPlayer = playerData.onlineLoans;

            for (int i = 0; i < onlineLoansPlayer.Count; i++)
            {
                if (onlineLoansPlayer[i].Status == OnlineLoansStatus.legal)
                {
                    GameObject myLegalObj = Instantiate(myLoansItem, loansLegalContentActiveDisplay);
                    myLegalObj.GetComponent<ItemMyPinjol>().onlineLoansPlayer = onlineLoansPlayer[i];
                    myPinjolLegalDisplay.Add(myLegalObj);
                }
                else if (onlineLoansPlayer[i].Status == OnlineLoansStatus.illegal)
                {
                    GameObject myIllegalObj = Instantiate(myLoansItem, loansIllegalContentActiveDisplay);
                    myIllegalObj.GetComponent<ItemMyPinjol>().onlineLoansPlayer = onlineLoansPlayer[i];
                    myPinjolIllegalDisplay.Add(myIllegalObj);
                }
            }
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

                int totalRepaymentAmount = receivedAmount + Mathf.RoundToInt(receivedAmount * (dailyInterestRate / 100f) * loanTenureDays);

                ModelOnlineLoans newOnlineLoans = new()
                {
                    status = isLegal ? OnlineLoansStatus.legal : OnlineLoansStatus.illegal,
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
