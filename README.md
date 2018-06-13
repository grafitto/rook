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
let string := "This is a test";
let bool := True;
let null := Null;
// This is the base charge allowed
let baseCharge := 100;

let ticket := if age >= 18 then baseCharge + 300 else baseCharge;

/**
  This is testing the inbuilt print function
  For now it only takes one parameter that should
  either be a constant or a variable.
*/
if (ticket == baseCharge) then 
      print("You must be below 18 hehe.")
      else
      print("Welcome sir/madam!");
```

Working on function calls...

Very simple for now hehe...

**Dreams:**

```javascript
let ticketCost := {:(age) 
    if age < 18 then 200 else 500;
};
let age := 24;
print(ticketCost(age));
```

**Still working on it...**