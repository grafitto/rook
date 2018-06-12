# Rook

A very simple programming language I am creating to gain knowledge in programming language design.
Rook is from the word rookie, I am a rookie at programming language design obviously thats why I am learning.

Current:

```javascript
let current := 2018; //This is the current year
let birth := 1993; //This is my birth year
let age := current - birth; //My age

/**
 * This is the base price regardless of the age
 */
let basePrice := 100;

/**
 * Ticket based on my age
 */
let ticket := if (age >= 18) 
                then 
                  basePrice + 300 
                else 
                  basePrice + 100;
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