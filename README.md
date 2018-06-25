# Rook
A few months as I was learning about compiler design, I came up with the project *Rook*.
*Rook* Is a very simple programming language I am making for fun.

Disclaimer:
1. I am not a professional in programming language design. I barely know anything about programming languages.
1. The language is barely tested, therefore its possible that there are tonnes of errors unearthed. For example the error reporter is drunk.
1. Unary operators are not supported **yet.**
1. I know the compiler is written in C#.
## Hello world.
```javascript
Print("Hello world!");
//Print is an inbuilt function used to print to the console.
```
Before we dive deep into Rook, A few points are worth noting.
- **Variable names can only contain alphabets and an underscore (_)** (for now)**.**
- **Only decimal numbers are supported** (for now).
- **Everything in Rook is an object.**
- **EVERY STATEMENT in Rook returns a value. I mean EVERY STATEMENT even an assingment.** An assignment statement returns the currently assigned value

### **Basic Types**
The language has very few basic data types. These are:
1. Numbers.
1. Strings.
1. Booleans.
1. Null.
### Numbers
Numbers are, well numbers. All numbers in Rook are in double precision.
```javascript
let number := 10;
let numberTwo := 0.453;
let numberThree := 45.034;
let another := 34 + 45 / 456 * 2; //Expressions are also supported
// Rook follows JavaScripts Precedence rules
```
Numbers contain a few methods:

**Number Functions**

Since unary operators are not supported yet, Numbers have a `Negate`    property that returns a negated new number.
```javascript
let negatedOne := 10.Negate; //-10
let negatedTwo := negatedOne.Negate; //10

let sqrt := 10.Sqrt; //Square root
let sqr := 10.Sqr; //Square
let exponential := 10.Exp; //Exponential
let absolute := 10.Abs; //Absolute
let cosine := 10.Cos; //Cosine
let sine := 10.Sin; //Sine
let power := 10.Power(2); //10 Power 2 => 100
let log := 10.Log(10); //Log base 10 of 10
```

### Strings
Strings are textual data. `"` is used to surround string data.
`\` can be used to excape characters but special characters E.g `\n` `\t` and others are not supported yet.

```javascript
let string := "Hello world!";
```
Strings contain a few methods:

**Concatnation**

`**+**`   or `Concat` method can be used to concatnate strings.
```javascript
let string := "Hello" + " world!";
//A + can be used to concatnate strings

let stringTwo := string.Concat("again");
let stringThree := "Another".Concat(" string");
//Using Concat method.
```

`ToNumber`  property which converts a string to a Number.

```javascript
let s := "200";
s.ToNumber; // Returns 200 as a number
"200".ToNumber; // is also valid
```

### Booleans
A boolean is a simpe true/false value. 
```javascript
let trueValue := True;
let falseValue := False;
```

Booleans can also be computed. Example:
```javascript
let bool := True || False;
let boolTwo := True && True || False;
//Etc
```

**Note:**
Strings with *"False"* (without quotes), *"Null"* (without quotes) or empty strings are casted to `False` when using them as booleans, everything else is casted to `True`    
```javascript
let bool := "Test" || False; // Returns True
```
Numbers are not implicitly converted to Booleans **yet.** 
But if you wanted to use numbers, you could use numbers as follows:
```javascript
let bool := 0 == 0 || False; // Returns True

let x := 12;
let isGreaterThanTen := x > 10; // This returns False
```

### Null
Null in Rook is an equivalent to nothing. 
```javascript
let x: // Rook assigns Null to x implicitly
let y := Null; // Or you could explicitly assign Null
```


### **Lists**
Lists in Rook are almost equivalent to Python lists. They are initialised with `[]` for empty lists. They can take any of Rook's basic types.
```javascript
let list := []; // Initializes an empty list
let listTwo := ["string", Null, False, 23]; // Can accept any type
```
Lists contains a few methods:

**Add**

Used to add an item at the end of the list
```javascript
let list := [];
list.Add(10); // [10]
list.Add(20); // [10, 20]
list.Add(30); // [10, 20, 30]
list.Add("end"); // [10, 20, 30, "end"]

list[0] := 45; // [45, 20, 30, "end"]

