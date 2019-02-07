
/////////////////////////////////////////////////////////////////////////////////////////////////
//Abstract tree node used as a parent for numerical and operation nodes for tree-based formulas//
/////////////////////////////////////////////////////////////////////////////////////////////////

public abstract class AbstractNode
{
	public abstract int evaluateDificulty();        
	public abstract int getValue();                
    public abstract BasicOperatorNode fetch();      
    public abstract int getBodmasWeight();         

    public abstract override string ToString();

    //Creates a tree based formula with a solution matching the result parameter, with a difficulty rating equal to the difficulty parameter//
    public static AbstractNode createTree(int result, int difficulty)
    {
        if (difficulty == 0)
        {
            return new NumberNode(result);
        }
        else
        {
            BasicOperatorNode root = new BasicOperatorNode(difficulty, result);
            bool generating = true;
            while (generating)
            {
                int currentDifficulty = root.evaluateDificulty();
                if (currentDifficulty < difficulty)
                {
                    BasicOperatorNode fetched = root.fetch();
                    int value = fetched.getLeafValue();
                    fetched.replaceLeaf(new BasicOperatorNode(difficulty - currentDifficulty, value));
                }
                else
                {
                    generating = false;
                }
            }
            return root;
        }
    }

}
