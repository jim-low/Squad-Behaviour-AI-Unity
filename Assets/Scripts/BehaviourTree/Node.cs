using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children) {
                _Attach(child);
            }
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object val = null;
            if (_data.TryGetValue(key, out val)) {
                return val;
            }

            Node node = _parent;
            if (node != null) {
                val = node.GetData(key);
            }
            return val;
        }

        public bool ClearData(string key)
        {
            bool cleared = false;
            if (_data.ContainsKey(key)) {
                _data.Remove(key);
                return true;
            }

            Node node = parent;
            if (node != null) {
                cleared = node.ClearData(key);
            }
            return cleared;
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

    }
}
