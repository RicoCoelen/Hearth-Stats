using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthStats
{
    static class Manager
    {
        public static bool isRunning = true;
    }

    class Program
    {
        static void Main(string[] args)
        {
            // current input
            String val;

            // make random class
            Random random = new Random();

            // initate objects
            BankRobber bankRobber = new BankRobber();
            Cop cop = new Cop();
            
            //give starting message
            Console.WriteLine("Simulation is starting...");

            while (Manager.isRunning == true)
            {
                // get input and convert to string
                val = Convert.ToString(Console.ReadLine());

                // check if program needs to be stopped
                if (val == "stop")
                {
                    Manager.isRunning = false;
                }
                else
                {
                    // update states based of vars and echo feedback
                    bankRobber.updateStates();
                    cop.updateStates(bankRobber);

                    // dev output
                    //Console.WriteLine(bankRobber.strength + " " + bankRobber.wealth + " " + bankRobber.distanceToCop + " " + cop.dutyTime);
                }
            }
            // wait till button press to exit
            Console.WriteLine("");
            Console.WriteLine("Simulation Ended, Press a key to exit...");
            Console.ReadLine();
        }
    }

    public class BankRobber
    {
        public float distanceToCop = 50;
        public float strength = 0;
        public float wealth = 0;

        private Random random = new Random();

         public State cState;

        public enum State
        {
            HAVING_GOOD_TIME = 0,
            ROBBING_BANK = 1,
            LAYING_LOW = 2,
            FLEEING = 3,
            CAUGHT = 4
        };

        public BankRobber()
        {
            cState = State.ROBBING_BANK;
        }

        public void updateStates()
        {
            // say current state
            sayFeedBack(cState);

            switch (cState)
            {
                case State.LAYING_LOW:
                    strength += 15;

                    // feel safe
                    if (strength > 20)
                        cState = State.ROBBING_BANK;

                    // spot cop
                    if (distanceToCop < 10)
                        cState = State.FLEEING;
                    break;

                case State.ROBBING_BANK:
                    wealth += 100;
                    distanceToCop -= 5;
                    strength -= 5;

                    // randomchance
                    if (random.Next(0, 100) < 25)
                        distanceToCop = 0;

                    // get rich
                    if (strength > 5 && wealth > 20)
                        cState = State.HAVING_GOOD_TIME;
                    // spot cop
                    if (distanceToCop < 20)
                        cState = State.FLEEING;
                    break;

                case State.HAVING_GOOD_TIME:
                    wealth -= 75;
                    strength -= 3;
                    distanceToCop -= 2;

                    // randomchance
                    if (random.Next(0, 100) < 15)
                        distanceToCop = 0;

                    // spot cop
                    if (distanceToCop < 10)
                    cState = State.FLEEING;
                    // get tired
                    if (strength < 5)
                        cState = State.LAYING_LOW;
                    break;

                case State.FLEEING:
                    wealth -= 100;
                    distanceToCop += 30;
                    strength -= 5;

                    // feel safe
                    if (distanceToCop > 10 && strength > 10 && wealth < 100)
                        cState = State.ROBBING_BANK;

                    // feel safe en rich
                    if (distanceToCop > 10 && strength > 10 && wealth > 100)
                        cState = State.HAVING_GOOD_TIME;

                    // get tired
                    if (strength < 10)
                        cState = State.LAYING_LOW;
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        public void sayFeedBack(State currentState)
        {
            String feedback = "Bank Robber: ";

            switch (currentState)
            {
                case State.HAVING_GOOD_TIME:
                    feedback += "I'm having a good time spending my money";
                    break;
                case State.ROBBING_BANK:
                    feedback += "I'm robbing banks and getting loads of money! Pew pew!";
                    break;
                case State.LAYING_LOW:
                    feedback += "I'm getting very tired, so I better lay low for a while";
                    break;
                case State.FLEEING:
                    feedback += "I see a cop, so I have to start running";
                    break;
                case State.CAUGHT:
                    feedback += "I'm Caught and im losing my freedom :C!";
                    break;
                default:
                    feedback += "Broken";
                    break;
            }

            Console.WriteLine(feedback);
        }
    }

    public class Cop
    {
        public State cState;
        public float dutyTime = 0;

        public enum State
        {
            OFF_DUTY = 0,
            ON_STAKE_OUT = 1,
            CHASING = 2,
            CAUGHT = 3
        };

        public Cop()
        {
            cState = State.ON_STAKE_OUT;
        }

        public void updateStates(BankRobber robber)
        {
            // say current state
            sayFeedBack(cState);

            switch (cState)
            {
                // update vars
                case State.ON_STAKE_OUT:
                    dutyTime -= 1;

                    if (robber.distanceToCop > 10 && dutyTime < 1)
                        cState = State.OFF_DUTY;

                    if (robber.distanceToCop < 10 && dutyTime > 0)
                        cState = State.CHASING;

                    break;

                case State.OFF_DUTY:
                    dutyTime = 10;
                    robber.distanceToCop = 50;

                    if (dutyTime > 5)
                        cState = State.ON_STAKE_OUT;
                    break;

                case State.CHASING:
                    dutyTime -= 1;
                    robber.distanceToCop -= 4;

                    if (robber.distanceToCop < 2) 
                        // make line and say feedback
                        Console.WriteLine(" ");
                        Console.WriteLine(" ");
                        cState = State.CAUGHT;
                        sayFeedBack(cState);
                        robber.cState = BankRobber.State.CAUGHT;
                        robber.sayFeedBack(robber.cState);
                        // stop simulation
                        Manager.isRunning = false;

                    if (robber.distanceToCop > 10 && dutyTime > 0)
                        cState = State.ON_STAKE_OUT;
                    break;

                default:
                    // do nothing
                    break;
            }
        }

        public void sayFeedBack(State currentState)
        {
            String feedback = "Cop: ";

            switch (currentState)
            {
                case State.OFF_DUTY:
                    feedback += "Duty time's over, I'm headed home";
                    break;
                case State.ON_STAKE_OUT:
                    feedback += "I'm on the lookout for any robbers";
                    break;
                case State.CHASING:
                    feedback += "I'm running behind a suspect";
                    break;
                case State.CAUGHT:
                    feedback += "I caught the robber and the city is finally safe!";
                    break;
                default:
                    feedback += "Broken";
                    break;
            }

            Console.WriteLine(feedback);
        }
    }
}
