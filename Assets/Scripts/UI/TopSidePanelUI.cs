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

        public Button topBtn;

        private StateTopBtn _stateTopBtn;

        public Button levelUpBtn;

        public bool isButtonEnabled {
            get => topBtn.interactable;
            private set => topBtn.interactable = value;
        }

        public void RefreshBtnInteractable()
            => topBtn.interactable = IsInteractableBtn();

        private bool IsInteractableBtn() {

            if (GameMng.current == null)
                return false;

            if (stateTopBtn == StateTopBtn.ShowMenu && GameMng.current.selectPanel != null) {
                return !GameMng.current.selectPanel.IsShowed;
            } else {

                return GameMng.current.fightMng.isThereSomeOneToFight
                    && !GameMng.current.fightMng.isInFight;

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

            topBtn.onClick.AddListener(() => {

                if (stateTopBtn == StateTopBtn.Start) {
                    GameMng.current.fightMng.StartFight();
                } else {
                    GameMng.current.selectPanel.Show();
                }

            });

            GameMng.current.fightMng.fightTeamPlayer
                .onChangeTeamCtrl
                .AddListener(RefreshBtnInteractable);

        }

    }

}
