﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkCommon.extention
{
    static class Util
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }
    }
}
