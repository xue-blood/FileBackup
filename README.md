# FileBackup
FileBackup tool, customing by commandline

# Usage
* Edit `restore.cmd` and `backup.cmd` as you need
    * `--from` the dir you want to copy file from
    * `--to` File will copy to this dir
    * `--include` file name need include patten, **allow multiply**
    * `--exclude` file name must not contain patten, **allow multiply**
    * `--remove` remove patten in file name, **allow multiplay**
    * `--rename` rename file name, usefull for delete empty dir, support character `\-` or string `org:new` replace  **allow multiplay**
    * `--debug` debug flag to show message
    * `--replace` replace the exisit file in target dir

* Drag folder which you want to backup to `backup.cmd`

* Drag folder which you want to restore to `restore.cmd`

#  使用方法
* 编辑 `restore.cmd` 和 `backup.cmd` 两个文件
    * `--from` 程序从该目录复制文件
    * `--to` 程序复制文件的目录路径
    * `--include` 文件名需要包含的内容, **可以指定多个**
    * `--exclude` 文件名不能包含的内容, **可以指定多个**
    * `--remove` 移除指定字符串, **可以指定多个**
    * `--rename` 重命令文件, 支持单个字符 `\-` 和多个字符 `org:new` **可以指定多个**
    * `--debug` 调试开关
    * `--replace` 替换目标路径中己有的文件

* 拖拽要备份的文件夹到 `backup.cmd`

* 拖拽要恢复的文件夹到 `restore.cmd`
