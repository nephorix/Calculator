# Calculator
Summary:

Overall Time Taken: 4.5 hours Including Testing

Features: I have implemented the solution to handle nested brackets and other conditions that were not tested by the sample questions in the Problem document. 

Eg. - 5 / - 5  = 1 (Division by negative) and 5 ( - 10 ) = -50 (Assuming multiplication if no operator is before or after opening and closing brackets). 4 - - 4 - - 4 = 12 (Converting double ‘-‘ to addition operation).

How to use: 

1) Navigate to Calculator/bin/Release/netcoreapp3.1/publish folder and run terminal command dotnet Calculator.dll (.NET Core 3.1 must be installed beforehand as that is the target framework I selected when creating the console project).

2) You will prompted to enter the parameters you want to compute after the message “Please enter parameters you want to calculate: ”. Please ensure spacing and proper parameter sequence is followed or else the program will display a message “Incorrect sequence of operators and operands detected!”. 

3) Scenarios / limitation that would break the sequence include placing more than two ‘-‘ operator simultaneously or placing more than one ‘+’ , ‘/‘ or ‘*’ simultaneously.

Eg: 
1) “1 - - - 3” won’t work. Use “1 - - ( - 3 )” instead.
2) “1 + - 3” won’t work. Use “1 - 3” or “1 + ( - 3 )” instead.
3) “1 / / 3” or “1 * * 3” are complete sequence error. Use “1 / 3” or “1 * 3”.

