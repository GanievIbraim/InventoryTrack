using InventoryTrack.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InventoryTrack.Controllers
{
    [ApiController]
    [Route("api/dynamic-table")]
    public class DynamicTableController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DynamicTableController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTable([FromBody] TableRequest request)
        {
            string classCode = GenerateClassCode(request.TableName, request.Fields);
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Entities");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, $"{request.TableName}.cs");
            await System.IO.File.WriteAllTextAsync(filePath, classCode);

            // TO DO: Реализовать миграцию и обновление базы данных
            // самое важное найти способ в runtime скомпилировать и подключиться к базе данных с новой таблице
            // вопрос: как не останавливая приложение создать новые таблицы и работать с ними
            return Ok(new { message = "Таблица и класс успешно созданы", classCode, filePath });
        }

        private string GenerateClassCode(string tableName, List<Field> fields)
        {
            var sb = new StringBuilder();

            sb.AppendLine("namespace InventoryTrack.Entities");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {tableName} : BaseEntity");
            sb.AppendLine("    {");

            foreach (var field in fields)
            {
                string fieldType = GetCSharpType(field.Type);
                sb.AppendLine($"        public {fieldType}? {field.Name} {{ get; set; }}");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GetCSharpType(string fieldType)
        {
            return fieldType switch
            {
                "string" => "string",
                "DateTime" => "DateTime",
                "int" => "int",
                "decimal" => "decimal",
                _ => "string"
            };
        }
    }

    public class TableRequest
    {
        public string TableName { get; set; }
        public List<Field> Fields { get; set; }
    }

    public class Field
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
