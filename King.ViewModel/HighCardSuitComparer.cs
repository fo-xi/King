using System.Collections.Generic;

namespace King.ViewModel
{
    public class HighCardSuitComparer : IComparer<CardVM>
    {
        public int Compare(CardVM x, CardVM y)
        {
            if (x.Suit != y.Suit)
            {
                if (x.Suit < y.Suit)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return y.CompareTo(x);
            }
        }
    }
}
