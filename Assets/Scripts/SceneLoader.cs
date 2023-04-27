using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject loaderScreen;
    public Slider slider;
    public Text textProgress;
    public int sceneIndex;
    private void Start()
    {
        StartCoroutine(LoadAsyncronously(sceneIndex));
    }
    //public void LoadScene(int sceneIndex)
    //{
    //    StartCoroutine(LoadAsyncronously(sceneIndex));
    //}
    //IEnumerator LoadAsyncronously(int sceneIndex)
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

    //    loaderScreen.SetActive(true);
    //    float value = 1f;
    //    while (!operation.isDone)
    //    {
    //        while (value == 1)
    //        {
    //            yield return new WaitForSeconds(5);
    //            slider.value += 0.1f;
                
    //            value-=0.1f;
    //        }
    //        //float progress = Mathf.Clamp01(operation.progress / .9f);
    //        //while (progress==1)
    //        //{

    //        //}
    //        //slider.value = progress/10;
    //        //textProgress.text = progress * 100f + "%";
    //        //yield return null;

    //    }
    
    IEnumerator LoadAsyncronously(int sceneIndex)
    {
        //float value = 1f;
            while (slider.value != 1)
            {
                yield return new WaitForSeconds(1);
                slider.value += 0.1f;
                textProgress.text = (slider.value * 100f).ToString("0") + "%";
        }
        SceneManager.LoadSceneAsync(sceneIndex);
    }

}
