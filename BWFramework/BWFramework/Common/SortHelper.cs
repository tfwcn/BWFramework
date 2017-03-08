using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.Common
{
    /// <summary>
    /// 排序
    /// </summary>
    public static class SortHelper
    {
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="left">开始位</param>
        /// <param name="right">结束位</param>
        public static void QuerySort(int[] array, int left, int right)
        {
            if (left >= right)
                return;
            int l = left;//左游标
            int r = right;//右游标
            int key = (l + r) / 2;//基数游标
            while (l < r)
            {
                if (array[r] > array[key])//先移动右游标，找到比基数小的数
                {
                    r--;
                }
                else if (array[l] < array[key])//移动左游标，找到比基数大的数
                {
                    l++;
                }
                else//互换值
                {
                    if (array[l] != array[r])
                    {
                        array[l] = array[r] + (array[r] = array[l]) * 0;
                    }
                    if (key == l)
                    {
                        key = r;
                    }
                    else if (key == r)
                    {
                        key = l;
                        r--;//把与基数相同的值放右边
                    }
                    else if (array[l] == array[r])
                    {
                        r--;//把与基数相同的值放右边
                    }
                }
            }
            QuerySort(array, left, key - 1);
            QuerySort(array, key + 1, right);
        }
    }
}
