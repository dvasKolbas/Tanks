using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulesScript : MonoBehaviour
{
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject RulesPanel;
    [SerializeField] private GameObject FirstPanel;
    [SerializeField] private GameObject SecondPanel;
    [SerializeField] private GameObject ThirdPanel;
    [SerializeField] private GameObject FourthPanel;
    [SerializeField] private GameObject FifthPanel;
    [SerializeField] private GameObject SixthPanel;
    [SerializeField] private GameObject SeventhPanel;
    [SerializeField] private GameObject EighthPanel;
    [SerializeField] private Text ButtonText;

    private GameObject currentPanel;
    private int panelNumber;

    /// <summary>
    /// ������ ������ ����
    /// </summary>
    public void OnStart()
    {
        panelNumber = 1;
        currentPanel = FirstPanel;
        FirstPanel.SetActive(true);
        ButtonText.text = "�����";
    }

    /// <summary>
    /// ������� ������ �����
    /// </summary>
    public void ClickNext()
    {
        panelNumber++;

        switch (panelNumber)
        {
            case 2:
                ChangePanel(SecondPanel);
                break;
            case 3:
                ChangePanel(ThirdPanel);
                break;
            case 4:
                ChangePanel(FourthPanel);
                break;
            case 5:
                ChangePanel(FifthPanel);
                break;
            case 6:
                ChangePanel(SixthPanel);
                break;
            case 7:
                ChangePanel(SeventhPanel);
                break;
            case 8:
                ChangePanel(EighthPanel);
                ButtonText.text = "������� ����";
                break;
            case 9:
                MainMenu();
                break;
        }

    }

    /// <summary>
    /// ������������ �������
    /// </summary>
    /// <param name="panel">��������� ������</param>
    private void ChangePanel(GameObject panel)
    {
        currentPanel.SetActive(false);
        currentPanel = panel;
        currentPanel.SetActive(true);

    }

    /// <summary>
    /// ����� � ������� ����
    /// </summary>
    public void MainMenu()
    {
        MainPanel.SetActive(true);
        RulesPanel.SetActive(false);
    }
}
