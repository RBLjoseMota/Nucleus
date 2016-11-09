﻿// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a version of this string with the first character captalised
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CapitaliseFirst(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            else
                return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// Returns a version of this string with spaces automatically placed before CamelCase capitals
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AutoSpace(this string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (char.IsUpper(c) && i > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Does this string represent an integer value?
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(this string str)
        {
            return str.All(char.IsDigit);
        }

        /// <summary>
        /// Does this string represent a floating-point or
        /// integer value?
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string str)
        {
            double value;
            return double.TryParse(str, out value) && !double.IsNaN(value);
        }

        /// <summary>
        /// Removes all non-numeric characters from the start and end of this string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A new string with characters that do not count as numbers removed from the 
        /// start and end, or an empty string if there are no numbers at all in the starting 
        /// string.</returns>
        public static string TrimNonNumeric(this string str)
        {
            int firstNum = -1;
            int lastNum = -1;

            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsNumber(str[i]))
                {
                    if (firstNum < 0) firstNum = i;
                    lastNum = i;
                }
            }
            if (firstNum < 0) //No numeric characters found!
                return "";
            else return str.Substring(firstNum, lastNum + 1 - firstNum);
        }

        /// <summary>
        /// Removes all non-letter characters from the start and end of this string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A new string with characters that do not count as letters removed from the 
        /// start and end, or an empty string if there are no letters at all in the starting 
        /// string.  </returns>
        public static string TrimNonLetters(this string str)
        {
            int firstNum = -1;
            int lastNum = -1;

            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsLetter(str[i]))
                {
                    if (firstNum < 0) firstNum = i;
                    lastNum = i;
                }
            }
            if (firstNum < 0) //No letter characters found!
                return "";
            else return str.Substring(firstNum, lastNum + 1 - firstNum);
        }

        /// <summary>
        /// Converts the string into a set of integer ID numbers.
        /// The string must consist only of numbers separated by spaces and the 'to' keyword,
        /// used to indicate continuous incementing ranges of 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ICollection<int> ToIDSet(this string str)
        {
            HashSet<int> result = new HashSet<int>();
            string[] tokens = str.Split(' ');
            int lastID = int.MaxValue;
            bool waitingForTo = false;
            foreach (string token in tokens)
            {
                string trimmed = token.Trim().Trim(new char[]{ ',' });
                if (!string.IsNullOrEmpty(trimmed))
                {
                    if (trimmed.IsInteger())
                    {
                        int parsed = int.Parse(trimmed);
                        if (waitingForTo)
                        {
                                for (int i = Math.Min(lastID, parsed); i < Math.Max(parsed, lastID); i++)
                                {
                                    if (!result.Contains(i)) result.Add(i);
                                }
                            
                        }
                        if (!result.Contains(parsed)) result.Add(parsed);
                        lastID = parsed;
                        waitingForTo = false;
                    }
                    else
                    {
                        int iTo = trimmed.IndexOf("to", StringComparison.CurrentCultureIgnoreCase);
                        if (iTo > 0)
                        {
                            //There is something before the 'to' - possibly the starting number?
                            string beforeTo = trimmed.Substring(0, iTo);
                            if (beforeTo.IsInteger()) lastID = int.Parse(beforeTo);
                            else
                                throw new ArgumentException("The ID description was invalid.  Only integer ID numbers and the keyword 'to' are allowed.");
                        }
                        if (iTo >= 0)
                        {
                            //A 'to' exists somewhere in the token
                            if (trimmed.Length > iTo + 2)
                            {
                                //There is something after the 'to' - possibly the ending number
                                string afterTo = trimmed.Substring(iTo + 2);
                                if (afterTo.IsInteger())
                                {
                                    int endID = int.Parse(afterTo);
                                    for (int i = lastID; i <= endID; i++)
                                    {
                                        if (!result.Contains(i)) result.Add(i);
                                    }
                                }
                                else
                                    throw new ArgumentException("The ID description was invalid.  Only integer ID numbers and the keyword 'to' are allowed.");
                            }
                            else waitingForTo = true;
                        }
                        else
                            throw new ArgumentException("The ID description was invalid.  Only integer ID numbers and the keyword 'to' are allowed.");
                    }
                }
            }
            return result;
        }

        public static bool EqualsIgnoreCase(this string thisString, string other)
        {
            return thisString.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}