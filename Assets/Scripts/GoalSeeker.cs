using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalSeeker : MonoBehaviour
{
    Goal[] mGoals;
    Action[] mActions;
    Action mChangeOverTime;
    const float TICK_LENGTH = 3.0f;


    public Vector3 kitchenPos;
    public Vector3 bedroomPos;
    public Vector3 couchPos;
    public Vector3 restroomPos;
    public Vector3 televisionPos;
    private void Start()
    {
        mGoals = new Goal[4];
        mGoals[0] = new Goal("Eat", 4);
        mGoals[1] = new Goal("Sleep", 3);
        mGoals[2] = new Goal("Bathroom", 3);
        mGoals[3] = new Goal("Entertainment", 2);

        mActions = new Action[7];
        mActions[0] = new Action("eat some raw food");
        mActions[0].targetGoals.Add(new Goal("Eat", -4f));
        mActions[0].targetGoals.Add(new Goal("Sleep", +1f));
        mActions[0].targetGoals.Add(new Goal("Bathroom", +1f));
        mActions[0].targetGoals.Add(new Goal("Entertainment", +1f));

        mActions[1] = new Action("eat a snack");
        mActions[1].targetGoals.Add(new Goal("Eat", -2f));
        mActions[1].targetGoals.Add(new Goal("Sleep", +1f));
        mActions[1].targetGoals.Add(new Goal("Bathroom", +1f));
        mActions[1].targetGoals.Add(new Goal("Entertainment", 0f));

        mActions[2] = new Action("sleep in a bed");
        mActions[2].targetGoals.Add(new Goal("Eat", +2f));
        mActions[2].targetGoals.Add(new Goal("Sleep", -4f));
        mActions[2].targetGoals.Add(new Goal("Bathroom", +2f));
        mActions[2].targetGoals.Add(new Goal("Entertainment", 0f));

        mActions[3] = new Action("sleep on the couch");
        mActions[3].targetGoals.Add(new Goal("Eat", +1f));
        mActions[3].targetGoals.Add(new Goal("Sleep", -2f));
        mActions[3].targetGoals.Add(new Goal("Bathroom", +1f));
        mActions[3].targetGoals.Add(new Goal("Entertainment", +1f));

        mActions[4] = new Action("drink a soda");
        mActions[4].targetGoals.Add(new Goal("Eat", -2f));
        mActions[4].targetGoals.Add(new Goal("Sleep", -3f));
        mActions[4].targetGoals.Add(new Goal("Bathroom", +2f));
        mActions[4].targetGoals.Add(new Goal("Entertainment", +2f));

        mActions[5] = new Action("use the restroom");
        mActions[5].targetGoals.Add(new Goal("Eat", 0f));
        mActions[5].targetGoals.Add(new Goal("Sleep", 0f));
        mActions[5].targetGoals.Add(new Goal("Bathroom", -4f));
        mActions[5].targetGoals.Add(new Goal("Entertainment", +1f));

        mActions[6] = new Action("watch TV");
        mActions[6].targetGoals.Add(new Goal("Eat", +2f));
        mActions[6].targetGoals.Add(new Goal("Sleep", +1f));
        mActions[6].targetGoals.Add(new Goal("Bathroom", +2f));
        mActions[6].targetGoals.Add(new Goal("Entertainment", -3f));

        mChangeOverTime = new Action("tick");
        mChangeOverTime.targetGoals.Add(new Goal("Eat", +4f));
        mChangeOverTime.targetGoals.Add(new Goal("Sleep", +3f));
        mChangeOverTime.targetGoals.Add(new Goal("Bathroom", +2f));
        mChangeOverTime.targetGoals.Add(new Goal("Entertainment", +3f));

        Debug.Log("Starting clock. One hour will pass every " + TICK_LENGTH + " seconds.");
        InvokeRepeating("Tick", 0f, TICK_LENGTH);

        Debug.Log("Press E to do something");
    }

    void Tick()
    {
        foreach (Goal goal in mGoals)
        {
            goal.value += mChangeOverTime.GetGoalChange(goal);
            goal.value = Mathf.Max(goal.value, 0f);
        }

        PrintGoals();
    }

    void PrintGoals()
    {
        string goalString = "";
        foreach (Goal goal in mGoals)
        {
            goalString += goal.name + ": " + goal.value + "; ";
        }
        goalString += "Discontentment: " + CurrentDiscontentment();
        Debug.Log(goalString);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E");
            Action bestThingToDo = ChooseAction(mActions, mGoals);
            Debug.Log("I think I will " + bestThingToDo.name);

            foreach (Goal goal in mGoals)
            {
                goal.value += bestThingToDo.GetGoalChange(goal);
                goal.value = Mathf .Max(goal.value, 0f);
            }

            if (bestThingToDo == mActions[0])
            {
                transform.position = kitchenPos;
            }

            if (bestThingToDo == mActions[1])
            {
                transform.position = kitchenPos;
            }

            if (bestThingToDo == mActions[2])
            {
                transform.position = bedroomPos;
            }

            if (bestThingToDo == mActions[3])
            {
                transform.position = couchPos;
            }

            if (bestThingToDo == mActions[4])
            {
                transform.position = kitchenPos;
            }

            if (bestThingToDo == mActions[5])
            {
                transform.position = restroomPos;
            }

            if (bestThingToDo == mActions[6])
            {
                transform.position = televisionPos;
            }

            PrintGoals();
        }
    }

    Action ChooseAction(Action[] actions, Goal[] goals)
    {

        Action bestAction = null;
        float bestValue = float.PositiveInfinity;

        foreach (Action action in actions)
        {
            float thisValue = Discontentment(action, goals);

            if (thisValue < bestValue)
            {
                bestValue = thisValue;
                bestAction = action;
            }
        }

        return bestAction;
    }

    float Discontentment(Action action, Goal[] goals)
    {
        float discontentment = 0f;

        foreach(Goal goal in goals)
        {
            float newValue = goal.value + action.GetGoalChange(goal);
            newValue = Mathf.Max(newValue, 0);
            discontentment += goal.GetDiscontentment(newValue);
        }

        return discontentment;
    }

    float CurrentDiscontentment()
    {
        float total = 0f;
        foreach (Goal goal in mGoals)
        {
            total += (goal.value * goal.value);
        }

        return total;
    }
    
}