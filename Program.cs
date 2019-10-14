using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthStats
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;
            String val;
            Random random = new Random();

            BankRobber bankRobber = new BankRobber();
            Cop cop = new Cop();

            while (isRunning == true)
            {
                // get input and convert to string
                val = Convert.ToString(Console.ReadLine());

                // check if program needs to be stopped
                if (val == "stop")
                {
                    isRunning = false;
                }
                else
                {
                    // update states based of vars and echo feedback
                    bankRobber.updateStates();
                    cop.updateStates();
                }
            }
        }
    }

    public class BankRobber
    {
        public float distanceToCop = 0;
        public float strength = 0;
        public float wealth = 0;
        private Random random = new Random();

        private State cState;

        public enum State
        {
            HAVING_GOOD_TIME = 0,
            ROBBING_BANK = 1,
            LAYING_LOW = 2,
            FLEEING = 3
        };

        public BankRobber()
        {
            cState = State.ROBBING_BANK;
        }

        public void updateStates()
        {
            switch (cState)
            {
                case State.LAYING_LOW:
                    strength += 15;

                    // feel safe
                    if (strength > 20)
                        cState = State.ROBBING_BANK;
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
                    wealth -= 5;
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
                    wealth -= 5;
                    distanceToCop += 30;
                    strength -= 5;

                    // feel safe
                    if (distanceToCop > 50 && strength > 10)
                        cState = State.ROBBING_BANK;
                    // get tired
                    if (strength < 10)
                        cState = State.LAYING_LOW;
                    break;
                default:
                    // do nothing
                    break;
            }

            // say current state
            sayFeedBack(cState);
        }

        private void sayFeedBack(State currentState)
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
                default:
                    feedback += "Broken";
                    break;
            }

            Console.WriteLine(feedback);
        }
    }

    public class Cop
    {
        private State cState;

        public enum State
        {
            OFF_DUTY = 0,
            ON_STAKE_OUT = 1,
            CHASING = 2
        };

        public Cop()
        {
            cState = State.ON_STAKE_OUT;
        }

        public void updateStates()
        {
            // update vars

            // say current state
            sayFeedBack(cState);
        }

        private void sayFeedBack(State currentState)
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
                default:
                    feedback += "Broken";
                    break;
            }

            Console.WriteLine(feedback);
        }
    }
}
