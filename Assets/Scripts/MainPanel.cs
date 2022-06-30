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
        // ��ô��ھ��
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

        // ������ʱ��
        totalTime = FormatTime(audioSource.clip.length);

        // �رհ�ť
        closeBtn.onClick.AddListener(() => {
            Application.Quit();
        });

        // ��С����ť
        minBtn.onClick.AddListener(() => {
            // ��ô��ھ��
            var hwd = GetForegroundWindow();
            // ���ô�����С��
            ShowWindow(hwd, SW_SHOWMINIMIZED);
        });

        maxBtn.onClick.AddListener(() => {
            // ��ô��ھ��
            var hwd = GetForegroundWindow();
            // ���ô������
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
    /// ���ڷ��
    /// </summary>
    const int GWL_STYLE = -16;
    /// <summary>
    /// ������
    /// </summary>
    const int WS_CAPTION = 0x00c00000;

    /// <summary>
    /// ��С��
    /// </summary>
    const int SW_SHOWMINIMIZED = 2;

    /// <summary>
    /// ���
    /// </summary>
    const int SW_SHOWMAXIMIZED = 3;
#endif
}
