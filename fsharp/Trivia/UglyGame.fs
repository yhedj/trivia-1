
namespace UglyTrivia

open Trivia;
open System;
open System.Collections.Generic;
open System.Linq;
open System.Text;

type Game() as this =

    let mutable state = NotStarted
    let players = List<Player>()
    
    let generateQuestions category =
        [1..50] |> Seq.map (fun i -> sprintf "%s Question %i" category i)

    member this.isPlayable(): bool =
        this.howManyPlayers() >= 2

    member this.add(playerName: String): bool =
        players.Add({ Name = playerName; Position = 0; Purses = 0; InPenaltyBox = false });

        Console.WriteLine(playerName + " was added");
        Console.WriteLine("They are player number " + players.Count.ToString());
        true

    member this.howManyPlayers(): int =
        players.Count;

    member this.roll(diceValue: int) =
        match state with
        | NotStarted -> 
            state <- Playing { 
                CurrentPlayer = players.Item(0)       
                IsGettingOutOfPenaltyBox = false         
                NextPlayers = Seq.skip 1 players
                Categories = [ 
                    { Name = "Pop"; Questions = generateQuestions "Pop" }
                    { Name = "Science"; Questions = generateQuestions "Science" }
                    { Name = "Sports"; Questions = generateQuestions "Sports" }
                    { Name = "Rock"; Questions = generateQuestions "Rock" } ] }
            this.roll(diceValue)
        | Playing currentTurn -> 
            state <- Playing (roll diceValue currentTurn)

    member this.wasCorrectlyAnswered(): bool =
        match state with
        | NotStarted -> failwith "Impossible"
        | Playing currentTurn ->
            if currentTurn.CurrentPlayer.InPenaltyBox then
                if currentTurn.IsGettingOutOfPenaltyBox then
                    Console.WriteLine("Answer was correct!!!!");
                    let currentTurn = addOnePurse currentTurn

                    state <- nextPlayer currentTurn

                    not (state = Won currentTurn.CurrentPlayer)
                else
                    state <- nextPlayer currentTurn
                    true;
            else
                Console.WriteLine("Answer was corrent!!!!");
                let currentTurn = addOnePurse currentTurn
                
                state <- nextPlayer currentTurn
                
                not (state = Won currentTurn.CurrentPlayer)

    member this.wrongAnswer(): bool=
        match state with
            | NotStarted -> failwith "Impossible"
            | Playing currentTurn ->
                Console.WriteLine("Question was incorrectly answered");
                Console.WriteLine(currentTurn.CurrentPlayer.Name + " was sent to the penalty box");
                let currentTurn = { currentTurn with CurrentPlayer = { currentTurn.CurrentPlayer with InPenaltyBox = true } }
                
                state <- nextPlayer currentTurn
                true

module GameRunner = 
    
    open Trivia

    [<EntryPoint>]
    let main argv = 
        let mutable isFirstRound = true;
        let mutable notAWinner = false;
        let aGame = Game();

        aGame.add("Chet") |> ignore;
        aGame.add("Pat") |> ignore;
        aGame.add("Sue") |> ignore;
        
        let rand = 
            match Array.toList argv with
            | seed::tail -> new Random(int seed)
            | _ -> new Random()

        while isFirstRound || notAWinner do
            isFirstRound <- false; 
            let randomRoll = rand.Next(5) + 1
            aGame.roll(randomRoll);
            
            if (rand.Next(9) = 7) then
                notAWinner <- aGame.wrongAnswer();
            else
                notAWinner <- aGame.wasCorrectlyAnswered();
            
        0
