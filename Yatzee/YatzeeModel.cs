using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Modeling;

namespace Yatzee
{
    /// <summary>
    /// Yatzee model program.
    /// </summary>
    public static partial class YatzeeModel
    {
        #region Parameters
        static int MaximumOfAllowedRolls = 3;

        #endregion

        public static int SCORE_DEBUG = 0;

        #region Classes

        abstract class Box 
        {
            public BoxSection section;
            public int score = 0;

            internal abstract int ComputeScore(Dice[] dices);

            internal int Score()
            {
                score = ComputeScore(dices);
                return score;
            }
        }

        abstract class ScoreBox : Box
        {
            public bool hasBeenScored = false;

             new internal int Score()
            {
                score = ComputeScore(dices);

                hasBeenScored = true;
                return score;
            }
        }

        abstract class BonusBox : Box
        {
            new internal int Score()
            {
                score += ComputeScore(dices);
                return score;
            }
        }

        enum BoxSection
        { 
            Lower,
            Upper
        }

        /// <summary>
        /// Round in a Yatzee Game.
        /// </summary>
        enum Round
        {
            One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Eleven, Twelve, Thirtheen
        }

        #endregion

        #region State Model

        /// <summary>
        /// State of the Yatzee Game.
        /// </summary>
        enum GameState
        {
            Initialized,
            DiceRolled,
            Scored,
            GameOver
        }
        static GameState currentGameState = GameState.Initialized;

        /// <summary>
        /// The current round the player is into
        /// </summary>
        static Round currentRound = Round.One;

        /// <summary>
        /// The current overall score of the player
        /// </summary>
        static int overallScore = 0;

        /// <summary>
        /// The set of 5 standard dices for the player
        /// </summary>
        static Dice[] dices =
            new Dice[5]{
                new Dice(), new Dice(), new Dice(),
                new Dice(), new Dice()};

        #region Score boxes
        /// <summary>
        /// The score boxes.
        /// </summary>
        static MapContainer<string, ScoreBox> boxesToBeScored =
            new MapContainer<string, ScoreBox>(){
                new KeyValuePair<string,ScoreBox> (ScoreBoxOne.id, new ScoreBoxOne()), 
                new KeyValuePair<string,ScoreBox> (ScoreBoxTwo.id, new ScoreBoxTwo()),
                new KeyValuePair<string,ScoreBox> (ScoreBoxThree.id, new ScoreBoxThree()),
                new KeyValuePair<string,ScoreBox> (ScoreBoxFour.id, new ScoreBoxFour()),
                new KeyValuePair<string,ScoreBox> (ScoreBoxFive.id, new ScoreBoxFive()),
                new KeyValuePair<string,ScoreBox> (ScoreBoxSix.id, new ScoreBoxSix()),
                new KeyValuePair<string,ScoreBox> (ThreeOfAKind.id, new ThreeOfAKind()),
                new KeyValuePair<string,ScoreBox> (FourOfAKind.id, new FourOfAKind()),
                new KeyValuePair<string,ScoreBox> (FullHouse.id, new FullHouse()),
                new KeyValuePair<string,ScoreBox> (SmallStraight.id, new SmallStraight()),
                new KeyValuePair<string,ScoreBox> (LargeStraight.id, new LargeStraight()),
                new KeyValuePair<string,ScoreBox> (Chance.id, new Chance()),
                new KeyValuePair<string,ScoreBox> (Yatzee.id, new Yatzee())};

        /// <summary>
        /// The score boxes for bonuses
        /// </summary>
        static SetContainer<BonusBox> bonusBoxes =
            new SetContainer<BonusBox>() { new LowerSectionBonusBox(), new UpperSectionBonusBox() };

        #endregion 

        static int rollValue = 0;

        static int rollNumber = 0;

        #endregion

        /// <summary>
        /// Accepting state condition: Valid test cases will be the ones for which our last move was to score.
        /// </summary>
        [AcceptingStateCondition]
        public static bool IsAccepting
        {
            get
            {
                return currentGameState == GameState.Scored || currentGameState == GameState.GameOver;
            }
        }

        //The game is over when all score boxes have been scored.
        public static bool IsGameOver
        {
            get
            {
                foreach (ScoreBox box in boxesToBeScored.Values)
                {
                    if (!box.hasBeenScored) return false;
                }
                return true;
            }
        }

        #region Rules

        public static IEnumerable<int> DiceValuesAllowed { get { return new DiceValues(new int[] { 1, 2, 3, 4, 5, 6 }); } }

