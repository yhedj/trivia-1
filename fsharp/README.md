F# refactoring towards immutability and pure functions
======================================================

I tried to refactor this kata with baby/small steps. I got the idea to focus on immutability (after having built a golden master file...).

My first attempt was really not baby steps-oriented, I first tried to remove state too quickly writing public methods (roll & answers) working on a type. I ended with a big refactor spending several hours without being able check the golden master. Result: I got a quite satisfying implementation, fixing "bugs", but unable to verify the golden master.

So I tried again with small steps, starting creating some type and moving behavior from existing methods to "pure functions" operating on this type.
I had added some cases on types and some pattern matching, that were revealed useless in the end, but it was a good step toward removing mutability iteratively.

As a feedback of these two experiences, I understood that starting with refactoring inner function to pure functions, removing mutability step by step was the key of success. It was in fact quite the same approach than in OO refactoring : let emerge some types, but create pure functions instead of encapsulate things in your types. I don't know if it is a good conclusion from a functional perspective, I feel it like that.

In addition, I can just support you to give a try to functional refactoring to exercise your skills in functional programming. It was was a good exercise for me.
