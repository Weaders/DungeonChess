using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.UI.NewsPopup {

    public class NewsPanel : MonoBehaviour {

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private NewsRow newsRowPrefab;

        [SerializeField]
        private Transform newsContainer;

        private void Reset() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void RefreshNewsList() {

            foreach (Transform tr in newsContainer)
                Destroy(tr.gameObject);

            var news = CityMng.current.cityData.GetCurrentNews();

            if (news == null)
                return;

            var openedListNews = news.GetOpenedNewsList();

            var rows = new List<NewsRow>(openedListNews.Count());

            foreach (var opendNews in openedListNews) {

                var row = Instantiate(newsRowPrefab, newsContainer);
                row.SetText(opendNews.headerKey, opendNews.textKey);
                rows.Add(row);

            }

            Canvas.ForceUpdateCanvases();

            foreach (var row in rows) {
                row.RecalcHeight();
            }

        }

        public void Show()
            => canvasGroup.Show();

        public void Hide()
            => canvasGroup.Hide();

    }
}
