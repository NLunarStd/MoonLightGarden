using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject loadingScreen; 
    public Slider progressBar;

    public string gameplayScene;
    public void LoadSceneAsync()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(sceneToLoad)); 
    }
    IEnumerator LoadTilemapAndScene()
    {
        // ��Ŵ Scene ����� Tilemap
        AsyncOperation tilemapLoadOperation = SceneManager.LoadSceneAsync(gameplayScene, LoadSceneMode.Additive);
        while (!tilemapLoadOperation.isDone)
        {
            progressBar.value = tilemapLoadOperation.progress / 2f; // �Ѿഷ progress bar
            yield return null;
        }

        // ���� Tilemap � Scene �����Ŵ
        Scene tilemapScene = SceneManager.GetSceneByName(gameplayScene);
        Tilemap tilemap = FindTilemapInScene(tilemapScene);

        // Assign Tilemap (������ҧ)
        if (tilemap != null)
        {
            yield return StartCoroutine(LoadAndAssignTilemap(tilemap));
        }
        else
        {
            Debug.LogError("Tilemap not found in scene: " + gameplayScene);
        }

        // ��Ŵ Scene ��ѡ
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        sceneLoadOperation.allowSceneActivation = false;

        while (!sceneLoadOperation.isDone)
        {
            progressBar.value = 0.5f + sceneLoadOperation.progress / 2f; // �Ѿഷ progress bar
            if (sceneLoadOperation.progress >= 0.9f)
            {
                sceneLoadOperation.allowSceneActivation = true;
            }
            yield return null;
        }

        // Unload Scene ����� Tilemap
        SceneManager.UnloadSceneAsync(gameplayScene);
    }

    Tilemap FindTilemapInScene(Scene scene)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            Tilemap tilemap = rootObject.GetComponentInChildren<Tilemap>();
            if (tilemap != null)
            {
                return tilemap;
            }
        }
        return null;
    }

    IEnumerator LoadAndAssignTilemap(Tilemap tilemap)
    {
        // ��¹������Ѻ��Ŵ��� Assign Tilemap �����
        // ...

        // ������ҧ��è��ͧ�����Ŵ Tilemap (᷹�������鴨�ԧ�ͧ�س)
        for (int i = 0; i < 100; i++)
        {
            progressBar.value = 0.25f + i / 200f; // �Ѿഷ progress bar
            yield return null;
        }

        Debug.Log("Tilemap loaded and assigned!");
    }
    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); 
        operation.allowSceneActivation = false; 

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); 

            progressBar.value = progress; 

            if (operation.progress >= 0.9f) 
            {
                operation.allowSceneActivation = true;
            }

            yield return null; 
        }
    }
}
