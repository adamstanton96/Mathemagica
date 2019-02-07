using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////////////////
//Binary tree leaf node used to represent a numerical value in tree-based formula generation//
//////////////////////////////////////////////////////////////////////////////////////////////

public class NumberNode : AbstractNode
{

    private int value;

    public NumberNode(int value)
    {
        this.value = value;
    }

    public override int evaluateDificulty()
    {
        return Difficulty.getDifficulty(this.value);
    }

    public override int getValue()
    {
        return this.value;
    }

    public override string ToString()
    {
        return this.value.ToString();
    }

    public override BasicOperatorNode fetch()
    {
        return null;
    }

    public override int getBodmasWeight()
    {
        return Difficulty.getBodmas(this.value);
    }
}