let x := list[3]; // x is now "end"

list.Length; //Returns the number of items on the list
```

### **Functions**
Lets go through functions and function calls before if statements. There is a good reason. Trust me.   
All functions in Rook are higher order functions. They can be passed as parameters and returned as return values.
Functions are denoted by `{` and `}`  so everything between those two curly brackets are considered within the function.
```javascript
let f := {} // This is an empty function
```
All functions return a value. Empty functions return `Null`    
There is no return statement in Rook. The value of the last statement is returned instead.
```javascript
let here := { 10; } //Function here would returh 10
// And since every statement returns a value, even assignments..
let there := {
    let x := 10;
}
// Then the above function would also return 10;
```

**Functions with arguments**

If you are defining a function that expects parameters, you use `:( [params] )` immedietly after `{` 
Example:
```javascript
let Power := {:(num, pow)
    num.Pow(pow);
}
let x := Power(10, 2)
//Value x would be 100, thats 10 power 2
```

Rook supports nested functions, but inner functions are only accessible locally. Its impossible to call a function within another function while outside the parent scope unless the inner function is the return value. *(Still debating whether to use `.` notation to access inner functions globally...)*    
### 
**Function calls**

Function calls are similar to most programming languages
```javascript
FunctionName(); // A function which does not accept any parameters
FunctionName(10, 30); // Calling a function that accepts 2 params
// Etc
```
### 
### **Conditional Statement**
**If - then - **else**** is the only conditional statement in Rook. The else part is optional.
Remember every statement in Rook returns a value? Well, even an `if`statement is not an exception.

```javascript
 //if condition then Expression  | Function call | function else Expression | Function call | Function
 if 10 > 20 then 10 else 20; 
 
 //Brackets on if condition is not a must
 if(10 > 20) then 10 else 20; // This is still valid
 
 if 20 <= 34 then f() else g() // f and g are imaginary functions

 if 10 == 20 then 400; // this evaluates to Null
  
 //Remember if statement returns a value, well...
 let x := if 40 > 20 then 400 else 200 + 34; //x => 300
```
A function definition is also valid after `then` or `else`
```javascript
let n := if True then {
    10;
} else {
    200;
}
// n will evaluate to 10
```
For now, its not possible to use both function definition and an Expression on the same if statement. (Its a bug)
```javascript
let s := if False then 10 else { 200; } // This will fail
let g := if False then 10 else 200; // This is also correct
let f := if False then { 10; } else { 200; } // This is correct 
```

### **Loops**
**Conventional** loop statements like `while` `for` `foreach`are not supported in Rook. However, **Recursion** is fully supported. 

For example, Printing 1 - 10 can be done as follows:
```javascript
let Printer := {:(start, end)
    if start >= end then { 
        Null;
    else {
        Print(start);
        Printer(start + 1, end);
    }
}

//Then call printer
Printer(1, 10);
```
I am debating whether to add **conventional** loops, still undecided :)

Below is are a few simple algorigms that runs on Rook

```javascript
/**
 * This function gets the age and returns the ticket
 */
let CalculateTicket := {:(age)
    let basePrice := 100;
    if age >= 18 then basePrice + 300 else basePrice + 100;
  }

/*
* This function calculates the age
* @params birthYear Number
* @return age Number
*/

let GetAge := {:(birthYear)
  2018 - birthYear;
}

Print("Welcome to IMAX movie theatre.");
Print("We would like a few of you details.");
let s := Read("Enter your birth year: ").ToNumber;
let age := GetAge(s);
let ticket := CalculateTicket(age);

let ans := "Your ticket is: ".Concat(ticket).Concat(". enjoy!");

Print(ans);
// This is a comment
```

```javascript
let Printer := {:(list, index)
  Print(list[index]); 
  if index != list.Length - 1 then Printer(list, index + 1);
}

let Populate := {

  let items := [];
  let s := Read("Enter a number to add to a list(q to quit): ");
  if s == "q" then {
      if items.Length > 0 then {
        Print("Here are the contents of the list:");
        Printer(items, 0);
      } else {
        Print("The list has no items to display");
      }
  } else {
      items.Add(s.ToNumber);
      Populate();
  }
}

Populate();
```

# This is STILL under construction