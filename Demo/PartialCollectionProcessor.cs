using System.Threading;

namespace Demo
{
    public class PartialCollectionProcessor
    {
        public int Compute(int[] items, int startIndex, int exclusiveEndIndex)
        {
            var result = 0;
            
            for (var i = startIndex; i < exclusiveEndIndex; i++)
            {
                Thread.Sleep(100);
                result += items[i];
            }

            return result;
        }
    }
}