        /// <summary>
        /// A rule that models the action of rolling of the dice.
        /// </summary>
        [Rule]
        static void Roll(  [Domain("DiceValuesAllowed")]int dice1Value,
                                [Domain("DiceValuesAllowed")]int dice2Value,
                                [Domain("DiceValuesAllowed")]int dice3Value,
                                [Domain("DiceValuesAllowed")]int dice4Value,
                                [Domain("DiceValuesAllowed")]int dice5Value)
        {
            Requires(currentGameState != GameState.GameOver);
            Requires(rollNumber < MaximumOfAllowedRolls); //We can still roll if we have not reach our maximum of allowed rolls.
            
            if (dices[0].isHeld) Requires(dices[0].value == dice1Value);
            if (dices[1].isHeld) Requires(dices[1].value == dice2Value);
            if (dices[2].isHeld) Requires(dices[2].value == dice3Value);
            if (dices[3].isHeld) Requires(dices[3].value == dice4Value);
            if (dices[4].isHeld) Requires(dices[4].value == dice5Value);

            dices[0].value = dice1Value;
            dices[1].value = dice2Value;
            dices[2].value = dice3Value;
            dices[3].value = dice4Value;
            dices[4].value = dice5Value;

            currentGameState = GameState.DiceRolled;
            rollNumber++;
        }

        public static IEnumerable<string> ScoreBoxIdsAvailable{ get{ return boxesToBeScored.Keys; }}

        /// <summary>
        /// A rule that models the scoring by player in the chosen score box
        /// </summary>
        [Rule]
        static int Score([Domain("ScoreBoxIdsAvailable")]string idBox, out int scoreForTheRound)
        {
            ScoreBox box = boxesToBeScored[idBox];
            Requires(currentGameState == GameState.DiceRolled);
            Requires(!box.hasBeenScored);
            foreach (BonusBox bonusBox in bonusBoxes)
                bonusBox.Score();
            scoreForTheRound = box.Score();
            UpdateOverallScore();
            currentGameState = GameState.Scored;
            ResetForNextRound();
            return overallScore;
        }

        [Rule]
        static internal void Hold(bool dice1HeldValue, bool dice2HeldValue, 
            bool dice3HeldValue, bool dice4HeldValue, bool dice5HeldValue)
        {
            Requires(currentGameState == GameState.DiceRolled);
            Requires(!(dice1HeldValue == true &&
                     dice2HeldValue == true &&
                     dice3HeldValue == true &&
                     dice4HeldValue == true &&
                     dice5HeldValue == true));
            
            dices[0].isHeld = dice1HeldValue;
            dices[1].isHeld = dice2HeldValue;
            dices[2].isHeld = dice3HeldValue;
            dices[3].isHeld = dice4HeldValue;
            dices[4].isHeld = dice5HeldValue;

        }

        #endregion

        #region Helpers

        /// <summary>
        /// Reset the required variables right after the player scores prior to the next round
        /// </summary>
        static void ResetForNextRound()
        {
            if (IsGameOver)
            {
                currentGameState = GameState.GameOver;
                return;
            }
            rollValue = 0;
            rollNumber = 0;
            foreach (Dice dice in dices)
                dice.isHeld = false;
            currentRound++;
        }

        static void UpdateOverallScore()
        {
            int value = 0;
            foreach (ScoreBox box in boxesToBeScored.Values)
            {
                if(box.hasBeenScored)
                    value += box.score;
            }

            foreach (BonusBox box in bonusBoxes)
            {
                value += box.score;
            }
            overallScore = value;
        }

        /// <summary>
        /// Asserts a  requirement.
        /// </summary>
        /// <param name="condition"></param>
        static void Requires(bool condition)
        {
            Condition.IsTrue(condition);
        }

        #endregion
    }


    [TypeBinding("Yatzee.Sample.DiceImpl")]
    public class Dice
    {
        public int value = 0;
        public bool isHeld = false;

        public Dice() { }

        public Dice(int value){
            this.value = value;
        }
    }


    #region IEnumerable for dice values

    // IEnumerable for dice values to be integer between 1 and 6
    public class DiceValues : IEnumerable<int>
    {
        private int[] _diceValues;
        public DiceValues(int[] values)
        {
            _diceValues = new int[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                _diceValues[i] = values[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new DiceValuesEnum(_diceValues);
        }

        public class DiceValuesEnum : IEnumerator<int>
        {
            public int[] __diceValues;

            // Enumerators are positioned before the first element
            // until the first MoveNext() call.
            int position = -1;

            public DiceValuesEnum(int[] list)
            {
                __diceValues = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < __diceValues.Length);
            }

            public void Reset()
            {
                position = -1;
            }

             object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public int Current
            {
                get
                {
                    try
                    {
                        return __diceValues[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            public void Dispose() { }
        }
    }

    #endregion
}
