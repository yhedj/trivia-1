module Trivia

open System.Linq

type Player = {
    Name: string
    Position: int
    Purses: int
    InPenaltyBox: bool
}

type GameState = 
    | Playing of CurrentTurn
    | Won of Player
and CurrentTurn = {
    CurrentPlayer: Player
    IsGettingOutOfPenaltyBox: bool // Same bug than initial code : a player that has been in penalty box is considered 
    NextPlayers: Player seq
    Categories: Category seq
}
and Category = {
    Name: string
    Questions: Question seq
}
and Question = string

let generateQuestions category =
    [1..50] |> Seq.map (fun i -> sprintf "%s Question %i" category i)

let addPlayer playerName players =
    let players = List.append players [{ Name = playerName; Position = 0; Purses = 0; InPenaltyBox = false }]
    printfn "%s was added" playerName
    printfn "They are player number %i" players.Length
    players

let askQuestion player currentTurn =
    let categoryIndex = player.Position % currentTurn.Categories.Count()
    let category = Seq.item categoryIndex currentTurn.Categories
    printfn "The category is %s" category.Name;
    printfn "%s" (category.Questions.First())
    let categoriesWithRemainingQuestions = 
        Seq.concat [
            Seq.take categoryIndex currentTurn.Categories
            Seq.singleton { category with Questions = category.Questions.Skip 1 }
            Seq.skip (categoryIndex + 1) currentTurn.Categories]
    { currentTurn with CurrentPlayer = player; Categories = categoriesWithRemainingQuestions }

let moveAndAskQuestion roll currentTurn =
    let player = { currentTurn.CurrentPlayer with Position = (currentTurn.CurrentPlayer.Position + roll) % 12 };
    printfn "%s's new location is %i" player.Name player.Position
    askQuestion player currentTurn
    
let roll diceValue currentTurn =
    printfn "%s is the current player" currentTurn.CurrentPlayer.Name
    printfn "They have rolled a %i" diceValue

    if currentTurn.CurrentPlayer.InPenaltyBox then
        if diceValue % 2 <> 0 then
            printfn "%s is getting out of the penalty box" currentTurn.CurrentPlayer.Name
            { (moveAndAskQuestion diceValue currentTurn) with IsGettingOutOfPenaltyBox = true }
        else
            printfn "%s is not getting out of the penalty box" currentTurn.CurrentPlayer.Name
            { currentTurn with IsGettingOutOfPenaltyBox = false }
    else
        moveAndAskQuestion diceValue currentTurn

let didPlayerWin player =
    player.Purses = 6

let nextPlayer currentTurn =
    if didPlayerWin currentTurn.CurrentPlayer
    then 
        Won currentTurn.CurrentPlayer
    else
        match Seq.toList currentTurn.NextPlayers with
        | nextPlayer::tail -> 
            Playing { currentTurn with CurrentPlayer = nextPlayer; NextPlayers = Seq.concat [tail;[currentTurn.CurrentPlayer]] }
        | [] -> failwith "Are you really playing alone ?!"

let addOnePurse currentTurn =
    let currentPlayer = { currentTurn.CurrentPlayer with Purses = currentTurn.CurrentPlayer.Purses + 1 }
    let currentTurn = { currentTurn with CurrentPlayer = currentPlayer }
    printfn "%s now has %i Gold Coins." currentPlayer.Name currentPlayer.Purses
    currentTurn

let wrongAnswer currentTurn =
    printfn "Question was incorrectly answered"
    printfn "%s was sent to the penalty box" currentTurn.CurrentPlayer.Name
    let currentTurn = { currentTurn with CurrentPlayer = { currentTurn.CurrentPlayer with InPenaltyBox = true } }
    nextPlayer currentTurn

let goodAnswer currentTurn =
    if currentTurn.CurrentPlayer.InPenaltyBox then
        if currentTurn.IsGettingOutOfPenaltyBox then
            printfn "Answer was correct!!!!"
            let currentTurn = addOnePurse currentTurn
            nextPlayer currentTurn
        else
            nextPlayer currentTurn
    else
        printfn "Answer was corrent!!!!"
        let currentTurn = addOnePurse currentTurn
        nextPlayer currentTurn

open System

let rec play (random:Random) game =
    match game with
    | Playing currentTurn -> 
        let randomRoll = random.Next(5) + 1
        let turn = roll randomRoll currentTurn
        if (random.Next(9) = 7) then
            play random (wrongAnswer turn)
        else
            play random (goodAnswer turn)
    | Won player -> ()
    