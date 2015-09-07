
namespace UglyTrivia

open Trivia;
open System;
open System.Collections.Generic;
open System.Linq;
open System.Text;

type Game(players: Player list) as this =

    let players = players
    
    member this.isPlayable(): bool =
        this.howManyPlayers() >= 2
        
    member this.howManyPlayers(): int =
        players.Length;

    member this.wasCorrectlyAnswered(game) =
        match game with
        | Playing currentTurn ->
            if currentTurn.CurrentPlayer.InPenaltyBox then
                if currentTurn.IsGettingOutOfPenaltyBox then
                    Console.WriteLine("Answer was correct!!!!");
                    let currentTurn = addOnePurse currentTurn
                    nextPlayer currentTurn
                else
                    nextPlayer currentTurn
            else
                Console.WriteLine("Answer was corrent!!!!");
                let currentTurn = addOnePurse currentTurn
                nextPlayer currentTurn
        | Won player -> failwith "%s already won the game !" player.Name

    member this.wrongAnswer(game) =
        match game with
        | Playing currentTurn ->
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(currentTurn.CurrentPlayer.Name + " was sent to the penalty box");
            let currentTurn = { currentTurn with CurrentPlayer = { currentTurn.CurrentPlayer with InPenaltyBox = true } }
            nextPlayer currentTurn
        | Won player -> failwith "%s already won the game !" player.Name

module GameRunner = 
    
    open Trivia

    [<EntryPoint>]
    let main argv = 
        let mutable isFirstRound = true;
        let mutable notAWinner = false;
        let players = list<Player>.Empty |> addPlayer "Chet" |> addPlayer "Pat" |> addPlayer "Sue"
        let aGame = Game(players);
        let mutable game = Playing { 
                CurrentPlayer = players.Item(0)       
                IsGettingOutOfPenaltyBox = false         
                NextPlayers = Seq.skip 1 players
                Categories = [ 
                    { Name = "Pop"; Questions = generateQuestions "Pop" }
                    { Name = "Science"; Questions = generateQuestions "Science" }
                    { Name = "Sports"; Questions = generateQuestions "Sports" }
                    { Name = "Rock"; Questions = generateQuestions "Rock" } ] }
        
        let rand = 
            match Array.toList argv with
            | seed::tail -> new Random(int seed)
            | _ -> new Random()
            
        play rand game            
        0
