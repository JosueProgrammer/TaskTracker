using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTracker
{
    public class Tareas
    {
        private int  Id { get; set; }

        //Propiedad de de acesso 
        public int _ID
        {
            get => Id;
            set => Id = value;
        }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("statusEnum")]
        public Status? StatusEnum { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

        // Constructor de Tareas, asignando ID usando GeneradorID
        public Tareas()
        {
            
        }


        public void GetInfo()
        {
            Console.WriteLine($"ID: {_ID}");
            Console.WriteLine($"Descripción: {Description ?? "Sin descripción"}");
            Console.WriteLine($"Estado: {StatusEnum}");
            Console.WriteLine($"Creado el: {createdAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "No especificado"}");
            Console.WriteLine($"Actualizado el: {updatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "No especificado"}");
            Console.WriteLine("");
        }

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        todo,
        in_progress,
        done
    }
}
