using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

using GameVanilla.Core;
using GameVanilla.Game.Common;
using GameVanilla.Game.Scenes;

namespace GameVanilla.Game.Popups
{
    public class SettingsPopup : Popup
    {
        [SerializeField]
        private ToggleGroup avatarToggleGroup;

        [SerializeField]
        private Slider soundSlider;

        [SerializeField]
        private Slider musicSlider;

        // 🔹 ویبره
        [SerializeField]
        private Slider vibrationSlider;

        [SerializeField]
        private AnimatedButton resetProgressButton;

        [SerializeField]
        private Image resetProgressImage;

        [SerializeField]
        private Sprite resetProgressDisabledSprite;

        private int currentAvatar;
        private int currentSound;
        private int currentMusic;
        private int currentVibration;

        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(avatarToggleGroup);
            Assert.IsNotNull(soundSlider);
            Assert.IsNotNull(musicSlider);
            Assert.IsNotNull(vibrationSlider); // 🔹
            Assert.IsNotNull(resetProgressButton);
            Assert.IsNotNull(resetProgressImage);
            Assert.IsNotNull(resetProgressDisabledSprite);
        }

        protected override void Start()
        {
            base.Start();

            var avatarSelected = PlayerPrefs.GetInt("avatar_selected");
            var toggles = avatarToggleGroup.GetComponentsInChildren<Toggle>();
            for (var i = 0; i < toggles.Length; i++)
            {
                toggles[i].isOn = i == avatarSelected;
            }

            soundSlider.value = PlayerPrefs.GetInt("sound_enabled", 1);
            musicSlider.value = PlayerPrefs.GetInt("music_enabled", 1);
            vibrationSlider.value = PlayerPrefs.GetInt("vibration_enabled", 1);

            currentSound = (int)soundSlider.value;
            currentMusic = (int)musicSlider.value;
            currentVibration = (int)vibrationSlider.value;
        }

        public void OnCloseButtonPressed()
        {
            Close();
        }

        public void OnSaveButtonPressed()
        {
            PlayerPrefs.SetInt("avatar_selected", currentAvatar);

            SoundManager.instance.SetSoundEnabled(currentSound == 1);
            SoundManager.instance.SetMusicEnabled(currentMusic == 1);

            // 🔹 ذخیره ویبره
            PlayerPrefs.SetInt("vibration_enabled", currentVibration);

            var homeScene = parentScene as HomeScene;
            if (homeScene != null)
            {
                homeScene.UpdateButtons();
            }

            Close();
        }

        public void OnResetProgressButtonPressed()
        {
            PuzzleMatchManager.instance.lastSelectedLevel = 0;
            PlayerPrefs.SetInt("next_level", 0);

            for (var i = 1; i <= 30; i++)
            {
                PlayerPrefs.DeleteKey($"level_stars_{i}");
            }

            resetProgressImage.sprite = resetProgressDisabledSprite;
            resetProgressButton.interactable = false;
        }

        public void OnHelpButtonPressed()
        {
            parentScene.OpenPopup<AlertPopup>("Popups/AlertPopup", popup =>
            {
                popup.SetTitle("Help");
                popup.SetText("Do you need help?");
            }, false);
        }

        public void OnInfoButtonPressed()
        {
            parentScene.OpenPopup<AlertPopup>("Popups/AlertPopup", popup =>
            {
                popup.SetTitle("About");
                popup.SetText("Created by gamevanilla.\n Copyright (C) 2018.");
            }, false);
        }

        public void OnGirlAvatarSelected()
        {
            currentAvatar = 0;
        }

        public void OnBoyAvatarSelected()
        {
            currentAvatar = 1;
        }

        public void OnSoundSliderValueChanged()
        {
            currentSound = (int)soundSlider.value;
        }

        public void OnMusicSliderValueChanged()
        {
            currentMusic = (int)musicSlider.value;
        }

        // 🔹 ویبره
        public void OnVibrationSliderValueChanged()
        {
            currentVibration = (int)vibrationSlider.value;
        }
    }
}
