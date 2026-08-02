using System.Collections.Generic;
using Game.System;
using UnityEngine;
using System.Linq;

namespace Main.World
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        [SerializeField] private GameObject itemPrefab;

        [Header("UI")]
        [SerializeField] private RectTransform contentInventory;
        [SerializeField] private List<Sprite> itemSprite;

        [Space]
        [SerializeField] List<GameObject> itemInventoryDisplay;
        [SerializeField] private List<ItemPlayer> itemPlayer;

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
            GenerateItemInventoryUI();
        }

        public void GenerateItemInventoryUI()
        {
            PlayerData playerData = GameManager.Instance.LoadData<PlayerData>("playerData");
            if (playerData.items.Count <= 0) return;

            itemPlayer = playerData.items;

            if (itemInventoryDisplay.Count > 0)
            {
                for (int i = 0; i < itemInventoryDisplay.Count; i++)
                {
                    GameObject itemObj = itemInventoryDisplay[i];
                    if (itemObj != null)
                    {
                        Destroy(itemObj);
                    }

                }

                itemInventoryDisplay.Clear();
            }

            foreach (ItemPlayer item in itemPlayer)
            {
                GameObject newItem = Instantiate(itemPrefab, contentInventory);

                ItemInventory newItemPlayer = newItem.GetComponent<ItemInventory>();
                newItemPlayer.itemImg = itemSprite.Find(img => img.name == item.id);
                newItemPlayer.ItemPlayer = item;

                itemInventoryDisplay.Add(newItem);
            }
        }

        public string GenerateUniqueID()
        {
            PlayerData playerData = GameManager.Instance.LoadData<PlayerData>("playerData");
            itemPlayer = playerData.items;

            string UniqueID;
            int attemptCount = 0;
            int maxAttempt = 10000;

            do
            {
                int randomNumber = Random.Range(0, 10000);
                UniqueID = randomNumber.ToString("D4");
                attemptCount++;

                if (attemptCount >= maxAttempt)
                {
                    return "FULL";
                }
            }
            while (itemPlayer.Any(item => item.uniqueID == UniqueID));
            return UniqueID;
        }
    }
}
