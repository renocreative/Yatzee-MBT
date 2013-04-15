using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yatzee
{
    public static partial class YatzeeModel
    {
        class ScoreBoxHelper
        {
            public static bool isYatzeeScrathed = false;
            public static bool alreadyGotUpperBonus = false;
            public static int Ones = 0;
            public static int Twos = 0;
            public static int Threes = 0;
            public static int Fours = 0;
            public static int Fives = 0;
            public static int Sixes = 0;

            public static void initializer(Dice[] dices)
            {
                Ones = 0;
                Twos = 0;
                Threes = 0;
                Fours = 0;
                Fives = 0;
                Sixes = 0;

                foreach (Dice dice in dices)
                {
                    if (dice.value == 1)
                    {
                        Ones++;
                    }
                    if (dice.value == 2)
                    {
                        Twos++;
                    }
                    if (dice.value == 3)
                    {
                        Threes++;
                    }
                    if (dice.value == 4)
                    {
                        Fours++;
                    }
                    if (dice.value == 5)
                    {
                        Fives++;
                    }
                    if (dice.value == 6)
                    {
                        Sixes++;
                    }
                }
            }

            // At least three dice showing the same face
            public static Boolean isThreeOfAKind(Dice[] dices)
            {
                initializer(dices);
                if ((Ones >= 3) || (Twos >= 3) || (Threes >= 3) || (Fours >= 3) || (Fives >= 3) || (Sixes >= 3))
                {
                    return true;
                }
                return false;
            }

            // At least four dice showing the same face
            public static Boolean isFourOfAKind(Dice[] dices)
            {
                initializer(dices);
                if ((Ones >= 4) || (Twos >= 4) || (Threes >= 4) || (Fours >= 4) || (Fives >= 4) || (Sixes >= 4))
                {
                    return true;
                }
                return false;
            }

            // A three-of-a-kind and a pair
            public static Boolean isFullHouse(Dice[] dices)
            {
                initializer(dices);
                if (((Ones == 3) || (Twos == 3) || (Threes == 3) || (Fours == 3) || (Fives == 3) || (Sixes == 3)) && ((Ones == 2) || (Twos == 2) || (Threes == 2) || (Fours == 2) || (Fives == 2) || (Sixes == 2)))
                {
                    return true;
                }
                return false;
            }

            //Four sequential dice  (1-2-3-4, 2-3-4-5, or 3-4-5-6)
            public static Boolean isSmallStraight(Dice[] dices)
            {
                initializer(dices);
                if (((Ones >= 1) && (Twos >= 1) && (Threes >= 1) && (Fours >= 1)) || ((Twos >= 1) && (Threes >= 1) && (Fours >= 1) && (Fives >= 1)) || ((Threes >= 1) && (Fours >= 1) && (Fives >= 1) && (Sixes >= 1)))
                {
                    return true;
                }
                return false;
            }


            //Five sequential dice  (1-2-3-4-5 or 2-3-4-5-6)
            public static Boolean isLargeStraight(Dice[] dices)
            {
                initializer(dices);
                if (((Ones == 1) && (Twos == 1) && (Threes == 1) && (Fours == 1) && (Fives == 1)) || ((Twos == 1) && (Threes == 1) && (Fours == 1) && (Fives == 1) && (Sixes == 1)))
                {
                    return true;
                }
                return false;
            }

            // is it a Yahtzee
            public static Boolean isYahtzee(Dice[] dices)
            {
                initializer(dices);
                if (((Ones == 5) || (Twos == 5) || (Threes == 5) || (Fours == 5) || (Fives == 5) || (Sixes == 5)))
                {
                    return true;
                }
                return false;
            }

            // is it a Yahtzee Bonus
            // mYahtzeeBonus is equal to true if the Yatzee Bonus has already been scored
            public static Boolean isYahtzeeBonus(Dice[] dices)
            {
                if (isYahtzee(dices) && (boxesToBeScored[Yatzee.id].hasBeenScored == true))
                {
                    return true;
                }
                return false;
            }

            // is it a Bonus in the Upper Section
            // If a player scores a total of at least 63 points in the boxes of the upper section
            public static Boolean isUpperBonus(Dice[] dices)
            {
                if (!alreadyGotUpperBonus && SumOfUpperBoxes() >= 63)
                {
                    alreadyGotUpperBonus = true;
                    return true;
                }
                return false;
            }

            #region Helpers

            public static int SumOfUpperBoxes()
            {
                return (boxesToBeScored[ScoreBoxOne.id].score +
                        boxesToBeScored[ScoreBoxTwo.id].score +
                        boxesToBeScored[ScoreBoxThree.id].score +
                        boxesToBeScored[ScoreBoxFour.id].score +
                        boxesToBeScored[ScoreBoxFive.id].score +
                        boxesToBeScored[ScoreBoxSix.id].score);
            }

            #endregion
        }
    }
}
