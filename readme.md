#### .Net6.0 minimal API + EFCore
- EFCore 一对一、一对多、多对多关系

#### EF Core 命令

- 创建迁移文件
  Add-Migration InitialCreate

- 更新数据库

  Update-Database

- 删除迁移文件

  Remove-Migration

- 删除数据库

  Drop-Database

- 生成数据库脚本

  Script-DbContext

- 生成本次迁移脚本

  Script-Migration

- 重新生成模型

Scaffold-DbContext 'Data Source=F:\Git\EFCoreTest\order.db' Microsoft.EntityFrameworkCore.SqLite -OutputDir Modes1

- 删除数据库
Drop-Database -Confirm
