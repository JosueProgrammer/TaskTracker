using TaskTracker;

TaskService taskService = new TaskService();

while (true)
{
    Console.Clear();
    Console.WriteLine(" ==== Menú de Gestión de Tareas ==== ");
    Console.WriteLine("1. Mostrar todas las tareas");
    Console.WriteLine("2. Agregar nueva tarea");
    Console.WriteLine("3. Actualizar tarea");
    Console.WriteLine("4. Eliminar tarea");
    Console.WriteLine("5. Marcar tarea como en progreso o realizada");
    Console.WriteLine("6. Enumerar tareas realizadas.");
    Console.WriteLine("7. Enumerar tareas no realizadas.");
    Console.WriteLine("8. Enumerar tareas en progreso");
    Console.WriteLine("9. Salir");
    Console.Write("Elige una opción: ");

    var opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            taskService.MostrarTareas();
            break;
        case "2":
            Console.WriteLine("Ingrese una descripcion");
            string? description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("La descripcion no puede ser nula o vacia");
                break; 
            }
            taskService.AgregarTarea(description);
            break;
        case "3":
            int idToUpdate = GetTaskIdFromUser();
            if (idToUpdate <= 0) break;

            Console.WriteLine("Ingrese una descripcion");
            string? newDescription = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                Console.WriteLine("La descripcion no puede ser nula o vacia");
                break;
            }
            taskService.ActualizarTarea(idToUpdate, newDescription);
            break;
        case "4":
            int idToDelete = GetTaskIdFromUser();
            if (idToDelete <= 0) break;
            taskService.EliminarTarea(idToDelete);
            break;
        case "5":
            int idToMark = GetTaskIdFromUser();
            if (idToMark <= 0) break;
            taskService.MarcarTarea(idToMark);
            break;
        case "6":
            taskService.EnumerarTareasRealizadas();
            break;
        case "7":
            taskService.EnumerarTareasNoRealizadas();
            break;
        case "8":
            taskService.EnumerarTareasEnCurso();
            break;
        case "9":
            Console.WriteLine("Saliendo del programa...");
            return; // Salir del programa
        default:
            Console.WriteLine("Opción no válida, por favor elige una opción entre 1 y 9.");
            break;
    }

    // Pausar para que el usuario vea el resultado antes de regresar al menú
    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
    Console.ReadKey();
}

int GetTaskIdFromUser()
{
    Console.WriteLine("Digite el Id de la tarea");
    if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
    {
        return id;
    }
    else
    {
        Console.WriteLine("El ID no es válido, debe ser un número mayor a cero.");
        return -1;
    }
}
