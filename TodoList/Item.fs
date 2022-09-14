module Item

type Item = 
    {
        id: string
        title: string
        content: string
        is_done: bool
    }

let create id title content is_done = 
    {
        id = id
        title= title
        content = content
        is_done = is_done
    }

let default_add title content = create (Nanoid.Nanoid.Generate()) title content false

let from_csv_str (str: string) = 
    let csv = str.Split ","
    
    create csv.[0] csv.[1] csv.[2] (bool.Parse csv.[3])

let display item =
    if item.is_done then
        $"[x] {item.id} {item.title} {item.content}"
    else
        $"[ ] {item.id} {item.title} {item.content}"

let load _ =
    if System.IO.File.Exists "todo_list.txt" then
        let input = System.IO.File.ReadAllText "todo_list.txt"

        let lines = input.Trim().Split("\r\n")

        lines
        |> Seq.filter (fun line -> line.Length > 0)
        |> Seq.map (fun line -> line.Trim())
        |> Seq.map from_csv_str
        |> Seq.toList
        |> Ok
    else
        Tips.data_file_not_found() |> Error

let to_csv_string item =
    $"{item.id},{item.title},{item.content},{item.is_done}\r\n"

