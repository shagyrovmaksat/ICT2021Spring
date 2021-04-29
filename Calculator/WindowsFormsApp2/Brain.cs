using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    delegate void DisplayMessage(string text);

    class Brain
    {
        DisplayMessage displayMessage;
        
        public Brain(DisplayMessage displayMessageDelegate)
        {
            this.displayMessage = displayMessageDelegate;
        }

        public double factorial(double num)
        {
            double res = 1;
            for (double i = 1; i <= num; i++)
            {
                res *= i;
            }
            return res;
        }

        string[] nonZeroDigit = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string[] digit = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string[] zero = { "0" };
        string[] operations = { "+", "-", "*", "/"};
        string[] specialOperations = { "±", "√a", "log10", "x!", "ln", "x^2", "x^2" };
        string[] equals = { "=" };
        string[] separator = { "," };

        enum State
        {
            Zero, 
            AccumulateDigits,
            AccumulateDigitsDecimal,
            ComputePending,
            Compute,
            ComputeSpecialOperation
        }

        State currentState = State.Zero;
        string previousNumber = "";
        string currentNumber = "";
        string currentOperation = "";

        public void ProccesSignal(string message)
        {
            if(message == "AC")
            {
                currentState = State.Zero;
                previousNumber = "";
                currentNumber = "";
                currentOperation = "";
                displayMessage("0");
                return;
            }

            switch(currentState)
            {
                case State.Zero:
                    ProccesZeroState(message, false);
                    break;
                case State.AccumulateDigits:
                    ProccesAccumulateDigits(message, false);
                    break;
                case State.ComputePending:
                    ProccesComputePending(message, false);
                    break;
                case State.Compute:
                    ProccesCompute(message, false);                    
                    break;
                case State.AccumulateDigitsDecimal:
                    ProccesAccumulateDigitsDecimal(message, false);
                    break;
                case State.ComputeSpecialOperation:
                    ProccesComputeSpecialOperation(message, false);
                    break;
                default:
                    break;
            }
        }

        void ProccesZeroState(string message, bool income)
        {

            if (income)
            {
                currentState = State.Zero;
                previousNumber = "";
                currentOperation = "";
                currentNumber = "0";
                displayMessage(currentNumber);
            }
            else
            {
                if (nonZeroDigit.Contains(message))
                {
                    ProccesAccumulateDigits(message, true);
                }
                else if (separator.Contains(message))
                {
                    ProccesAccumulateDigitsDecimal(message, true);
                }
                //else if message is zero just ignore
            }
        }

        void ProccesComputeSpecialOperation(string message, bool income)
        {

            if (income)
            {
                if(currentNumber != "")
                {
                    currentState = State.ComputeSpecialOperation;
                    double num = double.Parse(currentNumber);
                    if (message == "√a")
                    {
                        if (num >= 0)
                        {
                            double res = Math.Sqrt(num);
                            currentNumber = res.ToString();
                            displayMessage(currentNumber);
                        }
                    }
                    else if(message == "±")
                    {
                        currentNumber = (-num).ToString();
                        displayMessage(currentNumber);
                    }
                    else if (message == "3√a")
                    {
                        if (num >= 0)
                        {
                            double res = Math.Pow(num, (1.0/3.0));
                            currentNumber = res.ToString();
                            displayMessage(currentNumber);
                        }
                    }
                    else if (message == "log10")
                    {
                        if (num > 0)
                        {
                            double res = Math.Log10(num);
                            currentNumber = res.ToString();
                            displayMessage(currentNumber);
                        }
                    }
                    else if (message == "x!")
                    {
                        if (num > 2)
                        {
                            double res = this.factorial(num);
                            currentNumber = res.ToString();
                            displayMessage(currentNumber);
                        }
                    }
                    else if (message == "ln")
                    {
                        if (num > 0)
                        {
                            double res = Math.Log(num);
                            currentNumber = res.ToString();
                            displayMessage(currentNumber);
                        }
                    }
                    else if (message == "x^2")
                    {
                        currentNumber = (Math.Pow(num, 2)).ToString();
                        displayMessage(currentNumber);
                    }
                    else if (message == "x^3")
                    {
                        currentNumber = (Math.Pow(num, 3)).ToString();
                        displayMessage(currentNumber);
                    }
                }
            }
            else
            {
                if (nonZeroDigit.Contains(message))
                {
                    ProccesAccumulateDigits(message, true);
                }
                else if (separator.Contains(message))
                {
                    ProccesAccumulateDigitsDecimal(message, true);
                }
                else if (operations.Contains(message))
                {
                    ProccesComputePending(message, true);
                }
                else if (equals.Contains(message))
                {
                    ProccesCompute(message, true);
                }
                else if (specialOperations.Contains(message))
                {
                    ProccesComputeSpecialOperation(message, true);
                }
            }
        }

        void ProccesAccumulateDigits(string message, bool income)
        {
            if(income)
            {
                currentState = State.AccumulateDigits;
                if (zero.Contains(currentNumber))
                {
                    currentNumber = message;
                }
                else
                {
                    currentNumber = currentNumber + message;
                }
                displayMessage(currentNumber);
            }
            else
            {
                if (digit.Contains(message))
                {
                    ProccesAccumulateDigits(message, true);
                } 
                else if(operations.Contains(message))
                {
                    ProccesComputePending(message, true);
                } 
                else if(equals.Contains(message))
                {
                    ProccesCompute(message, true);
                }
                else if(separator.Contains(message))
                {
                    ProccesAccumulateDigitsDecimal(message, true);
                }
                else if (specialOperations.Contains(message))
                {
                    ProccesComputeSpecialOperation(message, true);
                }
            }
        }

        void ProccesAccumulateDigitsDecimal(string message, bool income)
        {
            if(income)
            {
                currentState = State.AccumulateDigitsDecimal;
                if(separator.Contains(message))
                {
                    //if(digit.Contains(currentNumber))
                    //{
                    //    currentNumber = currentNumber + message;
                    //}
                    if (currentNumber == "") currentNumber = previousNumber + message;
                    else currentNumber = currentNumber + message;
                }
                else if(digit.Contains(message))
                {
                    currentNumber = currentNumber + message;
                }
                displayMessage(currentNumber);
            }
            else
            {
                if(digit.Contains(message))
                {
                    ProccesAccumulateDigitsDecimal(message, true);
                }
                else if(operations.Contains(message))
                {
                    ProccesComputePending(message, true);
                }
                else if (equals.Contains(message))
                {
                    ProccesCompute(message, true);
                }
                else if (specialOperations.Contains(message))
                {
                    ProccesComputeSpecialOperation(message, true);
                }
            }
        }

        void ProccesComputePending(string message, bool income)
        {
            if (income)
            {
                if(previousNumber == "")
                {
                    currentState = State.ComputePending;
                    previousNumber = currentNumber;
                    currentNumber = "";
                    currentOperation = message;
                }
                else
                {
                    ProccesCompute(message, true);
                    currentState = State.ComputePending;
                    currentOperation = message;
                }
            } 
            else
            {
                if (digit.Contains(message))
                {
                    ProccesAccumulateDigits(message, true);
                }
                else if (operations.Contains(message))
                {
                    currentOperation = message;
                }
                else if (specialOperations.Contains(message))
                {
                    ProccesComputeSpecialOperation(message, true);
                }
            }
        }

        void ProccesCompute(string message, bool income)
        {
            if (income)
            {
                if(previousNumber != "" && currentNumber != "")
                {
                    currentState = State.Compute;
                    double a = double.Parse(previousNumber);
                    double b = double.Parse(currentNumber);
                    if (currentOperation == "+")
                    {
                        currentNumber = (a + b).ToString();
                    }
                    else if (currentOperation == "-")
                    {
                        currentNumber = (a - b).ToString();
                    }
                    else if (currentOperation == "*")
                    {
                        currentNumber = (a * b).ToString();
                    }
                    else if (currentOperation == "/")
                    {
                        currentNumber = (a / b).ToString();
                    }
                    previousNumber = currentNumber;
                    displayMessage(currentNumber);
                    currentNumber = "";
                }
                else if(currentNumber != "")
                {
                    previousNumber = currentNumber;
                    displayMessage(currentNumber);
                    currentNumber = "";
                }
            }
            else
            {
                currentOperation = "";
                if (nonZeroDigit.Contains(message))
                {
                    currentNumber = "0";
                    previousNumber = "";
                    ProccesAccumulateDigits(message, true);
                }
                else if(zero.Contains(message))
                {
                    ProccesZeroState(message, true);
                }
                else if (operations.Contains(message))
                {
                    ProccesComputePending(message, true);
                }
                else if(separator.Contains(message))
                {
                    ProccesAccumulateDigitsDecimal(message, true);
                }
            }
        }
    }
}
