
using System;

namespace Problem1480
{
    class Program
    {
        static int[] RunningSum(int[] nums)
        {
            for (int i = 1; i < nums.Length; i++)
            {
                nums[i] += nums[i - 1];
            }
            return nums;
        }

        static void print(int[] nums)
        {
            for(int i=0; i<nums.Length; i++)
            {
                Console.Write(nums[i]);
                Console.Write(" ");
            }
        }
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3, 4 };
            print(RunningSum(arr));
        }
    }
}
