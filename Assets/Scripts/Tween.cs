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
    [SerializeField] private RectTransform menuButton;
    private TextMeshProUGUI pressStartText;

    // Wall-mounted positions (wall at X=-7.51, Y=0-5, Z=-7.5 to 7.5)
    // X position is slightly in front of wall
    private Vector3 titleOffscreenPosition = new Vector3(-7.51f, 2.5f, 12f);  // Changed to positive Z for left-to-right movement
    private Vector3 titleMainPosition = new Vector3(-7.51f, 3.5f, 0f);
    private Vector3 titleTopPosition = new Vector3(-7.51f, 4.0f, 0f);  // Lowered Y position for menu
    private Vector3 pressStartPosition = new Vector3(-7.51f, 1.25f, 0f);  // Lowered to 1/4 height
    private Vector3 menuPanelOffscreenPosition = new Vector3(-7.51f, -2f, 0f);
    private Vector3 menuPanelPosition = new Vector3(-7.51f, 1.5f, 0f);
    private Vector3 playPanelOffscreenPosition = new Vector3(-7.51f, 2.5f, 12f);
    private Vector3 playPanelPosition = new Vector3(-7.51f, 2.5f, 5f);

    private void Start()
    {
        // Initialize positions for wall display
        gameTitle.position = titleOffscreenPosition;
        gameTitle.localScale = new Vector3(1f, 1f, 1f);

        pressStartText = pressStart.GetComponent<TextMeshProUGUI>();
        pressStartText.gameObject.SetActive(false);
        pressStart.position = pressStartPosition;

        menuButtonPanel.position = menuPanelOffscreenPosition;
        menuButtonPanel.localScale = new Vector3(1f, 1f, 1f);
        menuButtonPanel.gameObject.SetActive(false);

        playButtonPanel.position = playPanelOffscreenPosition;
        playButtonPanel.localScale = new Vector3(1f, 1f, 1f);
        playButtonPanel.gameObject.SetActive(false);

        TitleScreenTweenTitle();
    }

    private void TitleScreenTweenTitle()
    {
        bool hasTweened = false;
        // Move along Z axis (horizontal on wall) from right to left
        LeanTween.moveZ(gameTitle.gameObject, titleMainPosition.z, 3f)
            .setEase(LeanTweenType.easeOutExpo)
            .setDelay(1f)
            .setOnUpdate((float val) =>
            {
                // When the tween is halfway done, scale down the object
                if (val >= 0.5f && !hasTweened)
                {
                    LeanTween.scale(gameTitle.gameObject, new Vector3(0.7f, 0.7f, 0.7f), 2f).setEase(LeanTweenType.easeOutSine);
                    hasTweened = true;
                }
            })
            .setOnComplete(() =>
            {
                // When the move tween is done, scale the object back up
                LeanTween.scale(gameTitle.gameObject, new Vector3(1f, 1f, 1f), 0.5f).setEase(LeanTweenType.easeOutBack).setDelay(0.01f);

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
                OnPressStart();
            })
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong()
            .setDelay(1f);
    }

    private void OnPressStart()
    {
        LeanTween.cancel(pressStart.gameObject);
        // Flash repeatedly
        LeanTween.value(pressStart.gameObject, 1f, 0f, 0.2f)
            .setOnUpdate((float val) =>
            {
                pressStartText.alpha = val;
            })
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong(3)
            .setOnComplete(() =>
            {
                // Fade out
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
        // Move title to top position on wall
        LeanTween.moveZ(gameTitle.gameObject, titleTopPosition.z, 1f).setEase(LeanTweenType.easeOutSine);
        LeanTween.moveY(gameTitle.gameObject, titleTopPosition.y, 1f).setEase(LeanTweenType.easeOutSine);
        LeanTween.scale(gameTitle.gameObject, new Vector3(0.75f, 0.75f, 0.75f), 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenShowMenuButtonPanel()
    {
        menuButtonPanel.gameObject.SetActive(true);
        LeanTween.moveY(menuButtonPanel.gameObject, menuPanelPosition.y, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenShowPlayButtonPanel()
    {
        playButtonPanel.gameObject.SetActive(true);
        LeanTween.moveZ(playButtonPanel.gameObject, playPanelPosition.z, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenHideMenuButtonPanel()
    {
        LeanTween.moveY(menuButtonPanel.gameObject, menuPanelOffscreenPosition.y, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void TweenHidePlayButtonPanel()
    {
        LeanTween.moveZ(playButtonPanel.gameObject, playPanelOffscreenPosition.z, 1f).setEase(LeanTweenType.easeOutSine);
    }
}
