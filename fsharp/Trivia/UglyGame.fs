
namespace UglyTrivia

open Trivia;
open System;
open System.Collections.Generic;
open System.Linq;
open System.Text;

type Game() as this =

    let mutable state = NotStarted
    let players = List<Player>()
    
    let popQuestions = LinkedList<string>()
    let scienceQuestions = LinkedList<string>()
    let sportsQuestions = LinkedList<string>()
    let rockQuestions = LinkedList<string>()

    let mutable isGettingOutOfPenaltyBox = false;

    do
        for i = 1 to 50 do
            popQuestions.AddLast("Pop Question " + i.ToString()) |> ignore
            scienceQuestions.AddLast("Science Question " + i.ToString()) |> ignore
            sportsQuestions.AddLast("Sports Question " + i.ToString()) |> ignore
            rockQuestions.AddLast("Rock Question " + i.ToString()) |> ignore
    
    member this.isPlayable(): bool =
        this.howManyPlayers() >= 2

    member this.add(playerName: String): bool =
        players.Add({ Name = playerName; Position = 0; Purses = 0; InPenaltyBox = false });

        Console.WriteLine(playerName + " was added");
        Console.WriteLine("They are player number " + players.Count.ToString());
        true

    member this.howManyPlayers(): int =
        players.Count;

    member this.roll(roll: int) =
        let moveAndAskQuestion currentTurn =
            let player = { currentTurn.CurrentPlayer with Position = (currentTurn.CurrentPlayer.Position + roll) % 12 };

            Console.WriteLine(player.Name
                                + "'s new location is "
                                + player.Position.ToString());
            Console.WriteLine("The category is " + this.currentCategory(player.Position));
            this.askQuestion(player.Position);
            { currentTurn with CurrentPlayer = player }
        let rollFunc currentTurn =
            Console.WriteLine(currentTurn.CurrentPlayer.Name + " is the current player");
            Console.WriteLine("They have rolled a " + roll.ToString());

            if currentTurn.CurrentPlayer.InPenaltyBox then
                if roll % 2 <> 0 then
                    isGettingOutOfPenaltyBox <- true;

                    Console.WriteLine(currentTurn.CurrentPlayer.Name + " is getting out of the penalty box");
                    moveAndAskQuestion currentTurn
                else
                    Console.WriteLine(currentTurn.CurrentPlayer.Name + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox <- false;
                    currentTurn
            else
                moveAndAskQuestion currentTurn
        match state with
        | NotStarted -> 
            state <- Playing { CurrentPlayer = players.Item(0) ; NextPlayers = Seq.skip 1 players }
            this.roll(roll)
        | Playing currentTurn -> 
            state <- Playing (rollFunc currentTurn)

    member private this.askQuestion(position: int) =
        if this.currentCategory(position) = "Pop" then
            Console.WriteLine(popQuestions.First.Value);
            popQuestions.RemoveFirst();
            
        if this.currentCategory(position) = "Science" then
            Console.WriteLine(scienceQuestions.First.Value);
            scienceQuestions.RemoveFirst();
        
        if this.currentCategory(position) = "Sports" then
            Console.WriteLine(sportsQuestions.First.Value);
            sportsQuestions.RemoveFirst();

        if this.currentCategory(position) = "Rock" then
            Console.WriteLine(rockQuestions.First.Value);
            rockQuestions.RemoveFirst();


    member private this.currentCategory(position: int): String =

        if (position % 4 = 0) then "Pop";
        elif (position % 4 = 1) then "Science";
        elif (position % 4 = 2) then "Sports";
        else "Rock"

    member this.wasCorrectlyAnswered(): bool =
        match state with
        | NotStarted -> failwith "Impossible"
        | Playing currentTurn ->
            if currentTurn.CurrentPlayer.InPenaltyBox then
                if isGettingOutOfPenaltyBox then
                    Console.WriteLine("Answer was correct!!!!");
                    let currentTurn = addOnePurse currentTurn

                    let winner = this.didPlayerWin(currentTurn);
                    state <- nextPlayer currentTurn

                    winner;
                else
                    state <- nextPlayer currentTurn
                    true;
            else
                Console.WriteLine("Answer was corrent!!!!");
                let currentTurn = addOnePurse currentTurn

                let winner = this.didPlayerWin(currentTurn);
                state <- nextPlayer currentTurn
                
                winner;

    member this.wrongAnswer(): bool=
        match state with
            | NotStarted -> failwith "Impossible"
            | Playing currentTurn ->
                Console.WriteLine("Question was incorrectly answered");
                Console.WriteLine(currentTurn.CurrentPlayer.Name + " was sent to the penalty box");
                let currentTurn = { currentTurn with CurrentPlayer = { currentTurn.CurrentPlayer with InPenaltyBox = true } }
                
                state <- nextPlayer currentTurn
                true;

    member private this.didPlayerWin(currentTurn: CurrentTurn): bool =
        not (currentTurn.CurrentPlayer.Purses = 6);

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
