module Trivia

open System

type Player = 
    | InPenaltyBox of Playing
    | OutOfPenaltyBox of Playing
    | Winner of Playing
and Playing =
    {
        Name: string
        Position: int
        Purses: int
    }

type Category = { 
    Name: string
    Position: int
    Questions: Question list
}
and Question = string

type Game = {
    Players: Player list
    Categories: Category list
}

let nextPlayer game player =
    match game.Players with
    | next::tail -> 
        next, { game with Players = List.append tail [player] }
    | [] -> failwith "No players !"

let private displayCurrentPlayer (player:Playing) diceValue =
    Console.WriteLine(player.Name + " is the current player")
    Console.WriteLine("They have rolled a " + diceValue.ToString())

let private category position game =
    (List.filter (fun c -> c.Position = position % game.Categories.Length) game.Categories).Item 0

let private askQuestion category game =
    let question = category.Questions.Item 0
    Console.WriteLine(question);
    let categoryWithoutAskedQuestion = { category with Questions = Seq.skip 1 category.Questions |> Seq.toList }
    let unchangedCategories = List.filter (fun c -> c <> category) game.Categories
    { game with Categories = List.append unchangedCategories [ categoryWithoutAskedQuestion ] }

let private move diceValue game (player:Playing) =
    let newPosition = (player.Position + diceValue) % 12
    Console.WriteLine(player.Name + "'s new location is " + newPosition.ToString());
    let category = category newPosition game
    Console.WriteLine("The category is " + category.Name);
    let game = askQuestion category game
    { player with Position = newPosition }, game

let roll diceValue game player =
    match player with
    | InPenaltyBox playing ->
        displayCurrentPlayer playing diceValue
        if diceValue % 2 = 0 
        then 
            Console.WriteLine(playing.Name + " is not getting out of the penalty box")
            player, game
        else
            Console.WriteLine(playing.Name + " is getting out of the penalty box");
            let player, game = move diceValue game playing
            OutOfPenaltyBox player, game
    | OutOfPenaltyBox playing -> 
        displayCurrentPlayer playing diceValue
        let player, game = move diceValue game playing
        OutOfPenaltyBox player, game

let didNotAnswerCorrectly player =
    match player with
    | OutOfPenaltyBox playing 
    | InPenaltyBox playing ->
        Console.WriteLine("Question was incorrectly answered");
        Console.WriteLine(playing.Name + " was sent to the penalty box");
        InPenaltyBox playing

let answerCorrectly player =
    match player with
    | InPenaltyBox _ -> player
    | OutOfPenaltyBox playing -> 
        Console.WriteLine("Answer was correct!!!!");
        let player = { playing with Purses = playing.Purses + 1 }
        Console.WriteLine(playing.Name
                                + " now has "
                                + player.Purses.ToString()
                                + " Gold Coins.");

        if player.Purses = 6
        then Winner player
        else OutOfPenaltyBox player