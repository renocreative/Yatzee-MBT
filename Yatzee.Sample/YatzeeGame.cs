using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Modeling;

namespace Yatzee.Sample
{

    /// <summary>
    /// A dummy implementation that doesn't conform to the model (testing should fail).
    /// </summary>
    public static class YatzeeGame
    {
        public static void Roll(int dice1, int dice2, int dice3, int dice4, int dice5) { }

        public static int Score(string idBox, out int scoreForTheRound) { scoreForTheRound = 0; return 0; }

        public static void Hold(bool dice1HeldValue, bool dice2HeldValue, 
            bool dice3HeldValue, bool dice4HeldValue, bool dice5HeldValue) { }
    }

    public class DiceImpl
    {
        public int value = 0;
        public bool isHeld = false;
    }
}
