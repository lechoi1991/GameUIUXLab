using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하기 위해 필요합니다.
using UnityEngine.SceneManagement;

public class LoadingUI : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private Image loadingBarImage; // LoadingBar_Fill 할당
    [SerializeField] private TextMeshProUGUI percentText; // Text_Percent 할당

    [Header("설정")]
    [SerializeField] private string nextSceneName = "MainMenu"; // 이동할 씬 이름

    private void Start()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        // 씬 전환 로딩 처리
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                UpdateUI(op.progress);
            }
            else
            {
                timer += Time.deltaTime;
                float currentProgress = Mathf.Lerp(0.9f, 1.0f, timer);
                UpdateUI(currentProgress);

                if (currentProgress >= 1.0f)
                {
                    op.allowSceneActivation = true;
                }
            }
        }
    }

    private void UpdateUI(float progress)
    {
        // 게이지 채우기 (0.0 ~ 1.0)
        if (loadingBarImage != null)
        {
            loadingBarImage.fillAmount = progress;
        }

        // 텍스트에 퍼센트 표시 (0% ~ 100%)
        if (percentText != null)
        {
            percentText.text = Mathf.RoundToInt(progress * 100f) + "%";
        }
    }
}