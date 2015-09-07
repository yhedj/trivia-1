namespace UglyTrivia

open System

module GameRunner = 
    
    open Trivia

    [<EntryPoint>]
    let main argv = 
        let players = list<Player>.Empty |> addPlayer "Chet" |> addPlayer "Pat" |> addPlayer "Sue"
        let game = Playing { 
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
