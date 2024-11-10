namespace MatchingGame
{
    public class CardTag
    {
        public int cardId { get; set; }
        public bool isCorrect { get; set; }

        public CardTag(int id)
        {
            cardId = id;
            isCorrect = false;
        }

        public bool checkCorrect()
        {
            return isCorrect;
        }

        public void markAsCorrect()
        {
            isCorrect = true;
        }

    }
}