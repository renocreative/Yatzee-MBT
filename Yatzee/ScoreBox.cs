using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Modeling;

namespace Yatzee
{
    public static partial class YatzeeModel
    {

        #region UpperSection Boxes

        /// <summary>
        /// Upper Section Score Box: 
        /// Regular Score: Sum of the total number of dice faces matching that box
        /// </summary>
        abstract class ScoreBoxNumbered : ScoreBox
        {

            public ScoreBoxNumbered() { section = BoxSection.Upper; }

            /// <summary>
            /// Represent the value number on the score box (between 1 and 6)
            /// </summary>
            protected int boxNumber = 0;

            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                foreach (Dice dice in dices)
                {
                    if (dice.value == boxNumber)
                        score += dice.value;
                }
                return score;
            }
        }

        /// <summary>
        /// One Box: 
        /// Regular Score: Sum of the total number of dice faces matching that box value one
        /// </summary>
        class ScoreBoxOne : ScoreBoxNumbered { public static string id = "BoxOne"; public ScoreBoxOne() { boxNumber = 1; } }

        /// <summary>
        /// Two Box: 
        /// Regular Score: Sum of the total number of dice faces matching that box value two
        /// </summary>
        class ScoreBoxTwo : ScoreBoxNumbered { public static string id = "BoxTwo"; public ScoreBoxTwo() { boxNumber = 2; } }

        /// <summary>
        /// Three Box: 
        /// Regular Score: Sum of the total number of dice faces matching that box value three
        /// </summary>
        class ScoreBoxThree : ScoreBoxNumbered { public static string id = "BoxThree"; public ScoreBoxThree() { boxNumber = 3; } }

        /// <summary>
        /// Four Box: 
        /// Regular Score: Sum of the total number of dice faces matching that box value four
        /// </summary>
        class ScoreBoxFour : ScoreBoxNumbered { public static string id = "BoxFour"; public ScoreBoxFour() { boxNumber = 4; } }

        /// <summary>
        /// Five Box: 
        /// Regular Score: Sum of the total number of dice faces matching that box value five
        /// </summary>
        class ScoreBoxFive : ScoreBoxNumbered { public static string id = "BoxFive"; public ScoreBoxFive() { boxNumber = 5; } }

        /// <summary>
        /// Six Box: 
        /// Regular Score: Sum of the total number of dice faces matching that box value six
        /// </summary>
        class ScoreBoxSix : ScoreBoxNumbered { public static string id = "BoxSix"; public ScoreBoxSix() { boxNumber = 6; } }

        /// <summary>
        /// For upper section bonus: 
        /// If a player scores a total of at least 63 points in the boxes of the upper section,
        /// a bonus of 35 points will be captured to this extra box.
        /// </summary>
        class UpperSectionBonusBox: BonusBox { 
            public UpperSectionBonusBox() { 
                section = BoxSection.Upper; 
            }
            
            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                if (ScoreBoxHelper.isUpperBonus(dices))
                {
                    score = 35;
                }
                return score;
            }
        }

        #endregion

        #region LowerSection Boxes

        /// <summary>
        /// Three-Of-A-Kind: At least three dice showing the same face.
        /// Regular Score: Sum of all dice
        /// </summary>
        class ThreeOfAKind : ScoreBox 
        {
            public static string id = "ThreeOfAKind"; 

            public ThreeOfAKind() { section = BoxSection.Lower; }

            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                if (ScoreBoxHelper.isThreeOfAKind(dices))
                {
                    foreach (Dice dice in dices)
                    {
                        score += dice.value;
                    }
                }
                return score;
            }
        }




        /// <summary>
        /// Four-Of-A-Kind: At least four dice showing the same face
        /// Regular Score: Sum of all dice
        /// </summary>
        class FourOfAKind : ScoreBox 
        {
            public static string id = "FourOfAKind";

            public FourOfAKind() { section = BoxSection.Lower; }

            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                if (ScoreBoxHelper.isFourOfAKind(dices))
                {
                    foreach (Dice dice in dices)
                    {
                        score += dice.value;
                    }
                }
                return score;
            }
        }

        /// <summary>
        /// Full House: A three-of-a-kind and a pair
        /// Regular Score: 25
        /// </summary>
        class FullHouse : ScoreBox 
        {
            public static string id = "FullHouse";

            public FullHouse() { section = BoxSection.Lower; }

            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                if (ScoreBoxHelper.isFullHouse(dices)) {
                    score = 25;
                }
                return score;
            }
        }

        /// <summary>
        /// Small Straight: Four sequential dice (1-2-3-4, 2-3-4-5, or 3-4-5-6)
        /// Regular Score: 30
        /// </summary>
        class SmallStraight : ScoreBox 
        {
            public static string id = "SmallStraight";

            public SmallStraight() { section = BoxSection.Lower; }

            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                if (ScoreBoxHelper.isSmallStraight(dices))
                {
                    score = 30;
                }
                return score;
            }
        }

        /// <summary>
        /// Large Straight: Five sequential dice (1-2-3-4-5 or 2-3-4-5-6)
        /// Regular Score: 40
        /// </summary>
        class LargeStraight : ScoreBox 
        {
            public static string id = "LargeStraight";

            public LargeStraight() { section = BoxSection.Lower; }

            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                if (ScoreBoxHelper.isLargeStraight(dices))
                {
                    score = 40;
                }
                return score;
            }
        }

        /// <summary>
        /// Yahtzee: All five dice showing the same face
        /// Regular Score: 50 (First Yahtzee only)
        /// </summary>
        class Yatzee : ScoreBox 
        {
            public static string id = "Yatzee"; 

            public Yatzee() { section = BoxSection.Lower; }

            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                if (ScoreBoxHelper.isYahtzee(dices))
                {
                    score = 50;
                }
                return score;
            }
        }

        /// <summary>
        /// Chance:	Any combination
        /// 
        /// Often acts as discard box for a turn that will not fit in another category (thus the name), 
        /// although during a lucky game it can be used to record a high score
        /// 
        /// Regular Score: Sum of all dice
        /// </summary>
        class Chance : ScoreBox 
        {
            public static string id = "Chance"; 

            public Chance() { section = BoxSection.Lower; }

            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                foreach (Dice dice in dices)
                {
                        score += dice.value;
                }
                return score;
            }
        }

        /// <summary>
        /// For lower section bonus: 
        /// Actually for the Yatzee bonuses. 
        /// If a player scores more additional Yahtzees during the same game, 
        /// that player is awarded Yahtzees Bonus points worth 100 points each 
        /// given that the first Yahtzee was placed in the 50-point Yahtzee score box.
        /// </summary>
        class LowerSectionBonusBox : BonusBox
        {
            
            public LowerSectionBonusBox() { 
                section = BoxSection.Lower; 
            }

            internal override int ComputeScore(Dice[] dices)
            {
                int score = 0;
                if (ScoreBoxHelper.isYahtzeeBonus(dices))
                {
                    score = 100;
                    
                }
                
                return score;
            }
        }

        #endregion
    }
}
