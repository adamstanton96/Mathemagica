using System;
using System.Collections.Generic;

/////////////////////////////////////////////////////////////////////////////////////////////
//Binary tree node used to represent a numerical operation in tree-based formula generation//
/////////////////////////////////////////////////////////////////////////////////////////////

public class BasicOperatorNode: AbstractNode
{
    private char symbol; //Operator symbol defining node (+,-,*,/).

    //Generic binary tree variables...
    private AbstractNode left;
    private AbstractNode right;

    //Constructor//
    public BasicOperatorNode(int difficulty, int value)
    {
        this.symbol = Difficulty.getRandomBasicOperator(difficulty);
        switch (this.symbol)
        {
            case '+': generateAdditionNode(value); break;
            case '-': generateSubtractionNode(value); break;
            case '*': generateMultiplicationNode(value); break;
            case '/': generateDivisionNode(value); break;
        }
    }

    //Generates a node representing an addition operation within the formula//
    private void generateAdditionNode(int value)
    {
        //Based on the value provided, generates an addition operation and two number nodes equal to that value...
        int rightVal;
        if (value <= 1)
            rightVal = Difficulty.getRandomInt(value);
        else
            rightVal = Difficulty.getRandomInt(value - 1) + 1;

        int leftVal = value - rightVal;
        this.left = new NumberNode(leftVal);
        this.right = new NumberNode(rightVal);
    }

    //Generates a node representing a subtraction operation within the formula//
    private void generateSubtractionNode(int value)
    {
        //Based on the value provided, generates a subtraction operation and two number nodes equal to that value...
        int rightVal = Difficulty.getRandomInt(value)+1;
        int leftVal = value + rightVal;
        this.left = new NumberNode(leftVal);
        this.right = new NumberNode(rightVal);
    }

    //Generates a node representing a multiplication operation within the formula//
    private void generateMultiplicationNode(int value)
    {
        //Based on the value provided, generates a mutiplication operation and two number nodes equal to that value...
        //Due to reverse nature of operation node creation, division by zero must be taken into consideration...
        //Division by one and self is always possible, if possible, this method aims to avoid it, however it is potentially the only valid approach...
        List<int> options = new List<int>(); //List of potential operations that result in a correct value.
        int i = 2;
        while(i < value/2)
        {
            if(value % i == 0)
            {
                options.Add(i);
            }
            i++;
        }
        if (options.Count == 0)
        {
            int pick = Difficulty.getRandomInt(2);
            if (pick == 0)
            {
                this.symbol = '+';
                this.generateAdditionNode(value);
            }
            else
            {
                this.symbol = '-';
                this.generateSubtractionNode(value);
            }
        }
        else
        {
            int rightVal = options[Difficulty.getRandomInt(options.Count)];
            int leftVal = value / rightVal;
            this.left = new NumberNode(leftVal);
            this.right = new NumberNode(rightVal);
        }
    }

    //Generates a node representing a division opperation within the formula//
    private void generateDivisionNode(int value)
    {
        //Based on the value provided, generates a division operation and two number nodes equal to that value...
        int rightVal = Difficulty.getRandomInt(10) + 1;
        int leftVal = rightVal * value;
        this.left = new NumberNode(leftVal);
        this.right = new NumberNode(rightVal);
    }

    //Returns the overall difficulty rating of this node and its left and right subtrees//
    public override int evaluateDificulty()
    {
        int difficultyRating = Difficulty.getDifficulty(this.symbol);   //Initial difficulty rating based on current operator. 

        //Recursive call to determine difficulty of left and right sub trees and add them to the initial value...
        if(this.left != null)
        {
            difficultyRating += this.left.evaluateDificulty();
        }
        if (this.right != null)
        {
            difficultyRating += this.right.evaluateDificulty();
        }
        return difficultyRating;    //returns the total difficulty.
    }

    //Returns the total numerical value of operations attributed to this node//
	public override int getValue()
    {
        //Recursive call to determine node value based on subtree operators and values...
        switch (this.symbol)
        {
            case '+': return this.left.getValue() + this.right.getValue();
            case '-': return this.left.getValue() - this.right.getValue();
            case '*': return this.left.getValue() * this.right.getValue();
            case '/': return this.left.getValue() / this.right.getValue();
            default: return 0;
        }
    }

    //Fetches a leaf node from available subtrees - for use in creating new operation nodes//
    public override BasicOperatorNode fetch()
    {
        if(this.left is NumberNode || this.right is NumberNode)
        {  
            return this;
        }
        else
        {
            if (Difficulty.getRandomInt(2) == 0)
            {
                return left.fetch(); 
            }
            else
            {
                return right.fetch();
            }
        }
    }

    //Returns the value of leaf-numbernodes//
    public int getLeafValue()
    {
        if(this.left is NumberNode)
        {
            return this.left.getValue();
        }
        else
        {
            return this.right.getValue();
        }
    }

    //Replaces a numbernode with an operator node//
    public void replaceLeaf(BasicOperatorNode node)
    {
        if(this.left is NumberNode)
        {
            this.left = node;
        }
        else
        {
            this.right = node;
        }
    }

    //Returns the bodmas weighting of this node and its subtrees//
    public override int getBodmasWeight()
    {
        return Difficulty.getBodmas(this.symbol);
    }

    //ToString//
    public override string ToString()
    {
        String build = "";
        int weight = this.getBodmasWeight();
        if (this.left.getBodmasWeight() < weight)
        {
            build += "(" + this.left.ToString() + ")";
        }
        else
        {
            build += this.left.ToString();
        }
        build += this.symbol;
        int rightWeight = this.right.getBodmasWeight();
        if (rightWeight < weight || (rightWeight == 2 && weight == 2) || (rightWeight == 4 && weight == 4))
        {
            build += "(" + this.right.ToString() + ")";
        }
        else
        {
            build += this.right.ToString();
        }
        return build;
    }
}