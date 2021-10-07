using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Infinite loop to prompt input
            while (true)
            {
                Console.WriteLine("Please enter parameters you want to calculate: ");
                string sum = Console.ReadLine();

                try
                {
                    Console.WriteLine("The result is: " + Calculate(sum).ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Incorrect sequence of operators and operands detected!");
                }
            }
        }

        // Answer Method
        public static double Calculate(string sum)
        { 
            // To keep track of bracket opening and closing
            Stack<string> stack = new Stack<string>();

            // Split String to List
            List<string> list = sum.Split(' ').ToList();

            double num;
            // To inject '*' operator for a scenario where Eg. 4 ( 10 ) = 4 * ( 10 )
            for (int i = 0; i < list.Count; i++)
            {
                // Inject operator after closing bracket
                if (i + 1 < list.Count && list[i].Equals(")"))
                {
                    if (Double.TryParse(list[i + 1], out num))
                        list.Insert(i + 1, "*");
                }

                // Inject operator before closing bracket
                if (i - 1 >= 0 && list[i].Equals("("))
                {
                    if (Double.TryParse(list[i - 1], out num))
                        list.Insert(i, "*");
                }
            }

            // Convert all '-' + '-' to '+' or if single '-' and next value is an operand then append it together
            int negCount = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals("-"))
                {
                    ++negCount;

                    // Double '-' operator detected
                    if (negCount == 2)
                    {
                        list[i - 1] = "+";
                        list.RemoveAt(i);
                        --i;
                        negCount = 0;
                    }
                }
                else
                {
                    if (negCount == 1)
                    {
                        if (Double.TryParse(list[i], out num))
                        {
                            // If was '/' or '*' operator before '-' then no need insert '+' operator after saving operand as negative
                            if (i - 2 < 0 || (list[i - 2].Equals("/") || list[i - 2].Equals("*") || list[i - 2].Equals("(")))
                            {
                                list[i] = Convert.ToString(num * -1);
                                list.RemoveAt(i - 1);
                                --i;
                            }
                            else  // Eg. 1 - 4 = 1 + -4
                            {
                                list[i - 1] = "+";
                                list[i] = Convert.ToString(num * -1);
                            }
                        }
                    }
                    negCount = 0;
                }
            }

            // HashSet of Operators 
            HashSet<string> set = new HashSet<string>();

            // Insert Operators based on BODMAS
            set.Add("(");
            set.Add("/");
            set.Add("*");
            set.Add("+");
            set.Add("-");

            // Loop for each operator
            foreach (string op in set)
            {
                // If list already does not contain the operator, continue to next operator
                if (!list.Contains(op))
                    continue;

                // Keep looping if the current operator remains
                while(list.Contains(op))
                {
                    // Current position of operator
                    int pos = list.IndexOf(op);
                    int pos2 = pos + 1; // used to track range of bracket
                    double left = 0.0, right = 0.0;
                    
                    // If not a bracket operator, means its an operator that can be applied to left and right operand
                    if (!op.Equals("("))
                    {
                        right = Convert.ToDouble(list[pos + 1]);
                        left = Convert.ToDouble(list[pos - 1]);
                    }

                    double res = 0.0;
                    bool bracket = false;   // To store whether its a bracket or not

                    switch (op)
                    {
                        case "(":
                            // Push current opening bracket to stack
                            stack.Push(list[pos]);
                            StringBuilder sb = new StringBuilder();

                            // Loop through to find corresponding closing bracket
                            for (int j = pos + 1; j < list.Count; j++)
                            {
                                // Found another opening bracket, push to stack
                                if (list[j].Equals("("))
                                    stack.Push(list[j]);
                                    
                                // Found a closing bracket
                                else if (list[j].Equals(")") && stack.Count > 0)
                                {
                                    // Pop a corresponding opening bracket
                                    stack.Pop();

                                    // Stack is empty means we have closed the initial opening bracket
                                    if (stack.Count == 0)
                                    {
                                        pos2 = j; // Assign ending position
                                        // Use recursion to solve brackets / nested brackets
                                        res = Calculate(sb.ToString());
                                        bracket = true;
                                    }
                                }

                                sb.Append(list[j]);
                                sb.Append(" ");
                            }
                            break;
                        case "/":
                            res = left / right;
                            break;
                        case "*":
                            res = left * right;
                            break;
                        case "+":
                            res = left + right;
                            break;
                        case "-":
                            res = left - right;
                            break;
                    }

                    // Manipulate list and replace involved operators and operands to computed value
                    if (bracket)
                    {
                        // If current opeator was bracket means replace whole bracket range with computed result
                        list[pos] = res.ToString();
                        list.RemoveRange(pos + 1, pos2 - pos);
                    }
                    else  // /, *, + and - operations
                    {
                        // Negative result
                        if (res < 0)
                        {
                            // Last result, just store as negative result string
                            //if (pos - 1 == 0)
                            //{
                                list[pos - 1] = res.ToString();
                                list.RemoveRange(pos, 2);
                            //}
                            //else // Add '+' operator and save result as negative string
                            //{
                            //    list[pos - 1] = res.ToString();
                            //    list.RemoveRange(pos, 2);
                            //}
                        }
                        else  // Positive Result
                        {
                            list[pos - 1] = res.ToString();
                            list.RemoveRange(pos, 2);
                        }
                    }
                }
            }

            return Convert.ToDouble(list[0]);
        }
    }
}