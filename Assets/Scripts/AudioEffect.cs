using UnityEngine;

/// <summary>
/// 声音特效
/// </summary>
public class AudioEffect : MonoBehaviour
{
    // 用于显示的方块
    public GameObject cubeObj;
    // 方块的其实点
    public Transform startPoint;

    // 采样数据长度
    private const int SPECTRUM_CNT = 512;
    // 采样数据
    private float[] spectrumData = new float[SPECTRUM_CNT];
    // 方块Transform数组
    private Transform[] cubeTransforms = new Transform[SPECTRUM_CNT];

    void Start()
    {
        //cube生成与排列
        Vector3 p = startPoint.position;

        for (int i = 0; i < SPECTRUM_CNT; i++)
        {
            p = new Vector3(p.x + 0.11f, p.y, p.z);
            GameObject cube = Instantiate(cubeObj, p, cubeObj.transform.rotation);
            cube.transform.parent = startPoint;
            cubeTransforms[i] = cube.transform;
        }
    }

    /// <summary>
    /// 当前帧频率波功率，传到对应cube的localScale
    /// </summary>
    public void UpdateEffect(AudioSource audioSource)
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        float scaleY;
        for (int i = 0; i < SPECTRUM_CNT; i++)
        {
            scaleY = Mathf.Lerp(cubeTransforms[i].localScale.y, spectrumData[i] * 10000f, 0.5f);
            // 限制一下功率
            if (scaleY > 400)
            {
                scaleY -= 400;
            }
            else if (scaleY > 300)
            {
                scaleY -= 300;
            }
            else if (scaleY > 200)
            {
                scaleY -= 200;
            }
            else if (scaleY > 100)
            {
                scaleY -= 100;
            }

            cubeTransforms[i].localScale = new Vector3(0.15f, scaleY, 0.15f);
        }
    }
}
