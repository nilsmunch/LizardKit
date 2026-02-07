using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;

    private Action onAccept;
    private Action onReject;
    private float timer;
    private bool isRunning;

    public void Show(string message, Action acceptAction, Action rejectAction, int seconds)
    {
        messageText.text = message;
        onAccept = acceptAction;
        onReject = rejectAction;
        timer = seconds;
        isRunning = true;
        gameObject.SetActive(true);
        UpdateTimerDisplay();

        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(() => { Accept(); });

        rejectButton.onClick.RemoveAllListeners();
        rejectButton.onClick.AddListener(() => { Reject(); });
    }

    private void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;
        UpdateTimerDisplay();

        if (timer <= 0)
        {
            isRunning = false;
            Reject(); // Auto-reject
        }
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = Mathf.CeilToInt(timer).ToString();
    }

    private void Accept()
    {
        isRunning = false;
        gameObject.SetActive(false);
        onAccept?.Invoke();
    }

    private void Reject()
    {
        isRunning = false;
        gameObject.SetActive(false);
        onReject?.Invoke();
    }
}