using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tween : MonoBehaviour
{
    [SerializeField] private RectTransform gameTitle;
    [SerializeField] private RectTransform pressStart;
    [SerializeField] private RectTransform menuButtonPanel;
    [SerializeField] private RectTransform playButtonPanel;
    [SerializeField] private RectTransform settingsPanel;
    [SerializeField] private RectTransform blackScreen;
    private TextMeshProUGUI pressStartText;

    private void Start()
    {
        gameTitle.position = new Vector3(-500, Screen.height/2, 0);
        gameTitle.localScale = new Vector3(1f, 1f, 1f);

        pressStartText = pressStart.GetComponent<TextMeshProUGUI>();
        pressStartText.gameObject.SetActive(false);
        pressStart.position = new Vector3(Screen.width / 2, Screen.height / 3, 0);

        menuButtonPanel.position = new Vector3(Screen.width * 3 / 10, -500, 0);
        menuButtonPanel.localScale = new Vector3(1f, 1f, 1f);
        menuButtonPanel.gameObject.SetActive(false);

        playButtonPanel.position = new Vector3(-500, Screen.height * 4 / 10, 0);
        playButtonPanel.localScale = new Vector3(1f, 1f, 1f);
        playButtonPanel.gameObject.SetActive(false);

        settingsPanel.position = new Vector3(Screen.width /2, Screen.height + 1000, 0);
        settingsPanel.localScale = new Vector3(1f, 1f, 1f);
        settingsPanel.gameObject.SetActive(false);

        blackScreen.gameObject.SetActive(false);

        TitleScreenTweenTitle();
    }

    private void TitleScreenTweenTitle()
    {
        bool hasTweened = false;
        LeanTween.moveX(gameTitle.gameObject, Screen.width * 3 / 4, 3f)
            .setEase(LeanTweenType.easeOutExpo)
            .setDelay(1f)
            .setOnUpdate((float val) =>
            {
                // When the tween is halfway done, scale down the object
                if (val >= 0.5f && !hasTweened)
                {
                    LeanTween.scaleX(gameTitle.gameObject, 0.7f, 2f).setEase(LeanTweenType.easeOutSine);
                    hasTweened = true;
                }
            })
            .setOnComplete(() =>
            {
                // When the move tween is done, scale the object back up
                LeanTween.scaleX(gameTitle.gameObject, 1f, 0.5f).setEase(LeanTweenType.easeOutBack).setDelay(0.01f);

                // Tween the press start text
                TitleScreenTweenPressStart();
            });
    }

    private void TitleScreenTweenPressStart()
    {   
        pressStartText.alpha = 0f;
        pressStart.gameObject.SetActive(true);
        // Flashing the press start text continuously
        LeanTween.value(pressStart.gameObject, 0f, 1f, 0.5f)
            .setOnUpdate((float val) =>
            {
                pressStartText.alpha = val;
                if (Input.anyKeyDown)
                {
                    OnPressStart();
                }
            })
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong()
            .setDelay(1f);
    }

    private void OnPressStart()
    {
        LeanTween.cancel(pressStart.gameObject);
        //flash repeatedly
        LeanTween.value(pressStart.gameObject, 1f, 0f, 0.2f)
            .setOnUpdate((float val) =>
            {
                pressStartText.alpha = val;
            })
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong(3)
            .setOnComplete(() =>
            {
                //fade out
                LeanTween.value(pressStart.gameObject, 1f, 0f, 0.3f)
                    .setOnUpdate((float val) =>
                    {
                        pressStartText.alpha = val;
                    })
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnComplete(() =>
                    {
                        pressStart.gameObject.SetActive(false);
                        pressStartText.alpha = 1f;
                        MenuScreenTweenTitle();
                        TweenShowMenuButtonPanel();
                    });
            });
    }

    private void MenuScreenTweenTitle()
    {
        //move title to top of screen, 1/4 width from left and 1/5 height from top. Scale to 0.5
        LeanTween.moveX(gameTitle.gameObject, Screen.width / 2, 1f).setEase(LeanTweenType.easeOutSine);
        LeanTween.moveY(gameTitle.gameObject, Screen.height * 4 / 5, 1f).setEase(LeanTweenType.easeOutSine);
        LeanTween.scale(gameTitle.gameObject, new Vector3(0.75f, 0.75f, 0.75f), 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenShowMenuButtonPanel()
    {
        //move button panel to 3/10 width from left and 4/10 height from bottom
        menuButtonPanel.gameObject.SetActive(true);
        LeanTween.moveY(menuButtonPanel.gameObject, Screen.height * 4 / 10, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenShowPlayButtonPanel()
    {
        //move button panel to 3/10 width from left and 4/10 height from bottom
        playButtonPanel.gameObject.SetActive(true);
        LeanTween.moveX(playButtonPanel.gameObject, Screen.width * 3 / 10, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenHideMenuButtonPanel()
    {
        // LeanTween.moveY(menuButtonPanel.gameObject, -500, 1f).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
        // {
        //     menuButtonPanel.gameObject.SetActive(false);
        // });
        LeanTween.moveY(menuButtonPanel.gameObject, -500, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenHidePlayButtonPanel()
    {
        // LeanTween.moveX(playButtonPanel.gameObject, -500, 1f).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
        // {
        //     playButtonPanel.gameObject.SetActive(false);
        // });
        LeanTween.moveX(playButtonPanel.gameObject, -500, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenShowSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(true);
        LeanTween.moveY(settingsPanel.gameObject, Screen.height / 2, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenHideSettingsPanel()
    {
        LeanTween.moveY(settingsPanel.gameObject, Screen.height + 1000, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenPlayGameTransition(GameObject menuCanvas)
    {
        blackScreen.gameObject.SetActive(true);
        LeanTween.alpha(blackScreen, 1f, 1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
        {
            menuCanvas.SetActive(false);
            LeanTween.alpha(blackScreen, 0f, 1f).setEase(LeanTweenType.easeInOutSine);
        });
    }
}
