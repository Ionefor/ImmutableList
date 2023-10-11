using System.Collections.Immutable;

namespace ImmList
{
    public class ListNode<T>
    {
        public readonly T Value;
        public readonly ListNode<T> Next;

        public ListNode(T value, ListNode<T> next)
        {
            Value = value;
            Next = next;
        }
    }
    public class ImmutableList<T>
    {
        private readonly ListNode<T>[] listNodes;
        private int count;
        public int Count { get => count; }
        public T this[int index]
        {
            get => listNodes[index].Value;
        }
        public ImmutableList(ListNode<T>[] listNodes)
        {
            this.listNodes = listNodes;
            count = listNodes.Length;
        }
        public ImmutableList()
        {

        }
        public ImmutableList<T> Add(T value)
        {
            ListNode<T>[] temp;

            if (listNodes == null)
            {
                temp = new ListNode<T>[1];
                temp[0] = new ListNode<T>(value, null);
            }
            else
            {
                temp = AddWithReSizeArray(value);
            }

            count++;
            return new ImmutableList<T>(temp);
        }
        public ImmutableList<T> Replace(T value, int index)
        {
            if(index != listNodes.Length - 1)
            {
                listNodes[index] = new ListNode<T>(value, listNodes[index + 1]);
            }
            else
            {
                listNodes[index] = new ListNode<T>(value, null);
            }

            var newList = new ListNode<T>[listNodes.Length];
            newList[newList.Length - 1] = new ListNode<T>(listNodes[listNodes.Length - 1].Value, null);

            for (int i = listNodes.Length - 2; i >= 0; i--)
            {
                newList[i] = new ListNode<T>(listNodes[i].Value, newList[i + 1]);
            }

            return new ImmutableList<T>(newList);
        }
        public static ImmutableList<T> Union(ImmutableList<T> firstList, ImmutableList<T> secondtList)
        {
            var result = new ListNode<T>[firstList.Count + secondtList.Count];

            result[result.Length - 1] = new ListNode<T>(secondtList.listNodes[secondtList.Count - 1].Value, null);

            var firstCount = firstList.Count;
            var secondCount = secondtList.Count;

            for (int i = result.Length - 2; i >= 0; i--)
            {
                if (i > result.Length - secondtList.Count - 1)
                {
                    result[i] = new ListNode<T>(secondtList.listNodes[secondCount - 2].Value, result[i + 1]);
                    secondCount--;
                }
                else if (i == result.Length - secondtList.Count - 1)
                {
                    result[i] = new ListNode<T>(firstList.listNodes[firstCount - 1].Value, result[i + 1]);
                }
                else
                {
                    result[i] = new ListNode<T>(firstList.listNodes[firstCount - 2].Value, result[i + 1]);
                    firstCount--;
                }
            }
            return new ImmutableList<T>(result);
        }
        private ListNode<T>[] AddWithReSizeArray(T value)
        {
            var result = new ListNode<T>[listNodes.Length + 1];
            result[result.Length - 1] = new ListNode<T>(value, null);

            for (int i = result.Length - 2; i >= 0; i--)
            {
                result[i] = new ListNode<T>(listNodes[i].Value, result[i + 1]);
            }
            return result;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ImmutableList<int> list1 = new ImmutableList<int>();

            list1 = list1.Add(1);
            list1 = list1.Add(2);
            list1 = list1.Add(3);
            list1 = list1.Add(4);
            list1 = list1.Add(5);

            ImmutableList<int> list2 = new ImmutableList<int>();

            list2 = list2.Add(6);
            list2 = list2.Add(7);
            list2 = list2.Add(8);
            list2 = list2.Add(9);
            list2 = list2.Add(10);

            list2 = list2.Replace(99, 4);
            ImmutableList<int> list4 = ImmutableList<int>.Union(list1, list2);

            for (int i = 0; i < list4.Count; i++)
            {
                Console.WriteLine(list4[i]);
            }
        }
    }
}