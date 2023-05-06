using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//What is a Sequence Node?
//A Sequence node is a type of Composite Node, composite nodes are in charge of looking through the states of their child Nodes.
//A Sequence node basically traveses it's child nodes from left to right.
//If ONE of the Sequence node's children return false, it is deemed false.
//If ALL of the Sequence node's children return true, it is deemed true.

namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence(): base() {}
        public Sequence(List<Node> children): base(children) {}

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children) {
                switch (node.Evaluate()) {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}
