using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public enum Difficulty { Easy, Medium, Hard }

public enum AssociationTypes { Word, Pantomime, Sound, Object }

public class GameHandlerScript : MonoBehaviour
{
    public GameObject MainStage, ResultScreen, GameOptions, PauseScreen, GameEndScreen;


    public bool startedGame;

    public int rounds;
    public int playerAmount;

    public Image cardSpace;
    public Image PauseBuffer;
    public Image ClueImage;

    public Card[] Cards;

    public Card CurrentCard;
    public List<Card> DrawPile;
    public List<Card> DiscardPile;

    public int AssociationAmount;
    public AssociationBoxController[] AssociationCheckBoxes;

    public Text RoundText, TimerText, ClueText;

    public Text[] ScoreText;

    public Text DiffText1, DiffText2, DiffText3, DiffText4;

    public Dropdown[] DifficultyPicker;
    public Dropdown RoundAmountField;

    public int SecondAmount, currentSeconds;
    public Difficulty difficulty;

    public int score;

    public Button DrawCardButton, AddAssociationButton, AddClueButton, startGameButton, DrawNewCardButton, EndRoundButton;

    public AudioSource GameEnd, RoundEnd;

    // Start is called before the first frame update
    void Start()
    {
        startedGame = false;
        RoundAmountField.interactable = true;
        resetEverything();
        GameOptions.SetActive(true);
        SetDifficulty();
        PauseBuffer.gameObject.SetActive(true);
        setDifficultyDescriptions();
        setRoundNumberDescription();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeDifficulty(int i)
    {
        difficulty = (Difficulty)DifficultyPicker[i].value;
    }

    public void startGame()
    {

        GameOptions.SetActive(false);

        if (!startedGame)
        {
            startedGame = true;
            DrawNewCardButton.interactable = true;
            playerAmount = RoundAmountField.value + 2;
            rounds = playerAmount;

            //SetDifficulty();

            setupDeck();
        }

        TimerText.GetComponent<AudioSource>().Play();

        startRound();



    }

    public void SetDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                AssociationAmount = 2;
                SecondAmount = 60;
                break;
            case Difficulty.Medium:
                AssociationAmount = 2;
                SecondAmount = 45;
                break;
            case Difficulty.Hard:
                AssociationAmount = 1;
                SecondAmount = 30;
                break;
            default:
                break;
        }

