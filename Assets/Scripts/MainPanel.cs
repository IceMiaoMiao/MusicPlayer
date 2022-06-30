using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Runtime.InteropServices;
using System;

public class MainPanel : MonoBehaviour
{
    public AudioEffect audioEffect;
    public AudioSource audioSource;

    public Button closeBtn;
    public Button minBtn;
    public Button maxBtn;
  
    public Button playBtn;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Slider slider;
    public Text timeText;

    private string totalTime;
    private StringBuilder timeSbr = new StringBuilder();
    private StringBuilder formatTimeSbr = new StringBuilder();

    private void Awake()
    {
        Screen.SetResolution(1280, 720, false);
#if UNITY_STANDALONE && !UNITY_EDITOR
        // 获得窗口句柄
        var hwd = GetForegroundWindow();
        var wl = GetWindowLong(hwd, GWL_STYLE);
        wl &= ~WS_CAPTION;
        SetWindowLong(hwd, GWL_STYLE, wl);
#endif
    }

    void Start()
    {
        audioSource.loop = true;
        playBtn.onClick.AddListener(() =>
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                playBtn.image.sprite = pauseSprite;
            }
            else
            {
                audioSource.Pause();
                playBtn.image.sprite = playSprite;
            }
        });
  

        slider.onValueChanged.AddListener((v) =>
        {
            if (v < 1)
                audioSource.time = v * audioSource.clip.length;
        });

        // 计算总时长
        totalTime = FormatTime(audioSource.clip.length);

        // 关闭按钮
        closeBtn.onClick.AddListener(() => {
            Application.Quit();
        });

        // 最小化按钮
        minBtn.onClick.AddListener(() => {
            // 获得窗口句柄
            var hwd = GetForegroundWindow();
            // 设置窗口最小化
            ShowWindow(hwd, SW_SHOWMINIMIZED);
        });

        maxBtn.onClick.AddListener(() => {
            // 获得窗口句柄
            var hwd = GetForegroundWindow();
            // 设置窗口最大化
            ShowWindow(hwd, SW_SHOWMAXIMIZED);
        });
    }

    void Update()
    {
        slider.value = audioSource.time / audioSource.clip.length;
        timeSbr.Clear();
        timeSbr.Append(FormatTime(audioSource.time));
        timeSbr.Append('/');
        timeSbr.Append(totalTime);
        timeText.text = timeSbr.ToString();
        if (audioSource.isPlaying)
        {
            audioEffect.UpdateEffect(audioSource);
        }
    }

    private string FormatTime(float t)
    {

        formatTimeSbr.Clear();
        var hour = Mathf.Floor(t / 3600);
        var miniute = Mathf.Floor(t % 3600 / 60);
        var sec = Mathf.Floor(t % 60);
        if (hour > 0)
        {
            formatTimeSbr.Append(hour.ToString().PadLeft(2, '0'));
            formatTimeSbr.Append(':');
            formatTimeSbr.Append(miniute.ToString().PadLeft(2, '0'));
            formatTimeSbr.Append(':');
            formatTimeSbr.Append(sec.ToString().PadLeft(2, '0'));
        }
        else
        {
            formatTimeSbr.Append(miniute.ToString().PadLeft(2, '0'));
            formatTimeSbr.Append(':');
            formatTimeSbr.Append(sec.ToString().PadLeft(2, '0'));
        }
        return formatTimeSbr.ToString();
    }

#if UNITY_STANDALONE
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    public static extern long GetWindowLong(IntPtr hwd, int nIndex);

    [DllImport("user32.dll")]
    public static extern void SetWindowLong(IntPtr hwd, int nIndex, long dwNewLong);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwd, int cmdShow);

    /// <summary>
    /// 窗口风格
    /// </summary>
    const int GWL_STYLE = -16;
    /// <summary>
    /// 标题栏
    /// </summary>
    const int WS_CAPTION = 0x00c00000;

    /// <summary>
    /// 最小化
    /// </summary>
    const int SW_SHOWMINIMIZED = 2;

    /// <summary>
    /// 最大化
    /// </summary>
    const int SW_SHOWMAXIMIZED = 3;
#endif
}
