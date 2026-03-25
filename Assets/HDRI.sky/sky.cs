using UnityEngine;
using System.Collections;

public class SkyboxSmoothChanger : MonoBehaviour
{
    public Material[] skyboxes;

    // 1つのSkybox = 5分（300秒）
    public float totalTimePerSkybox = 300f;

    // フェード時間（この中に含まれる）
    public float fadeTime = 10f;

    int index = 0;

    void Start()
    {
        RenderSettings.skybox = skyboxes[index];
        StartCoroutine(SkyboxLoop());
    }

    IEnumerator SkyboxLoop()
    {
        while (true)
        {
            // 表示時間（フェード分を引く）
            yield return new WaitForSeconds(totalTimePerSkybox - fadeTime);

            // 暗くする（フェードアウト）
            yield return StartCoroutine(FadeExposure(1.3f, 0.4f));

            // Skybox変更
            index = (index + 1) % skyboxes.Length;
            RenderSettings.skybox = skyboxes[index];

            // 明るくする（フェードイン）
            yield return StartCoroutine(FadeExposure(0.4f, 1.3f));
        }
    }

    IEnumerator FadeExposure(float from, float to)
    {
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(from, to, t / fadeTime);
            RenderSettings.skybox.SetFloat("_Exposure", value);
            yield return null;
        }
    }
}