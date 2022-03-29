using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinInShop : MonoBehaviour
{
    [SerializeField] private Knive knivePrefab;

    [SerializeField] private SkinInfo skininfo;
    private Image skinImage;

    [SerializeField] private Text priceLabel;
    [SerializeField] private Image logo;

    [SerializeField] private float chanceToWinCurrentSkin;

    private bool isPurchased;
    private int bossDefeatedCount = 0;

    private void Awake()
    {
        //PlayerPrefs.SetInt(skininfo.SkinId.ToString(), 0);    //обнулить покупки
        //IsSkinPurchased();

        PlayerPrefs.SetInt("Defeated Bosses", bossDefeatedCount);
        PlayerPrefs.SetInt("ApplesCollected",100);  //дебаг фича
        IsSkinPurchased();
    }

    private void Update()
    {
        if(PlayerPrefs.GetInt(skininfo.SkinId.ToString()) == 1 && knivePrefab.GetComponentInChildren<SpriteRenderer>().sprite != skininfo.SkinSprite)
        {
            priceLabel.text = "sold";
        }

        if (GameManager.Instance.DefetedBosses > bossDefeatedCount)
        {
            bossDefeatedCount = GameManager.Instance.DefetedBosses;
            PlayerPrefs.SetInt("Defeated Bosses", GameManager.Instance.DefetedBosses);
            OpenFreeSkin();
        }

        Debug.Log(PlayerPrefs.GetInt("Defeated Bosses"));
    }

    public void OnPurchaseButtonClicked()
    {
        if (!isPurchased)
        {
            if (PlayerPrefs.GetInt("ApplesCollected") >= skininfo.SkinPrice)
            {
                int currentApples = PlayerPrefs.GetInt("ApplesCollected");
                PlayerPrefs.SetInt("ApplesCollected", currentApples - skininfo.SkinPrice);
                PlayerPrefs.SetInt(skininfo.SkinId.ToString(), 1);

                IsSkinPurchased();
            }
        }
        else
        {
            knivePrefab.EquipSkin(skininfo);
            priceLabel.text = "equiped";
        }
    }

    public void OpenFreeSkin()
    {
        if(PlayerPrefs.GetInt(skininfo.SkinId.ToString()) != 1)
        {
            var chanceToWin = Random.Range(0, 100);
            if (chanceToWin < chanceToWinCurrentSkin)
            {
                PlayerPrefs.SetInt(skininfo.SkinId.ToString(), 1);
                IsSkinPurchased();
            }
        }
        Debug.Log(skininfo.name);
    }

    private void IsSkinPurchased()
    {
        if(PlayerPrefs.GetInt(skininfo.SkinId.ToString()) == 1)
        {
            isPurchased = true;
            priceLabel.text = "sold";
            priceLabel.fontSize = 20;
            logo.enabled = false;
        }
        else if (PlayerPrefs.GetInt(skininfo.SkinId.ToString()) == 0)
        {
            isPurchased = false;
            priceLabel.text = skininfo.SkinPrice.ToString();
            logo.enabled = true;
        }
    }
}