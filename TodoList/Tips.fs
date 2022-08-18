module Tips
let help _ = "help"

let unknow_command command = $"未知命令：{command}"

let unimplement_method command = $"未实现的方法{command}"

let unexpect_param_amount expect_amount actually_amount = 
    $"错误：期待{expect_amount}个参数，实际{actually_amount}个参数。"


