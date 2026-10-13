# Toll fee calculator 1.1

## Algorithm
I decided to go with a *divide and conquer* approach to solve the problem. When the problem is divided into sub-problems we get the following:
1. What is the fee for any given time?
2. What is the highest fee during any one hour period?
3. What is the total fee for any given day?
4. What is the total fee for any given `Vehicle` for any given set of `dates`?

When the sub-problems above are solved in order then you have solved the problem.

**Bonus**: The current algorithm also calculates the toll fee for all passes during year 2013. I.e: You can send in a list of passes for a whole week, month and so on.

## Interface
I decided to maintain the current interface, `int GetTollFee(Vehicle vehicle, DateTime[] dates)`, in order to avoid breaking applications that depend on the current implementation. If the interface is changed then all depending applications break.

## Testing
Tests need to be exhaustive to ensure the correctness of the implementation. When tests are thorough then performance and readability can be improved with confidence.

## Correctness, performance and, readability
When I wrte a program I consider the three things. They are, in order of importance:
- **Correctness**: A program must produce the expected result. Otherwise its value is zero.
- **Performance**: Between two correct implementations choose the most performant one. This makes a program more scalable.
- **Readability**: Conventions can be broken in order to improve readability. E.g: `t_0559` is easier to understand and read than `fiveHoursAndFiftyNineMinutes`.

## Comments
I think the algorithm should be improved based on the data that is processed. That is, we should modify the algorithm based on the common case. E.g.: If there are a lot of days with total fees that exceed the maximum fee per day then the algorithm should perform a check every now and then to see if the maximum fee has been exceeded and proceed with the processing of the next day.
