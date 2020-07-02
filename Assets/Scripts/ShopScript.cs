using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    private string ownedItems = "";
    private const string ownedItemsKey = "OwnedItems";

    int equippedBallIndex = 0;
    private const string equippedBallKey = "EquippedBall";
    public List<GameObject> ballPrefabList;
    public List<GameObject> ballsList;
    //List<string> ownedItemsList = new List<string>();
    string[] ownedItemsList;

    public static int playerCurrency;
    public const string playerCurrencyKey = "PlayerCurrency";

    public TextMeshProUGUI moneyText;

    public int startingBallCost = 25;
    public int numOfBallsBeforeCostIncrease = 3;
    public float costIncrease = 25; 
    private void Start()
    {
        equippedBallIndex = PlayerPrefs.GetInt(equippedBallKey, 0);
        playerCurrency = PlayerPrefs.GetInt(playerCurrencyKey, 0);
        PlayerPrefs.DeleteAll();
        playerCurrency = 10000;

        moneyText.text = "$" + playerCurrency;
        ownedItems = PlayerPrefs.GetString(ownedItemsKey, "Classic");
        // take away spaces
        ownedItems.Replace(" ", "");
        ownedItemsList = ownedItems.Split(char.Parse(","));
        
        // adds the current ball instances to the ball list
        foreach(GameObject ball in ballPrefabList)
        {
            ballsList.Add(GameObject.Find(ball.name));
        }

        for(int i = 0; i < ballsList.Count(); i++)
        {
            GameObject ball = ballsList[i].gameObject;
            for (int x = 0; x < ownedItemsList.Count(); x++)
            {
                // if this ball is owned
                if (ball.name == ownedItemsList[x])
                {
                    // remove the buy button so that they don't buy it again
                    //ball.transform.Find("BuyButton").gameObject.SetActive(false);
                    ball.transform.Find("Canvas/BuyButton").gameObject.SetActive(false);
                    // if this ball is equipped
                    if (ballsList[equippedBallIndex].name == ball.name)
                    {
                        // disable the equip button
                        ball.transform.Find("Canvas/EquipButton").gameObject.GetComponent<Button>().interactable = false;
                        ball.transform.Find("Canvas/EquipButton/EquipText").gameObject.GetComponent<TextMeshProUGUI>().text = "Equipped";
                    }
                }
                else // if this ball is not owned
                {
                    int cost = Mathf.RoundToInt(startingBallCost + i / numOfBallsBeforeCostIncrease * costIncrease);

                    ball.transform.Find("Canvas/BuyButton/BuyButtonText").GetComponent<TextMeshProUGUI>().text = "$" + cost;
                }
            }
        }
        // set the equipped button
        for (int i = 0; i < ballsList.Count(); i++)
        {
            GameObject ball = ballsList[i];
            if (ball.name == ballsList[equippedBallIndex].name)
            {
                equippedBallIndex = i;
                ball.transform.Find("Canvas/EquipButton").gameObject.GetComponent<Button>().interactable = false;
                ball.transform.Find("Canvas/EquipButton/EquipText").gameObject.GetComponent<TextMeshProUGUI>().text = "Equipped";
                break;
            }
        }
    }

    public void Buy(string itemName)
    {
        if (!ownedItems.Contains(itemName))
        {
            int cost = 0;
            for (int i = 0; i < ballsList.Count(); i++)
            {
                GameObject ball = ballsList[i];
                if (ball.name == itemName)
                {
                    cost = Mathf.RoundToInt(startingBallCost + i / numOfBallsBeforeCostIncrease * costIncrease);

                    if (playerCurrency >= cost)
                    {
                        ball.transform.Find("Canvas/BuyButton").gameObject.GetComponent<Button>().gameObject.SetActive(false);
                        playerCurrency -= cost;
                    } else
                    {
                        return;
                    }
                    break;
                }
            }

            ownedItems += "," + itemName;
            moneyText.text = "$" + playerCurrency;

            PlayerPrefs.SetString(ownedItemsKey, ownedItems);
            PlayerPrefs.SetInt(playerCurrencyKey, playerCurrency);
        }
    }

    public void Equip(string itemName)
    {
        // make the previous equipped ball equippable again. Enable button and change text
        ballsList[equippedBallIndex].transform.Find("Canvas/EquipButton").gameObject.GetComponent<Button>().interactable = true;
        ballsList[equippedBallIndex].transform.Find("Canvas/EquipButton/EquipText").gameObject.GetComponent<TextMeshProUGUI>().text = "Equip";

        for(int i = 0; i < ballsList.Count(); i++)
        {
            GameObject ball = ballsList[i];
            if (ball.name == itemName)
            {
                equippedBallIndex = i;
                PlayerPrefs.SetInt(equippedBallKey, equippedBallIndex);
                ball.transform.Find("Canvas/EquipButton").gameObject.GetComponent<Button>().interactable = false;
                ball.transform.Find("Canvas/EquipButton/EquipText").gameObject.GetComponent<TextMeshProUGUI>().text = "Equipped";
                break;
            }
        }
    }
}
