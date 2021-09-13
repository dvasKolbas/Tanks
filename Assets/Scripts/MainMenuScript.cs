using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject RulesPanel;
    [SerializeField] private GameObject AuthorPanel;
    [SerializeField] private AudioSource StartMusic;
    [SerializeField] private AudioSource GameMusic;
    [SerializeField] private AudioSource Click;
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private Text SoundButtonMenu;
    [SerializeField] private Text SoundButtonGame;

    private bool sound;

    private void Start()
    {
        sound = true;
    }

    /// <summary>
    /// ������ ����� ����
    /// </summary>
    public void GameStart()
    {
        MainPanel.SetActive(false);
        GamePanel.SetActive(true);
        GameMusic.Play();
        StartMusic.Stop();
        FindObjectOfType<GameScript>().OnStart();
    }

    /// <summary>
    /// ������ ������ ����
    /// </summary>
    public void RulesStart()
    {
        MainPanel.SetActive(false);
        RulesPanel.SetActive(true);
        FindObjectOfType<RulesScript>().OnStart();
    }

    /// <summary>
    /// ������ ������� ����
    /// </summary>
    public void AuthorStart()
    {
        MainPanel.SetActive(false);
        AuthorPanel.SetActive(true);
    }

    /// <summary>
    /// ���������� ����� � ����
    /// </summary>
    public void ClickSound()
    {
        sound = !sound;
        AudioListener audio = MainCamera.GetComponent<AudioListener>();
        audio.enabled = sound;
        if (sound)
        {
            SoundButtonMenu.text = "����: ���";
            SoundButtonGame.text = "����: ���";
        }
        else
        {
            SoundButtonMenu.text = "����: ����";
            SoundButtonGame.text = "����: ����";
        }
    }

    /// <summary>
    /// ���� ������
    /// </summary>
    public void ButtonSound()
    {
        Click.Play();
    }
}
