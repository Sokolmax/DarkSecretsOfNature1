﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControllerScript : MonoBehaviour
{
    public Card thisCard;

    public bool isPlayerCard;

    public CardInfoScript info;
    public CardMovementScript movement;

    GameManagerScript gameManager;

    public void Init(Card card, bool isPlayerCard)
    {
        thisCard = card;
        gameManager = GameManagerScript.instance;
        this.isPlayerCard = isPlayerCard;

        if(isPlayerCard)
        {
            info.ShowCardInfo();
            GetComponent<AttackedCardScript>().enabled = false;
        }
        else
        {
            info.HideCardInfo();
        }
    }

    public void OnCast()//каст
    {
        if(isPlayerCard)
        {
            gameManager.playerHandCards.Remove(this);
            gameManager.playerFieldCards.Add(this);
            gameManager.ReduceEnergy(true, thisCard.cost);
            gameManager.CheckCardsForManaAvailability();
        }
        else
        {
            gameManager.enemyHandCards.Remove(this);
            gameManager.enemyFieldCards.Add(this);
            gameManager.ReduceEnergy(false, thisCard.cost);
        }

        thisCard.isPlaced = true;

    }

    public void OnTakeDamage(CardControllerScript attacker = null)//получение урона
    {
        CheckForAlive();
    }

    public void OnDamageDeal()//нанесение урона
    {
        thisCard.canAttack = false;
        info.HighlightCard(false);
    }

    public void CheckForAlive()
    {
        if(thisCard.isAlive)
            info.RefreshData();
        else
            DestroyCard();
    }

    public void DestroyCard()
    {
        movement.OnEndDrag(null);

        RemoveCardFromList(gameManager.enemyFieldCards);
        RemoveCardFromList(gameManager.enemyHandCards);
        RemoveCardFromList(gameManager.playerFieldCards);
        RemoveCardFromList(gameManager.playerHandCards);

        Destroy(gameObject);
    }

    void RemoveCardFromList(List<CardControllerScript> list)//удаление карты из списка
    {
        if(list.Exists(x => x == this))
            list.Remove(this);
    }
}
