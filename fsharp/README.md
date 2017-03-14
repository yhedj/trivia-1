# Session #0

## Code smells

* aGame.add : 
    * liste ?
    * mutabilité de Game
* structures en tableau distinctes pour l'état des joueurs (penaltybox, purses, places, players)
* mot clé "mutable" et opérateur "<-"
* boucle while
* Game orienté objet
* Primitive obsession

## Golden master

# Refactoring session #1

* New module where we introduce "pure" functions and types, calling legacy code to keep golden master.
* Remove while -> recursive function instead

# Refactoring session #2

* Extract method (winAPurse, nextPlayer, movePlayer)
* Replace lots of "if" by pattern matching (& modulo)