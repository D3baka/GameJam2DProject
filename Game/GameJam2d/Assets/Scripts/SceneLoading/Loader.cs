using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The loader class is a helper class used to Load scenes. It first loads a "Loading Scene" with a loading screen.
/// </summary>
public static class Loader {
    public enum Scene {
        MainMenu,
        GameScene,
        LoadingScene
    }


    private static Scene targetScene;

    public static void Load(Scene targetScene) {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }    

    public static void LoaderCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }

}
