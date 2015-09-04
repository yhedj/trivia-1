module Trivia

open System.Linq

type Player = {
    Name: string
    Position: int
    Purses: int
    InPenaltyBox: bool
}

type GameState = 
    | NotStarted
    | Playing of CurrentTurn
and CurrentTurn = {
    CurrentPlayer: Player
    NextPlayers: Player seq
    Categories: Category seq
}
and Category = {
    Name: string
    Questions: Question seq
}
and Question = string
    
let askQuestion player currentTurn =
    let categoryIndex = player.Position % currentTurn.Categories.Count()
    let category = Seq.nth categoryIndex currentTurn.Categories
    printfn "The category is %s" category.Name;
    printfn "%s" (category.Questions.First())
    let categoriesWithRemainingQuestions = 
        Seq.concat [
            Seq.take categoryIndex currentTurn.Categories
            Seq.singleton { category with Questions = category.Questions.Skip 1 }
            Seq.skip (categoryIndex + 1) currentTurn.Categories]
    { currentTurn with CurrentPlayer = player; Categories = categoriesWithRemainingQuestions }

let nextPlayer currentTurn =
    match Seq.toList currentTurn.NextPlayers with
    | nextPlayer::tail -> 
        Playing { currentTurn with CurrentPlayer = nextPlayer; NextPlayers = Seq.concat [tail;[currentTurn.CurrentPlayer]] }
    | [] -> failwith "Are you really playing alone ?!"

let addOnePurse currentTurn =
    let currentPlayer = { currentTurn.CurrentPlayer with Purses = currentTurn.CurrentPlayer.Purses + 1 }
    let currentTurn = { currentTurn with CurrentPlayer = currentPlayer }
    printfn "%s now has %i Gold Coins." currentPlayer.Name currentPlayer.Purses
    currentTurn