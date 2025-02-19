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
    Console.WriteLine("5. Salir");
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
                return;
            }

            taskService.AgregarTarea(description);
            break;
        case "3":
            Console.WriteLine("Digite el Id de la tarea");
            int id = Convert.ToInt32(Console.ReadLine());

            if (id <= 0)
            {
                Console.WriteLine("El ID no puede ser cero o menor que cero");
                return;
            }

            Console.WriteLine("Ingrese una descripcion");
            string? Description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Description))
            {
                Console.WriteLine("La descripcion no puede ser nula o vacia");
                return;
            }

            taskService.ActualizarTarea(id,Description);
            break;
        case "4":
            Console.WriteLine("Digite el Id de la tarea");
            int id2 = Convert.ToInt32(Console.ReadLine());

            if (id2 <= 0)
            {
                Console.WriteLine("El ID no puede ser cero o menor que cero");
                return;
            }
            taskService.EliminarTarea(id2);
            break;
        case "5":
            Console.WriteLine("Saliendo del programa...");
            return; // Salir del bucle y terminar el programa
        default:
            Console.WriteLine("Opción no válida, por favor elige una opción entre 1 y 5.");
            break;
    }

    // Pausar para que el usuario vea el resultado antes de regresar al menú
    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
    Console.ReadKey();
}