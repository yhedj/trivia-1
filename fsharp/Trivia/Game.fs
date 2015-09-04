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