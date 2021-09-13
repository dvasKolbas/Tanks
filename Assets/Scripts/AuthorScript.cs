using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorScript : MonoBehaviour
{
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject AuthorPanel;
    
    /// <summary>
    /// ����� � ������� ����
    /// </summary>
    public void MainMenu()
    {
        MainPanel.SetActive(true);
        AuthorPanel.SetActive(false);
    }
}
