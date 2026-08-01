using UnityEngine;
using Game.System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Main.World
{
    [RequireComponent(typeof(AnimatedShop))]
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Instance { private set; get; }

        [Header("Prefabs")]
        [SerializeField] private GameObject btnPrefab;

        [Space]

        [Header("Item Shop")]
        [SerializeField] private List<Sprite> image;
        [SerializeField] private List<ItemShop> items;
        [SerializeField] private ItemShop item;

        [Header("UI")]
        [SerializeField] private RectTransform contentShop;
        [SerializeField] private TextMeshProUGUI itemShopName;
        [SerializeField] private TextMeshProUGUI itemShopMoneyPlus;
        [SerializeField] private TextMeshProUGUI itemShopPrice;
        [SerializeField] private Image previewIcon;

        [Space]

        private AnimatedShop animatedShop;

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
            animatedShop = GetComponent<AnimatedShop>();

            ItemShopDatabase itemShopDatabase = GameManager.Instance.LoadData<ItemShopDatabase>("itemShop");
            items = itemShopDatabase.items;

            InitializeItemShopUI();
        }

        #region Public Function
        public void OnItemShopClicked(ItemShop itemShop)
        {
            // GameObject btnObj = EventSystem.current.currentSelectedGameObject;
            item = itemShop;
            UpdatePreviewItemUI();

            animatedShop.PreviewItem();
        }

        public void ResetItemSelected()
        {
            item = null;
            animatedShop.ResetPreviewItem();
        }
        #endregion

        private void UpdatePreviewItemUI()
        {
            itemShopName.text = item.name;
            itemShopMoneyPlus.text = $"Money+ RP. {GameManager.Instance.CurrencyFormat(item.moneyPlus)}";
            itemShopPrice.text = $"RP. {GameManager.Instance.CurrencyFormat(item.buy)}";

            previewIcon.sprite = image.Find(img => img.name == item.id);
        }

        private void InitializeItemShopUI()
        {
            for (int i = 0; i < items.Count; i++)
            {
                GameObject newBtnShop = Instantiate(btnPrefab);
                newBtnShop.transform.SetParent(contentShop, false);

                ItemBtnShop itemBtnShop = newBtnShop.GetComponent<ItemBtnShop>();
                itemBtnShop.item = items[i];

                Image icon = itemBtnShop.icon;

                Sprite targetImage = image.Find(img => img.name == itemBtnShop.item.id);

                if (targetImage != null)
                {
                    icon.sprite = targetImage;
                }
            }
        }
    }
}
