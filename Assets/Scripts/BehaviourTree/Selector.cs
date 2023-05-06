using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//What is a selector Node?
//A selector node is a type of Composite Node, composite nodes are in charge of looking through the states of their child Nodes.
//A selector node basically traveses it's child nodes from left to right.
//If ALL the selector node's children return false, it is deemed false.
//If ALL the selector node's children return true, it is deemed true.

public class Selector : Node //A selector is a type of node, since a composite node is a also a node.
{
    //firstly, make a list that helps to store the states of the child nodes.
    protected List<Node> m_nodes = new List<Node>();

    //the constructor, it should contain the list of node states from the children.
    public Selector(List<Node> nodes)
    {
        m_nodes = nodes;
    }

    //check child nodes. IF all children return false, the selector is deemed false, the opposite applies.
    //override the Evaluate() function from the Node Class. 

    public override NodeStates Evaluate()
    {

        foreach (Node node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    continue;
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeStates.FAILURE;
        return m_nodeState;
    }
}
