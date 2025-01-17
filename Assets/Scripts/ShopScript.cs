﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    private string ownedItems = "";
    public static string ownedItemsKey = "OwnedItems";

    int equippedBallIndex = 0;
    string equippedBall;
    public static string equippedBallKey = "EquippedBall";
    public List<GameObject> ballPrefabList;
    public List<GameObject> ballsList;
    //List<string> ownedItemsList = new List<string>();
    string[] ownedItemsList;

    public static int playerCurrency;
    public static string playerCurrencyKey = "PlayerCurrency";

    public TextMeshProUGUI moneyText;

    public int startingBallCost = 25;
    public int numOfBallsBeforeCostIncrease = 3;
    public float costIncrease = 25;

    public Dictionary<string, int> ballPrices = new Dictionary<string, int>();
    private void Start()
    {
        equippedBall = PlayerPrefs.GetString(equippedBallKey, "Classic");
        playerCurrency = PlayerPrefs.GetInt(playerCurrencyKey, 25);

        //PlayerPrefs.DeleteKey(ownedItemsKey);

        moneyText.text = playerCurrency.ToString();
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

            Button buyButton = ball.transform.Find("Canvas/BuyButton").GetComponent<Button>();
            Button equipButton = ball.transform.Find("Canvas/EquipButton").GetComponent<Button>();

            buyButton.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            buyButton.onClick.AddListener(() => Buy(ball.name));

            equipButton.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            equipButton.onClick.AddListener(() => Equip(ball.name));

            for (int x = 0; x < ownedItemsList.Count(); x++)
            {
                // if this ball is owned
                if (ball.name == ownedItemsList[x])
                {
                    // remove the buy button so that they don't buy it again
                    //ball.transform.Find("BuyButton").gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(false);

                    // if this ball is equipped
                    if (equippedBall == ball.name)
                    {
                        // disable the equip button
                        equipButton.gameObject.GetComponent<Button>().interactable = false;
                        equipButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped";
                    }
                }
                else // if this ball is not owned
                {
                    //int cost = Mathf.RoundToInt(startingBallCost + i / numOfBallsBeforeCostIncrease * costIncrease);
                    int cost = ball.GetComponent<BallCost>().cost;
                    buyButton.GetComponentInChildren<TextMeshProUGUI>().text = cost.ToString();
                }
            }
        }
        // set the equipped button
        for (int i = 0; i < ballsList.Count(); i++) // loops through all balls to find which one is equipped
        {
            GameObject ball = ballsList[i];
            if (ball.name == equippedBall)
            {
                equippedBallIndex = i;
                ball.transform.Find("Canvas/EquipButton").gameObject.GetComponent<Button>().interactable = false;
                ball.transform.Find("Canvas/EquipButton/EquipText").gameObject.GetComponent<TextMeshProUGUI>().text = "Equipped";
                break;
            }
        }
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
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
                    cost = ball.GetComponent<BallCost>().cost;
                    //cost = Mathf.RoundToInt(startingBallCost + i / numOfBallsBeforeCostIncrease * costIncrease);

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
            moneyText.text = playerCurrency.ToString();

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
                PlayerPrefs.SetString(equippedBallKey, ball.name);
                ball.transform.Find("Canvas/EquipButton").gameObject.GetComponent<Button>().interactable = false;
                ball.transform.Find("Canvas/EquipButton/EquipText").gameObject.GetComponent<TextMeshProUGUI>().text = "Equipped";
                break;
            }
        }
    }
}
