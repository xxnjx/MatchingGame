using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    class Model
    {
        static int clickCount = 0;
        static int correctCount = 0;
        static int wrongCount = 0;
        public static CardTag[] cardTags = new CardTag[Config.numCards];
        static Random random = new Random();

        public static void init()
        {
            initializeCardTags();
            clickCount = 0;
            correctCount = 0;
            wrongCount = 0;
        }

        private static void initializeCardTags()
        {
            List<int> cardIds = new List<int>();

            for(int index = 0; index < Config.numCards; index++)
            {
                cardIds.Add(index / 2);
            }

            shuffle(cardIds);

            for(int index = 0; index < cardIds.Count; index++)
            {
                cardTags[index] = new CardTag(cardIds[index]);
            }
        }

        private static void shuffle(List<int> list)
        {
            int num = list.Count;

            for(int index = num - 1; index > 0; index--)
            {
                int rm = random.Next(0, index + 1);
                int temp = list[index];
                list[index] = list[rm];
                list[rm] = temp;
            }
        }

        public static string getCardName(int tag)
        {
            switch (tag % 10)
            {
                case 0: return "AC";
                case 1: return "2C";
                case 2: return "3C";
                case 3: return "4C";
                case 4: return "5C";
                case 5: return "6C";
                case 6: return "7C";
                case 7: return "8C";
                case 8: return "9C";
                case 9: return "10C";
                default: return "Back";
            }
        }

        public static bool isClickable(Button btn, Button first)
        {
            CardTag tag = btn.Tag as CardTag;
            return (tag != null && tag.checkCorrect()) || btn == first;
        }

        public static void increaseClickCount() => clickCount++;
        public static int getClickCount() => clickCount;
        public static void increaseCorrectCount() => correctCount++;
        public static int getCorrectCount() => correctCount;
        public static void increaseWrongCount() => wrongCount++;
        public static int getWrongCount() => wrongCount;
    }
}
