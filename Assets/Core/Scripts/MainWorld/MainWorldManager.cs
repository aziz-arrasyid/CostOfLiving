using TMPro;
using UnityEngine;
using Game.System;

namespace Main.World
{
    public class MainWorldManager : MonoBehaviour
    {
        public static MainWorldManager Instance {private set; get; }

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI money;

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
            UpdateMoneyUI();
        }

        public void UpdateMoneyUI()
        {
            PlayerData playerData = GameManager.Instance.LoadData<PlayerData>("playerData");
            money.text = $"RP. {GameManager.Instance.CurrencyFormat(playerData.money)}";
        }
    }
}
