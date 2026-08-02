using Game.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.World
{
    public class ItemInventory : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image img;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI moneyPlus;
        [SerializeField] private TextMeshProUGUI itemDescription;

        public Sprite itemImg;
        public ItemPlayer ItemPlayer;

        private void Start()
        {
            itemName.text = ItemPlayer.name;
            moneyPlus.text = $"+RP {GameManager.Instance.CurrencyFormat(ItemPlayer.moneyPlus)}";
            img.sprite = itemImg;
            itemDescription.text = "bonus income per correct answer";
        }
    }
}
