using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTracker
{
    public class TaskService
    {
        List<Tareas> TasKServiceApp = new List<Tareas>();
        private string? filePath = "task.json";

        // Agregar tareas
        public void AgregarTarea(string description)
        {
            Console.WriteLine(" ==== Agregar una tarea ==== ");

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Error: La descripción de la tarea no puede estar vacía.");
                return;
            }

            try
            {
                // Leer el archivo si existe y deserializarlo
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    TasKServiceApp = JsonSerializer.Deserialize<List<Tareas>>(jsonString) ?? new List<Tareas>();
                }

                int newId = 1;

                if (TasKServiceApp.Count > 0)
                {
                    newId = TasKServiceApp.Max(t => t._ID) + 1;
                }


                var nuevaTarea = new Tareas
                {
                    _ID = newId,
                    Description = description.Trim(),
                    createdAt = DateTime.UtcNow,
                    updatedAt = DateTime.UtcNow,
                    StatusEnum = Status.todo
                };

                // Agregar la nueva tarea al final de la lista
                TasKServiceApp.Add(nuevaTarea);

                // Serializar la lista completa de tareas sin cambiar el orden
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }

                };

                // serializar el json actualizado
                string updatedJson = JsonSerializer.Serialize(TasKServiceApp, options);

                // Guardar todas las tareas en el archivo
                File.WriteAllText(filePath, updatedJson);

                Console.WriteLine("Tarea agregada con éxito.");
                Console.WriteLine($"Total de tareas: {TasKServiceApp.Count}");
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                  "{0}: The write operation could not " +
                  "be performed because the specified " +
                  "part of the file is locked.",
                  ex.GetType().Name);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error en la conversión JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }
        }


        //mostrar tarea
        public void MostrarTareas()
        {
            Console.Clear();
            Console.WriteLine(" ==== Mostrar tareas ==== ");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("El archivo JSON no existe.");
                return;
            }

            try
            {
                // Leer y deserializar el archivo JSON
                var jsonReader = File.ReadAllText(filePath);
                var tareasList = JsonSerializer.Deserialize<List<Tareas>>(jsonReader) ?? new List<Tareas>();

                // Si no hay tareas, mostrar mensaje
                if (!tareasList.Any()) // Más limpio que tareasList.Count == 0
                {
                    Console.WriteLine("No hay tareas para mostrar.");
                    return;
                }

                // Mostrar las tareas
                tareasList.ForEach(t => t.GetInfo());
            }
            catch (Exception ex) // Captura cualquier error inesperado
            {
                Console.WriteLine($"Error al mostrar tareas: {ex.Message}");
            }
        }

        //actualizar tarea
        public void ActualizarTarea(int id, string description)
        {
            Console.Clear();
            Console.WriteLine(" ==== Actualizar una tarea ==== ");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("El archivo JSON no existe.");
                return;
            }

            try
            {
                // Leer y deserializar el archivo JSON
                var jsonString = File.ReadAllText(filePath);
                var tareasList = JsonSerializer.Deserialize<List<Tareas>>(jsonString) ?? new List<Tareas>();

                // Buscar la tarea a actualizar
                var tareaExistente = tareasList.FirstOrDefault(t => t._ID == id);

                if (tareaExistente == null)
                {
                    Console.WriteLine("No se encontró la tarea para actualizar.");
                    return;
                }

                // Actualizar solo los campos necesarios
                tareaExistente.Description = description;
                tareaExistente.updatedAt = DateTime.UtcNow;
                tareaExistente.StatusEnum = Status.in_progress;

                // Serializar la lista completa de tareas sin cambiar el orden
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                // Sobrescribir el archivo con la lista actualizada
                File.WriteAllText(filePath, JsonSerializer.Serialize(tareasList, options));

                Console.WriteLine("Tarea actualizada con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la tarea: {ex.Message}");
            }
        }



        //eliminar tareas
        public void EliminarTarea(int id)
        {
            Console.Clear();
            Console.WriteLine(" ==== Eliminar una tarea ==== ");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("El archivo JSON no existe.");
                return;
            }

            try
            {
                // Leer y deserializar el archivo JSON
                var jsonString = File.ReadAllText(filePath);
                var tareasList = JsonSerializer.Deserialize<List<Tareas>>(jsonString) ?? new List<Tareas>();

                // Intentar eliminar la tarea por ID
                int tareasEliminadas = tareasList.RemoveAll(t => t._ID == id);

                // Serializar la lista completa de tareas sin cambiar el orden
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                if (tareasEliminadas > 0)
                {
                    // Guardar la lista actualizada en el archivo JSON
                    File.WriteAllText(filePath, JsonSerializer.Serialize(tareasList, options));
                    Console.WriteLine($"Tarea con ID {id} eliminada con éxito.");
                }
                else
                {
                    Console.WriteLine("No se encontró ninguna tarea con ese ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la tarea: {ex.Message}");
            }
        }


        // Marcar una tarea como en progreso o realizada
        public void MarcarTarea(int id)
        {
            Console.WriteLine("=== Marcar tarea como en progreso o realizada. ===");


            if (!File.Exists(filePath))
            {
                Console.WriteLine("El archivo JSON no existe.");
                return;
            }

            try
            {
                // Leer y deserializar el archivo JSON
                var jsonString = File.ReadAllText(filePath);
                var tareasList = JsonSerializer.Deserialize<List<Tareas>>(jsonString) ?? new List<Tareas>();

                // Buscar la tarea a actualizar
                var tareaExistente = tareasList.FirstOrDefault(t => t._ID == id);


                if (tareaExistente == null)
                {
                    Console.WriteLine("No se encontró la tarea para actualizar.");
                    return;
                }

                //Lista de tareas disponibles 
                Console.WriteLine("Lista de estados disponibles en la enumeración Status:");
                foreach (var i in Enum.GetValues(typeof(Status)))
                {
                    Console.WriteLine($"{(int)i} - {i}");
                }

                //Digitando el valor para el estado
                Console.WriteLine("Digite un valor para el estado");
                int estado = Convert.ToInt32(Console.ReadLine());

                if (estado < 0 || !Enum.IsDefined(typeof(Status), estado))
                {
                    Console.WriteLine("El estado ingresado no es válido.");
                    return;
                }

                tareaExistente.StatusEnum = (Status)estado;

                // Serializar la lista completa de tareas
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // Sobrescribir el archivo con la lista actualizada
                File.WriteAllText(filePath, JsonSerializer.Serialize(tareasList, options));

                Console.WriteLine("Tarea actualizada con éxito.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        // Enumere todas las tareas que se realizan
        public void EnumerarTareasRealizadas()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("El archivo JSON no existe.");
                return;
            }

            try
            {
                // Leer y deserializar el archivo JSON
                var jsonPath = File.ReadAllText(filePath);
                var tareasList = JsonSerializer.Deserialize<List<Tareas>>(jsonPath) ?? new List<Tareas>();

                // Verificar si la lista de tareas está vacía
                if (!tareasList.Any())
                {
                    Console.WriteLine("No hay tareas para enumerar.");
                    return;
                }

                foreach (var tarea in tareasList)
                {
                    // Filtrar por estado 'todo'
                    if (tarea.StatusEnum == Status.todo)
                    {
                        tarea.GetInfo();
                    }
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al procesar el archivo JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        //Enumere todas las tareas que no se han realizado
        public void EnumerarTareasNoRealizadas()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("El archivo JSON no existe.");
                return;
            }

            try
            {
                // Leer y deserializar el archivo JSON
                var jsonPath = File.ReadAllText(filePath);
                var tareasList = JsonSerializer.Deserialize<List<Tareas>>(jsonPath) ?? new List<Tareas>();

                // Verificar si la lista de tareas está vacía
                if (tareasList.Count == 0)
                {
                    Console.WriteLine("No hay tareas para enumerar.");
                    return;
                }

                foreach (var tarea in tareasList)
                {
                    // Filtrar por estado 'done'
                    if (tarea.StatusEnum == Status.done)
                    {
                        tarea.GetInfo();
                    }
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al procesar el archivo JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Enumere todas las tareas que están en curso
        public void EnumerarTareasEnCurso()
        {

            if (!File.Exists(filePath))
            {
                Console.WriteLine("El archivo JSON no existe.");
                return;
            }

            try
            {
                // Leer el Json y deserializarlo
                var jsonPath = File.ReadAllText(filePath);
                var listTareas = JsonSerializer.Deserialize<List<Tareas>>(jsonPath) ?? new List<Tareas>();

                if (listTareas.Count == 0)
                {
                    Console.WriteLine("No hay tareas para enumerar");
                    return;
                }
                
                foreach(var tareas in listTareas)
                {
                    // Filtrar por estado 'in_progress'
                    if (tareas.StatusEnum == Status.in_progress)
                    {
                        tareas.GetInfo();
                    }
                }
                         

            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al procesar el archivo JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
