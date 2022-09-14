module Tips
let help _ = "help"

let unknow_command command = $"未知命令：{command}"

let unimplement_method command = $"未实现的方法{command}"

let unexpect_param_amount expect_amount actually_amount = 
    $"错误：期待{expect_amount}个参数，实际{actually_amount}个参数。"

let data_file_not_found _ =
    $"错误：未找到todo_list.txt文件。"

let save_success = "保存成功"

let can_not_finish_not_exist_item id = 
    $"不能完成id为[{id}]的待办事项，因为该id指定的待办事项不存在。"
let can_not_delete_not_exist_item id =
    $"不能删除id为{id}的待办事项，因为该id指定的待办事项不存在。"
