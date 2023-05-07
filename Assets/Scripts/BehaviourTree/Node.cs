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

        private static Dictionary<string, object> _dataContext = new Dictionary<string, object>();

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
            Debug.Log(_dataContext.Count);
        }

        public object GetData(string key)
        {
            Debug.Log("_dataContext count: " + _dataContext.Count);

            object val = null;
            if (_dataContext.TryGetValue(key, out val)) {
                Debug.Log(val);
                return val;
            }

            Node node = parent;
            while (node != null) {
                val = node.GetData(key);
                Debug.Log(val);
                if (val != null)
                    return val;

                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            bool cleared = false;
            if (_dataContext.ContainsKey(key)) {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null) {
                cleared = node.ClearData(key);
                if (cleared) {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

    }
}
