open System


let load_item_list_from_file _ = 
    let input = System.IO.File.ReadAllText "todo_list.txt"

    let lines = input.Trim().Split("\r\n")
    lines
    |> Seq.filter (fun line -> line.Length > 0)
    |> Seq.map (fun line -> line.Trim())
    |> Seq.map Item.from_csv_str
    |> Seq.toList
    |> Ok

let save csv_str_lines =
    System.IO.File.WriteAllLines("todo_list.txt", csv_str_lines)
    Ok "保存成功"

let to_list array = 
    if Array.isEmpty array then
        List.empty
    else
        List.ofArray array

let validate_add param = 
    match List.length param with
    | 2 -> 
        Item.default_add param.[0] param.[1] |> Ok
    | _ -> Tips.unexpect_param_amount 2 (List.length param) |> Error

let add item_result = 
    let csv_lines = System.IO.File.ReadAllLines "todo_list.txt"

    let save path string_array = 
        System.IO.File.WriteAllLines(path, string_array)
        Ok "保存成功"

    match item_result with
    | Error e -> Error e
    | Ok item -> 
        let old_csv_list = csv_lines |> List.ofArray
        let new_csv_list = (item |> Item.to_csv_string) :: old_csv_list
        
        new_csv_list
        |> Array.ofList
        |> save "todo_list.txt"
        
        
let validate_list param = 
    match List.length param with
    | 0 ->
        Ok param
    | _ -> Tips.unexpect_param_amount 0 (List.length param) |> Error
    

let list _ = 
    let str_seq = 
        load_item_list_from_file()
        |> Result.defaultValue List.empty
        |> Seq.map Item.display
        |> Seq.fold (fun acc item_display -> acc + item_display + "\r\n") String.Empty

    match String.length str_seq with
    | 0 -> Error "暂无待办事项"
    | _ -> Ok str_seq
    
    
let validate_done list = 
    match List.length list with
    | 1 -> list.[0] |> Ok
    | _ -> Tips.unexpect_param_amount 1 (List.length list) |> Error

let done_todo id_result = 
    
    match id_result with
    |Ok id -> 

        let item_list = 
            load_item_list_from_file()
            |> Result.defaultValue List.Empty

        let old_item = item_list |> Seq.tryFind (fun item -> item.id = id)

        
        match old_item with
        | Some item -> 
            let new_item_list =
                {item with is_done = true}
                ::
                (item_list
                |> Seq.filter (fun item -> item.id <> id)
                |> Seq.toList)

            new_item_list
            |> Seq.toList
            |> Seq.map Item.to_csv_string
            |> Seq.toArray
            |> save
        | None -> $"不能完成id为[{id}]的待办事项，因为该id指定的待办事项不存在。" |> Error
    | Error e -> Error e

let validate_remove list = 
    match List.length list with
    | 1 -> Ok list.[0]
    | _ -> Tips.unexpect_param_amount 1 (List.length list) |> Error
    
let remove id_result = 
    match id_result with
    | Ok id ->
        match Item.load() with
        | Ok item_list ->
            let exist_id = 
                item_list
                |> List.tryFindIndex (fun item -> item.id = id)

            match exist_id with
            | Some id -> 
                item_list
                |> List.removeAt id
                |> List.map Item.to_csv_string
                |> Array.ofList
                |> save
            | None -> $"不能删除id为{id}的待办事项，因为该id指定的待办事项不存在。" |> Error
            
        | Error e -> Error e
    | Error e -> Error e

[<EntryPoint>]
let main argv = 
    let input = to_list argv
    
    let result = 
        match input with
        | [] -> Tips.help() |> Error
        | head::tail -> 
            match head with
            | "add" -> validate_add tail |> add
            | "list" -> 
                validate_list tail |> ignore 
                list ()
            | "done" -> validate_done tail |> done_todo
            | "rm" -> validate_remove tail |> remove
            | _ ->  Tips.unknow_command head |> Error

    match result with
    | Ok t -> printfn "%s" t
    | Error e -> eprintfn "\x1B[31;40m%s\x1B[0m" e

    0

