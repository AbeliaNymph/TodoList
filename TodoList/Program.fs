open System

let help _ = "help"

let unknow_command command = $"未知命令：{command}"

let unimplement_method command = $"未实现的方法{command}"

type Item = 
    {
        id: string
        title: string
        content: string
        is_done: bool
    }

let to_list array = 
    if Array.isEmpty array then
        List.empty
    else
        List.ofArray array

let add list = Ok "add"


[<EntryPoint>]
let main argv = 
    let input = to_list argv
    
    let result = 
        match input with
        | [] -> help() |> Error
        | head::tail -> 
            match head with
            | "add" -> add tail
            | "list" -> failwith (unimplement_method head)
            | "done" -> failwith (unimplement_method head)
            | "rm" -> failwith (unimplement_method head)
            | _ ->  unknow_command head |> Error

    match result with
    | Ok t -> printfn "%A" t
    | Error e -> eprintfn "%A" e
    0