﻿using System;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.TopSidePanel {

    public enum StateTopBtn { 
        Start, ShowMenu
    }

    public class TopSidePanelUI : MonoBehaviour {

        public TextMeshProUGUI moneyCount;

        public TextMeshProUGUI level;

        public TextMeshProUGUI charactersCount;

        public TextMeshProUGUI levelOfCharacters;

        public Button topBtn;

        private StateTopBtn _stateTopBtn;

        public bool isButtonEnabled {
            get => topBtn.interactable;
            private set => topBtn.interactable = value;
        }

        public void RefreshBtnInteractable()
            => topBtn.interactable = IsInteractableBtn();

        private bool IsInteractableBtn() {

            if (stateTopBtn == StateTopBtn.ShowMenu) {
                return !GameMng.current.selectPanel.IsShowed;
            } else {
                try {
                    return GameMng.current.fightMng.isThereSomeOneToFight
                        && !GameMng.current.fightMng.isInFight;
                } catch (Exception e) {
                    Debug.Log(e.Message);
                }

                return false;


            }

        }

        public StateTopBtn stateTopBtn {
            get => _stateTopBtn;
            set {
                _stateTopBtn = value;

                if (_stateTopBtn == StateTopBtn.Start)
                    topBtn.GetComponentInChildren<TextMeshProUGUI>().text = TranslateReader.GetTranslate("fight");
                else
                    topBtn.GetComponentInChildren<TextMeshProUGUI>().text = TranslateReader.GetTranslate("display_menu");

                RefreshBtnInteractable();
            }
        }

        public void Init() {

            moneyCount.Subscribe(GameMng.current.playerData.money);
            level.Subscribe(GameMng.current.level);

            charactersCount.Subscribe(
                () => $"{GameMng.current.playerData.charactersCount}/{GameMng.current.playerData.maxCharacterCount}", 
                OrderVal.UIUpdate, 
                GameMng.current.playerData.charactersCount, 
                GameMng.current.playerData.maxCharacterCount
            );

            topBtn.onClick.AddListener(() => {

                if (stateTopBtn == StateTopBtn.Start) {
                    GameMng.current.fightMng.StartFight();
                } else {
                    GameMng.current.selectPanel.Show();
                }

            });

            levelOfCharacters.Subscribe(GameMng.current.playerData.levelOfCharacters);

            GameMng.current.fightMng.fightTeamPlayer
                .onChangeTeamCtrl
                .AddListener(RefreshBtnInteractable);

        }

    }

}
