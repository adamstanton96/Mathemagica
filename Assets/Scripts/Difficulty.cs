using System;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////////////////////
//Script for use in difficulty calculations as part of tree-based formula generation//
//////////////////////////////////////////////////////////////////////////////////////

public static class Difficulty
{
    private static Random r = new Random();
    private static List<Operator> operators = null;

    private static void load()
    {
        operators = new List<Operator>();
        operators.Add(new Operator('+', 1, 1));
        operators.Add(new Operator('-', 2, 2));
        operators.Add(new Operator('*', 3, 3));
        operators.Add(new Operator('/', 3, 4));
    }

    public static int getRandomInt(int bound)
    {
        return r.Next(bound);
    }

    public static char getRandomBasicOperator(int difficulty)
    {
        if (operators == null)
        {
            load();
        }

        if (difficulty < 1)
        {
            return '+';
        }

        List<Operator> subSet = new List<Operator>();
        foreach (Operator o in operators)
        {
            if (o.getDifficulty() <= difficulty)
            {
                subSet.Add(o);
            }
        }

        return subSet[r.Next(subSet.Count)].getSymbol();
    }

    public static int getBodmas(char symbol)
    {
        if (operators == null)
        {
            load();
        }

        foreach (Operator o in operators)
        {
            if (o.getSymbol() == symbol)
            {
                return o.getBodmasOrder();
            }
        }
        return 0;
    }

    public static int getBodmas(int value)
    {
        return 100;
    }

    public static int getDifficulty(char symbol)
    {
        if (operators == null)
        {
            load();
        }
        foreach (Operator o in operators)
        {
            if (o.getSymbol() == symbol)
            {
                return o.getDifficulty();
            }
        }
        return 0;
    }

    public static int getDifficulty(int value)
    {
        return 0;
    }
}
public class Operator
{
    private char symbol;
    private int difficulty;
    private int bodmasOrder;

    public Operator(char symbol, int difficulty, int bodmasOrder)
    {
        this.symbol = symbol;
        this.difficulty = difficulty;
        this.bodmasOrder = bodmasOrder;
    }

    public char getSymbol()
    {
        return this.symbol;
    }

    public int getDifficulty()
    {
        return this.difficulty;
    }

    public int getBodmasOrder()
    {
        return this.bodmasOrder;
    }
}