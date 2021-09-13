using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    #region Объекты Unity
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject ResultFightPanel;
    [SerializeField] private Image fightTimerImage;
    [SerializeField] private Image tankTimerImage;
    [SerializeField] private Image workerTimerImage;
    [SerializeField] private Image oilTimerImage;
    [SerializeField] private Text tankTimerText;
    [SerializeField] private Text tankCostText;
    [SerializeField] private Text tankNumberText;
    [SerializeField] private Text workerTimerText;
    [SerializeField] private Text workerCostText;
    [SerializeField] private Text workerNumberText;
    [SerializeField] private Text oilTimerText;
    [SerializeField] private Text oilrNumberText;
    [SerializeField] private Text fightTimerText;
    [SerializeField] private Text fightTankNumberText;
    [SerializeField] private Text fightOilNumberText;
    [SerializeField] private Text ResultFightText;
    [SerializeField] private Text GameNoticeText;
    [SerializeField] private Button TankButton;
    [SerializeField] private Button WorkerButton;
    [SerializeField] private Button PauseButton;
    [SerializeField] private AudioSource StartMusic;
    [SerializeField] private AudioSource GameMusic;
    [SerializeField] private AudioSource TankSound;
    [SerializeField] private AudioSource FightSound;
    [SerializeField] private AudioSource WorkerSound;
    [SerializeField] private AudioSource OilSound;
    #endregion

    #region Приватные переменные
    private bool fight, makeTank, makeWorker, makeOil, gameNotice, gamePause;
    private int tank, worker, oil, oilAdd, enemyTank, round, killTanks, oilSpent;
    private float fightTimer, tankTimer, workerTimer, oilTimer, gameNoticeTimer;
    #endregion

    #region Публичные переменные(для отладки параметров игры)
    public int tankStart, workerStart, oilStart, 
        tankCost, workerCost, workerMined,
        fightTime, tankTime, workerTime, oilTime,
        tankOilRate,
        maxRound, enemyResRound;
    #endregion

    /// <summary>
    /// Метод запуска игры,
    /// присваиваем значение переменным для запуска
    /// </summary>
    public void OnStart()
    {
        gamePause = false;
        GamePause();
        PauseButton.interactable = true;
        ResultFightText.text = "";
        GameNoticeText.text = "";
        enemyTank = 0;
        round = 0;
        fightTimer = fightTime;
        oilTimer = oilTime;
        fight = true;
        makeTank = false;
        makeWorker = false;
        makeOil = true;
        tankTimer = 0;
        workerTimer = 0;
        killTanks = 0;
        oilSpent = 0;

        tank = tankStart;
        TankButton.interactable = true;
        tankCostText.text = tankCost.ToString();
        tankNumberText.text = tank.ToString();

        worker = workerStart;
        WorkerButton.interactable = true;
        workerCostText.text = workerCost.ToString();
        workerNumberText.text = worker.ToString();

        oil = oilStart;
        oilAdd = worker * workerMined;
        oilrNumberText.text = oil.ToString();

        fightOilNumberText.text = (tankOilRate * tank).ToString();
        fightTankNumberText.text = enemyTank.ToString();
       
        CheckTimer(tankTimer, makeTank, tankTimerText, tankTimerImage, tankTime);
        CheckTimer(workerTimer, makeWorker, workerTimerText, workerTimerImage, workerTime);
        PrintGameNotice($"Осталось боев без врага: {enemyResRound - round}");
    }

    void Update()
    {
        //Сообщения о действиях в игре
        if(gameNotice)
        {
            gameNoticeTimer -= Time.deltaTime;
            if (gameNoticeTimer <= 0)
            {
                gameNotice = false;
                GameNoticeText.text = "";
            }
        }

        //Расчет времени до боя
        if (fight)
        {
            fightTimer -= Time.deltaTime;
            (fightTimer, fight) = CheckTimer(fightTimer, fight, fightTimerText, fightTimerImage, fightTime);
            if((fightTime-fightTimer) >= 3)
                ResultFightText.text = "";
            if (!fight)
            {
                CheckFight();
                if (round == maxRound)
                    WinGame();
                else
                {
                    fightTimer = fightTime;
                    fight = true;
                }
            }
        }

        //Расчет времени для создания танка
        if (makeTank)
        {
            tankTimer -= Time.deltaTime;
            (tankTimer, makeTank) = CheckTimer(tankTimer, makeTank, tankTimerText, tankTimerImage, tankTime);
            if (!makeTank)
            {
                TankSound.Play();
                tank++;
                tankNumberText.text = tank.ToString();
                fightOilNumberText.text = (tankOilRate * tank).ToString();
                TankButton.interactable = true;
            }
        }

        //Расчет времени для создания рабочего
        if (makeWorker)
        {
            workerTimer -= Time.deltaTime;
            (workerTimer, makeWorker) = CheckTimer(workerTimer, makeWorker, workerTimerText, workerTimerImage, workerTime);
            if (!makeWorker)
            {
                WorkerSound.Play();
                worker++;
                workerNumberText.text = worker.ToString();
                WorkerButton.interactable = true;
            }
        }

        //Расчет времени для создания нефти
        if (makeOil)
        {
            oilTimer -= Time.deltaTime;
            (oilTimer, makeOil) = CheckTimer(oilTimer, makeOil, oilTimerText, oilTimerImage, oilTime);
            if (!makeOil)
            {
                OilSound.Play();
                oil += oilAdd;
                oilrNumberText.text = oil.ToString();
                oilAdd = worker * workerMined;
                oilTimer = oilTime;
            }
            makeOil = true;
        }
    }

    /// <summary>
    /// Метод создания танка
    /// </summary>
    public void MakeTank()
    {
        if (tankCost <= oil)
        {
            oil -= tankCost;
            oilSpent += tankCost; 
            oilrNumberText.text = oil.ToString();
            makeTank = true;
            tankTimer = tankTime;
            TankButton.interactable = false;
        }
        else
            PrintGameNotice("Недостаточно нефти");
    }

    /// <summary>
    /// Метод создания рабочего
    /// </summary>
    public void MakeWorker()
    {
        if (workerCost <= oil)
        {
            oil -= workerCost;
            oilSpent += workerCost;
            oilrNumberText.text = oil.ToString();
            makeWorker = true;
            workerTimer = workerTime;
            WorkerButton.interactable = false;
        }
        else
            PrintGameNotice("Недостаточно нефти");
    }

    /// <summary>
    /// Метод проверки теймера на сброс
    /// </summary>
    /// <param name="timer">Текущее значение таймера</param>
    /// <param name="timerUse">Использование таймера</param>
    /// <param name="textArea">Текстовое отображение таймера</param>
    /// <param name="timerImage">Графическое отображение таймера</param>
    /// <param name="timerStart">Начальное значение таймера</param>
    /// <returns>Текущее значение таймера, использование таймера</returns>
    private (float, bool) CheckTimer(float timer, bool timerUse, Text textArea, Image timerImage, float timerStart)
    {
        if (timer <= 0)
        {
            timer = 0;
            timerUse = false;
            textArea.text = "";
            timerImage.fillAmount = 0;
        }
        else
        {
            textArea.text = Mathf.Round(timer).ToString();
            timerImage.fillAmount = timer / timerStart;
        }
        return (timer, timerUse);
    }

    /// <summary>
    /// Метод проверки боя
    /// </summary>
    private void CheckFight()
    {
        FightSound.Play();
        if (oil < tankOilRate * tank)
        {
            DefeatGame("В бою закончилась нефть");
            Debug.Log("Нефть");
        }
        else 
        {
            oil -= (tank * tankOilRate);
            oilrNumberText.text = oil.ToString();
            if (tank < enemyTank)
            {
                DefeatGame("Танки противника превзошли числом");
                Debug.Log("Танки");
            }

            else
            {
                Debug.Log("Победа");
                ResultFightText.text = "Победа";
                round++;
                oilSpent += (tank * tankOilRate);

                if (round > enemyResRound)
                {
                    killTanks += enemyTank;
                    tank -= enemyTank;
                    enemyTank++;
                }
                else if (round == enemyResRound)
                {
                    PrintGameNotice($"Осталось боев без врага: {enemyResRound - round}");
                    enemyTank++;
                }
                else
                    PrintGameNotice($"Осталось боев без врага: {enemyResRound - round}");

                tankNumberText.text = tank.ToString();
                fightTankNumberText.text = enemyTank.ToString();
            }
        }
    }

    /// <summary>
    /// Метод вывода игровых сообщений
    /// </summary>
    /// <param name="text"></param>
    private void PrintGameNotice(string text)
    {
        GameNoticeText.text = text;
        gameNotice = true;
        gameNoticeTimer = 3;
    }

    /// <summary>
    /// Метод победы в игре
    /// </summary>
    private void WinGame()
    {
        gamePause = true;
        PauseButton.interactable = false;
        Debug.Log("Победа в игре");
        Image img = ResultFightPanel.GetComponent<Image>();
        img.color = new Color(0.38f, 0.66f, 0.358f, 0.396f);
        GamePause();
        PrintGameNotice($"Сыграно раундов: {round}\n" +
                        $"Убито танков: {killTanks}\n" +
                        $"Потраченно нефти: {oilSpent} ");
        ResultFightText.text = "Победа в игре";
    }

    /// <summary>
    /// Метод поражение в игре
    /// </summary>
    private void DefeatGame(string text)
    {
        gamePause = true;
        PauseButton.interactable = false;
        Debug.Log("Поражение в игре");
        Image img = ResultFightPanel.GetComponent<Image>();
        img.color = new Color(0.641f, 0.337f, 0.317f, 0.396f);
        GamePause();
        PrintGameNotice($"{text}\n" +
                        $"Сыграно раундов: {round}\n" +
                        $"Убито танков: {killTanks}\n" +
                        $"Потраченно нефти: {oilSpent} ");
        ResultFightText.text = "Поражение";
    }

    /// <summary>
    /// Выход в главное меню
    /// </summary>
    public void ExitToMainMenu() 
    {
        MainPanel.SetActive(true);
        GamePanel.SetActive(false);
        GameMusic.Stop();
        StartMusic.Play();
    }

    /// <summary>
    /// Метод нажатия паузы
    /// </summary>
    public void ClickPause()
    {
        gamePause = !gamePause;
        Image img = ResultFightPanel.GetComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0f);
        GamePause();
        if (gamePause)
            PrintGameNotice("Пауза");
        else
            PrintGameNotice("");
       
    }

    /// <summary>
    /// Метод постановки игры на паузу
    /// </summary>
    private void GamePause()
    {
        ResultFightPanel.SetActive(gamePause);
        if (gamePause)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }

   




}
