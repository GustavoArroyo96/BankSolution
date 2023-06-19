using System.Text.RegularExpressions;
using BankConsole;

if(args.Length == 0){
    EmailService.SendMail();
}else{
        ShowMenu();
    }

void ShowMenu(){
    Console.Clear();
    Console.WriteLine("Selecciona una opción: ");
    Console.WriteLine("1 - Crear un Usuario nuevo.");
    Console.WriteLine("2 - Eliminar un usuario existente.");
    Console.WriteLine("3 -  Salir.");

    int opcion = 0;
    do{
        string input = Console.ReadLine();

        if(!int.TryParse(input, out opcion)){
            Console.WriteLine("Debes ingresar un número (1, 2 o 3).");
        }else if(opcion > 3){
            Console.WriteLine("Debes ingresar un número válido (1, 2 o 3).");
        }
    }while(opcion == 0 || opcion > 3);

    switch(opcion){
        case 1:
            CreateUser();
            break;
        case 2:
            DeleteUser();
            break;
        case 3:
            Environment.Exit(0);
            break;
    }
}

void CreateUser(){
    Console.Clear();
    Console.WriteLine("Ingresa la información del usuario: ");
    int ID; 

    Console.Write("ID: ");
    ID = validarID();

    Console.Write("Nombre: ");
    string name = Console.ReadLine();

    Console.Write("Email: ");
    string email = Console.ReadLine();
    while(!ValidarCorreo(email)){
        Console.Write("!Correo invalido!\t\tIntente de nuevo: ");
        email = Console.ReadLine();
    }

    Console.Write("Saldo: ");
    decimal balance = decimal.Parse(Console.ReadLine());
    while(!validarBalance(balance)){
        Console.Write("!Saldo invalido!\t\tIntente de nuevo: ");
        balance = decimal.Parse(Console.ReadLine());
    }

    Console.Write("Escribe 'c' si el usuarios es Cliente, 'e' si es Empleado: ");
    string userType = Console.ReadLine();
    while(!validarEntrada(userType)){
        Console.Write("!Entrada invalida!\t\tIntente de nuevo: ");
        userType = Console.ReadLine();
    }

    User newUser;

    if(userType.Equals('c')){
        Console.Write("Regimen Fiscal: ");
        char taxRegime = char.Parse(Console.ReadLine());

        newUser = new Client(ID, name, email, balance, taxRegime);
    }else{
        Console.Write("Departamento: ");
        string department = Console.ReadLine();

        newUser = new Employee(ID, name, email, balance, department);
    }

    Storage.AddUser(newUser);

    Console.WriteLine("Usuario creado.");
    Thread.Sleep(2000);
    ShowMenu();
}

void DeleteUser(){

    string input = "";
    bool existeID = false;
    int ID;

    Console.Clear();

    Console.Write("Ingresa el ID del usuario a eliminar: ");
    do
    {
        input = Console.ReadLine();

        if(!int.TryParse(input, out ID)){
            Console.Write("ERROR: Debes ingresar un número.\t\tIntente de nuevo: ");
        }else if(ID <= 0){
            Console.Write("ERROR: Debes ingresar un número entero positivo.\t\tIntente de nuevo: ");
        }else if(!(existeID = Storage.BuscarID(ID))){
            Console.Write("ERROR: No existe usuario con ese ID.\t\tIntente de nuevo: ");
        }
    } while(ID <= 0 || !existeID);
    
    string result = Storage.DeleteUser(ID);

    if (result.Equals("Success")){
        Console.Write("Usuario eliminado.");
        Thread.Sleep(2000);
        ShowMenu();
    }
}

int validarID(){

    int id;
    string input = "";
    bool existeID = false;

    do
    {
        input = Console.ReadLine();

        if(!int.TryParse(input, out id)){
            Console.WriteLine("ERROR: Debes ingresar un número");
            Console.Write("ID: ");
        }else if(id <= 0){
            Console.WriteLine("ERROR: Debes ingresar un número entero positivo.");
            Console.Write("ID: ");
        }else if(existeID = Storage.BuscarID(id)){
            Console.WriteLine("ERROR: Ya existe alguien registrado con ese ID");
            Console.Write("ID: ");
        }
    } while(id <= 0 || existeID);
    
    return id;
}

bool ValidarCorreo(string correo)
{
    string patron = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

    if (Regex.IsMatch(correo, patron))
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(correo);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    else
    {
        return false;
    }
}

bool validarEntrada(string input){

    char opcion;
    bool entradaValida = false;

    if (input.Length == 1 && (input[0] == 'c' || input[0] == 'e'))
    {
        opcion = input[0];
        entradaValida = true;
    }

    return entradaValida;
}

bool validarBalance(decimal balance){

    bool entradaValida = false;

    if(balance > 0){
        entradaValida = true;
    }

    return entradaValida;
}