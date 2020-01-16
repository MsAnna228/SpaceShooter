using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    private int _quest1CurrentIndex;
    private int _quest2CurrentIndex;
    private int _quest3CurrentIndex;

    private bool _questOneComplete;
    private bool _questTwoComplete;
    private bool _questThreeComplete;

    private int _questOneInfo;

    public Text[] _quests;


    //to be saved in player prefs
    private int goalAmount = 20;
    private int rabbitAmount = 5;
    private int survivalAmount = 30;
    private int treeAmount = 10;
    private int fruitAmount = 10;
    private int fruitType = 3; //picked randomly

    private void Start()

    {
        _questOneInfo = PlayerPrefs.GetInt("Quest One");

        goalAmount = PlayerPrefs.GetInt("Goal Amount");
        rabbitAmount = PlayerPrefs.GetInt("Rabbit Amount");
        survivalAmount = PlayerPrefs.GetInt("Survival Amount");
        treeAmount = PlayerPrefs.GetInt("Tree Amount");
        fruitAmount = PlayerPrefs.GetInt("Fruit Amount");
    }


    private void Update()
    {
        
    }

    void SetNewQuest()
    {
        for(int i = 0; i < _quests.Length; i++)
        {
            int questIndex = Random.Range(1, 14);
            switch (questIndex)
            {
                case 0:
                    _quests[i].text = "Reach a score of " + goalAmount.ToString();
                    break;
                case 1:
                    _quests[i].text = "Beat your current high score";
                    break;
                case 2:
                    _quests[i].text = "Avoid " + rabbitAmount.ToString() + " rabbits";
                    break;
                case 3:
                    _quests[i].text = "Survive for " + survivalAmount.ToString() + " time";
                    break;
                case 4:
                    _quests[i].text = "Avoid " + treeAmount.ToString() + " trees";                
                    break;

                default:
                    break;

            }
        }
    }

    public void CheckIfScore()
    {

    }

    //if that number is not already taken by quest slot 2 or 3, then
    //set the quest text to that


    //reach [goal amount] of score
    //beat your current high score
    //avoid [rabbit amount] of rabbits
    //survive for [survival amount] of time
    //avoid [tree amount] of trees

    //try co-op mode (bool)
    //collect [fruit amount] of [fruit type]
    //make it into a bonus level and get [bonus amount] of fruit
    //use [spear amount] of spears
    //survive [speed up amounts] of speed ups
    //collect no fruit for [fruit free time]
    //wear the [costume type] of costume
    //defeat the [boss name]


    //You get 3 quests at a time and you have to complete one in order for another one to be unlocked. 
    //You cannot skip quests
    //Need to store quests in between plays. 

}
