using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Admin.Model
{
    public static class SQLExtension
    {
        public static void AddParam(this DbCommand? command, string key, object value)
        {
            if (command == null) return;

            var p = command.CreateParameter();
            p.ParameterName = key;
            p.Value = value;
            command.Parameters.Add(p);
        }

        public static int DisplayLength(this string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;

            int length = 0;
            foreach (char c in text)
            {
                if (c >= '\u1100' && (
            (c >= '\u1100' && c <= '\u115F') || // Hangul Jamo
            (c >= '\u2E80' && c <= '\uA4CF') || // CJK Radicals Supplement & Kangxi Radicals etc.
            (c >= '\uAC00' && c <= '\uD7A3') || // Hangul Syllables
            (c >= '\uF900' && c <= '\uFAFF') || // CJK Compatibility Ideographs
            (c >= '\uFE10' && c <= '\uFE19') || // Vertical Forms
            (c >= '\uFE30' && c <= '\uFE6F') || // CJK Compatibility Forms
            (c >= '\uFF00' && c <= '\uFF60') || // Full-width Forms
            (c >= '\uFFE0' && c <= '\uFFE6')))
                {
                    length += 2;
                }
                else
                {
                    length += 1;
                }
            }
            return length;
        }
    }
}
