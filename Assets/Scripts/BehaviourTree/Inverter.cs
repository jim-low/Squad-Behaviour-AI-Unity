using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

//What is an Inverter Node?
//An Inverter node is a type of Decorator Node, decorator nodes can only have one child node, 
//A Decorator node alters its designated child node's status according to the type of decorator node.
//An Inverter serves to reverse the status it's child node, if it is true, turn it to false, opposite applies, it does nothing to the running status.

public class Inverter : Node
{
    /* //the child node to be evaluated, since decorators are defined to just have one child, no list is needed */
    /* private Node m_node; */

    /* public Node node */
    /* { */
    /*     get { return m_node; } */
    /* } */

    /* //for the constructor, it has to contain the status of the node. */
    /* public Inverter(Node node) */
    /* { */
    /*     m_node = node; */
    /* } */

    /* //return success if the child fails, or the opposite, reinitialize to running if its in running state. */
    /* //override the Evaluate() function from the Node Class. */ 
    /* public override NodeStates Evaluate() */
    /* { */
    /*     switch (m_node.Evaluate()) */
    /*     { */
    /*         case NodeStates.FAILURE: */
    /*             m_nodeState = NodeStates.SUCCESS; */
    /*             return m_nodeState; */
    /*         case NodeStates.SUCCESS: */
    /*             m_nodeState = NodeStates.FAILURE; */
    /*             return m_nodeState; */
    /*         case NodeStates.RUNNING: */
    /*             m_nodeState = NodeStates.RUNNING; */
    /*             return m_nodeState; */
    /*     } */
    /*     m_nodeState = NodeStates.SUCCESS; */
    /*     return m_nodeState; */
    /* } */
}
