using UnityEngine;
using Game.System;
using UnityEngine.UI;

namespace Main.World
{
    public class ItemBtnShop : MonoBehaviour
    {
        public ItemShop item;
        public Image icon;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => ShopManager.Instance.OnItemShopClicked(item));
        }
    }
}
