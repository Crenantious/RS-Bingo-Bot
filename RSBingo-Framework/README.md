Rebuilding EFCore Models
========================

Line to rebuild Models from the DB directly, use the Package Manager Console and set RSBingo-Framework as the startup/default project

scaffold-dbcontext "Server=xxxx;Database=xxx;Uid=xxx;Pwd=xxx" MySql.Data.EntityFrameworkCore -OutputDir Modelsx
or
scaffold-dbcontext "Server=127.0.0.1;Database=xxx;Uid=xxx;Pwd=xxx" Pomelo.EntityFrameworkCore.MySql -OutputDir Modelsx