        currentSeconds = SecondAmount;

        
    }

    public void startRound()
    {
        if (rounds > 0)
        {
            CancelInvoke();

            rounds--;
            //SetDifficulty();
            restartDeck();
            //currentSeconds = 0;

            ClueText.text = "";

            DrawCardButton.interactable = (difficulty == Difficulty.Easy);
            AddAssociationButton.interactable = (difficulty == Difficulty.Easy || difficulty == Difficulty.Medium);

            InvokeRepeating("timerControl", 0f, 1);

            RestartHintButtons();

            DrawCard();
        }
        else
        {
            setScores();
        }
    }

    private void restartDeck()
    {
        foreach (var item in DiscardPile)
        {
            DrawPile.Add(item);
        }

        DrawPile = DrawPile.OrderBy(x => UnityEngine.Random.Range(0f, 100f)).ToList<Card>();

        DiscardPile.Clear();
    }

    public void AddAssociation()
    {
        AssociationBoxController box;

        do
        {
            box = AssociationCheckBoxes[UnityEngine.Random.Range(0, AssociationCheckBoxes.Length)];
        } while (box.gameObject.activeSelf || CurrentCard.getHintForAssociation(box.boxType) == "");

        box.gameObject.SetActive(true);
    }

    public void DrawCard()
    {
        CurrentCard = DrawPile[0];

        cardSpace.sprite = CurrentCard.image;

        DrawPile.RemoveAt(0);

        DiscardPile.Add(CurrentCard);

        if (DiscardPile.Count == Cards.Length)
        {
            restartDeck();
        }

        //RestartHintButtons();

        ClueImage.gameObject.SetActive(false);


        foreach (var item in AssociationCheckBoxes)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < AssociationAmount; i++)
        {
            AddAssociation();
        }

    }

    public void AddClue()
    {
        ClueImage.gameObject.SetActive(true);

        AssociationBoxController box;

        do
        {
            box = AssociationCheckBoxes[UnityEngine.Random.Range(0, AssociationCheckBoxes.Length)];
        } while (!box.gameObject.activeSelf);

        ClueText.text = CurrentCard.getHintForAssociation(box.boxType);
    }

    public void RestartHintButtons()
    {
        foreach (var item in AssociationCheckBoxes)
        {
            item.gameObject.SetActive(false);
        }

        AddClueButton.interactable = true;
        DrawCardButton.interactable = ( difficulty == Difficulty.Easy);
        AddAssociationButton.interactable = (difficulty == Difficulty.Easy || difficulty == Difficulty.Medium);

        
    }

    public void setupDeck()
    {
        DrawPile = new List<Card>();
        DiscardPile = new List<Card>();

        for (int i = 0; i < Cards.Length; i++)
        {
            DrawPile.Add(Cards[i]);
        }

        DrawPile = DrawPile.OrderBy(x => UnityEngine.Random.Range(0f, 100f)).ToList<Card>();
    }

    public void timerControl()
    {
        if (currentSeconds != 0)
        {
            currentSeconds--;

            int minutes = currentSeconds / 60;
            int seconds = (currentSeconds + 1) % 60;

            String time = ((minutes < 10) ? "0" : "")+ ((currentSeconds + 1) / 60).ToString() + ":" + ((seconds < 10) ? "0" : "") + ((currentSeconds + 1) % 60).ToString();

            TimerText.text = time;
        }
        else
        {
            TimerText.text = "00:00";
            currentSeconds = SecondAmount;
            CancelInvoke();

            EndRound();
        }
    }

    public void EndRound()
    {
        PauseBuffer.gameObject.SetActive(true);

        ClueImage.gameObject.SetActive(false);

        TimerText.GetComponent<AudioSource>().Stop();

        if (rounds > 0)
        {
            RoundEnd.Play();
            ResultScreen.SetActive(true);
            RoundText.text = (playerAmount - rounds + 1).ToString() + "/" + playerAmount;
            DifficultyPicker[1].value = (int)difficulty;
        }
        else
        {
            GameEnd.Play();
            GameEndScreen.SetActive(true);
            startedGame = false;
        }
    }

    public void resetEverything()
    {

        AssociationAmount = 1;
        score = 0;
        setScores();
        startedGame = false;
        RestartHintButtons();
        foreach (var item in DifficultyPicker)
        {
            item.ClearOptions();

            List<OptionData> ListData = new List<OptionData>() { 
                                                             new OptionData("לק"),
                                                             new OptionData("ינוניב"),
                                                             new OptionData("השק") };

            item.AddOptions(ListData);
        }
        ClueText.text = "";
        ClueImage.gameObject.SetActive(false);

        difficulty = (Difficulty)DifficultyPicker[0].value;
    }

    public void setScores()
    {
        foreach (var item in ScoreText)
        {
            item.text = score.ToString();
        }
    
    }

    public void addScore()
    {
        score++;
        setScores();
        
    }

    public void PauseGame()
    {
        PauseBuffer.gameObject.SetActive(true);
        PauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        PauseBuffer.gameObject.SetActive(false);
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void setDifficultyDescriptions()
    {
        DiffText1.text = ((SecondAmount == 0) ? "ףוס ןיא" : SecondAmount.ToString())+" :ןמז"; 
        DiffText2.text = AssociationAmount.ToString() + "-" + (AssociationAmount + 1).ToString() + " :תויצאיצוסא";

        int helpAmount = 3 - (int)difficulty;

        DiffText3.text = helpAmount + " :תורזע";


    }

    public void setRoundNumberDescription()
    {
        DiffText4.text = RoundAmountField.value + 2 + " :םיפתתשמ רפסמ";
    }
}
