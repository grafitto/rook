# Rook

A very simple programming language I am creating to gain knowledge in programming language design.
Rook is from the word rookie, I am a rookie at programming language design obviously thats why I am learning.

Current:

```javascript
/**
  All numbers are of type Double
  Only 4 data types are supported
   1. Numbers (Doubles)
   2. Strings
   3. Booleans (True|False)
   4. Null (Null)
*/
let current := 2018;
let birth := 1993;
let age := current - birth;

let ticket := {:(age)
  let baseTicket := 100;

  if age >= 18 then baseTicket + 300 else baseTicket + 100;
}

let x := ticket(age);

Print(x);
//This is a comment
//This is also another comment
```

Now that function calls are over, 
Notice that the code above is almost similar to te one below

**Dreams:**

```javascript
let ticketCost := {:(age) 
    if age < 18 then 200 else 500;
};
let age := 24;
print(ticketCost(age));
```
Now its time for implementing the `.` access to allow stuff like:

```javascript
10.Negate; //Should return -10

"Hello".Concat("World"); //Should return "Hello world"

//Etc

```

Then vigorous testing will start...