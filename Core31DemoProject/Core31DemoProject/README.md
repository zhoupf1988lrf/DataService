# .net core 基础框架
## EF Code First 生成数据库命令
添加包 Microsoft.EntityFrameworkCore.Tools

1. 生成migration
> Add-Migration InitDefaultDbMigration -c DefaultDbContext -o Migrations/DefaultDb

2. 生成数据库
> Update-Database

## 常用HTTP状态码

| 状态码 | 说明 |
| --- | --- |
| 200 | 请求成功
| 302 | 重定向
| 304 | 使用本地缓存
| 400 | 客户端请求的语法错误，服务器无法理解
| 401 | 请求要求用户的身份认证
| 403 | 用户权限不足
| 404 | 服务器无法根据客户端的请求找到资源（网页）
| 500 | 服务器内部错误，无法完成请求


## Core控制台启动命令
 1 cd 切到运行目录下（或直接在运行目录的资源管理器去cmd）
 2 输入： dotnet aaa.dll --urls=http://0.0.0.0:8527 --ip=127.0.0.1 --port=8527