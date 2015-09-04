module Trivia

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
}
    
let nextPlayer currentTurn =
    match Seq.toList currentTurn.NextPlayers with
    | nextPlayer::tail -> 
        Playing { CurrentPlayer = nextPlayer; NextPlayers = Seq.concat [tail;[currentTurn.CurrentPlayer]] }
    | [] -> failwith "Are you really playing alone ?!"

let addOnePurse currentTurn =
    let currentPlayer = { currentTurn.CurrentPlayer with Purses = currentTurn.CurrentPlayer.Purses + 1 }
    let currentTurn = { currentTurn with CurrentPlayer = currentPlayer }
    printfn "%s now has %i Gold Coins." currentPlayer.Name currentPlayer.Purses
    currentTurn