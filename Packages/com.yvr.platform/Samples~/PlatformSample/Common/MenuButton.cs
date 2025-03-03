using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private string m_SceneName;

    private void Start() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick); }

    public void OnClick()
    {
        Debug.Log($"MenuButton.OnClick: sceneName -> {m_SceneName}");

        SceneManager.LoadScene(m_SceneName);
    }
}