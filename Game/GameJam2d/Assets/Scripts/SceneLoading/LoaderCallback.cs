using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used in the Loading Scene to load the target scene.
/// </summary>
public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update() {
        if (isFirstUpdate) {
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
