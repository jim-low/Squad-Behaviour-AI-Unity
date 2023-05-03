using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//What is a Sequence Node?
//A Sequence node is a type of Composite Node, composite nodes are in charge of looking through the states of their child Nodes.
//A Sequence node basically traveses it's child nodes from left to right.
//If ONE of the Sequence node's children return false, it is deemed false.
//If ALL of the Sequence node's children return true, it is deemed true.

public class Sequence : Node
{
    //firstly, make a list that helps to store the states of the child nodes.
    protected List<Node> m_nodes = new List<Node>();

    //the constructor, it should contain the list of node states from the children.
    public Sequence(List<Node> nodes)
    {
        m_nodes = nodes;
    }

    //check child nodes. IF ANY children return false, the selector is deemed false.
    //override the Evaluate() function from the Node Class. 
    public override NodeStates Evaluate()
    {
        bool anyChildRunning = false;

        foreach (Node node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState; // it fails, so return fail immediately

                case NodeStates.SUCCESS:
                    continue; //dont return yet, if it is a success, it should still continue checking other child nodes.

                case NodeStates.RUNNING:
                    anyChildRunning = true;
                    continue;

                default:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
            }
        }

        m_nodeState = anyChildRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
        return m_nodeState;

    }
}
