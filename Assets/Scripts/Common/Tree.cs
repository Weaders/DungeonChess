using System.Collections.Generic;

namespace Assets.Scripts.Common {

    public class TreeNode<T> {

        private LinkedList<TreeNode<T>> _childs = new LinkedList<TreeNode<T>>();

        public TreeNode(T newData, int newLevel = 0) {
            data = newData;
            level = newLevel;
        }

        public T data { get; set; }

        public IEnumerable<TreeNode<T>> childs => _childs;

        public int level { get; private set; }

        public TreeNode<T> AddChild(T data) {

            var newNode = new TreeNode<T>(data, level + 1);
            _childs.AddLast(newNode);
            return newNode;

        }

    }
}
