using UnityEngine;
using Game.System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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
        [SerializeField] private List<ItemShop> items;
        [SerializeField] private ItemShop item;

        [Header("UI")]
        [SerializeField] private RectTransform contentShop;

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
            animatedShop.PreviewItem();
        }

        public void ResetItemSelected() 
        { 
            item = null;
            animatedShop.ResetPreviewItem();
        }
        #endregion

        private void InitializeItemShopUI()
        {
            for (int i = 0; i < items.Count; i++)
            {
                GameObject newBtnShop = Instantiate(btnPrefab);
                newBtnShop.transform.SetParent(contentShop, false);

                ItemBtnShop itemBtnShop = newBtnShop.GetComponent<ItemBtnShop>();
                itemBtnShop.item = items[i];
            }
        }
    }
}
