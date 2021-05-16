using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.City {

    [CreateAssetMenu(menuName = "City/NewsList")]
    public class NewsList : ScriptableObject {

        public NewsEntity[] newsEntities;

        public IEnumerable<NewsEntity> GetOpenedNewsList()
            => newsEntities.Where(ne => ne.newsConditionOr.All(n => n.Check()));

    }


}
