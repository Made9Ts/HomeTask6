using System;
using System.IO;
using NLog;

// Задание 2: Пользовательское исключение
public class InvalidEmailException : Exception
{
    public InvalidEmailException(string message) : base(message) { }
}

public class EmailValidator
{
    private string email;
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

    public void SetEmail(string newEmail)
    {
        try
        {
            if (!newEmail.Contains("@"))
            {
                throw new InvalidEmailException("Некорректный адрес электронной почты");
            }
            email = newEmail;
            Console.WriteLine($"Email успешно установлен: {email}");
        }
        catch (InvalidEmailException ex)
        {
            logger.Error(ex, "Ошибка при установке email");
            throw;
        }
    }
}

class Program
{
    // Настройка NLog
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

    static void Main()
    {
        // Задание 1: Обработка исключений при работе с файлами
        ReadFileWithExceptionHandling();

        // Задание 2: Проверка пользовательского исключения
        TestEmailValidation();
    }

    static void ReadFileWithExceptionHandling()
    {
        while (true)
        {
            try
            {
                Console.Write("Введите путь к файлу: ");
                string filePath = Console.ReadLine().Trim('"'); // Удаление кавычек

                // Проверка существования файла перед открытием
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Файл не найден: {filePath}");
                }

                // Чтение и обработка файла
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            // Пример: попытка преобразовать строку в число
                            int number = int.Parse(line);
                            Console.WriteLine($"Прочитано число: {number}");
                        }
                        catch (FormatException)
                        {
                            // Логирование и пропуск некорректной строки
                            logger.Warn($"Некорректный формат строки: {line}");
                            Console.WriteLine($"Пропуск некорректной строки: {line}");
                        }
                    }
                }
                break; // Успешное чтение файла
            }
            catch (FileNotFoundException ex)
            {
                // Логирование ошибки отсутствия файла
                logger.Error(ex, "Файл не найден");
                Console.WriteLine($"Ошибка: Файл не найден. {ex.Message}");
            }
            catch (IOException ex)
            {
                // Логирование других ошибок ввода/вывода
                logger.Error(ex, "Ошибка ввода/вывода");
                Console.WriteLine($"Ошибка ввода/вывода: {ex.Message}");
            }
        }
    }

    static void TestEmailValidation()
    {
        EmailValidator validator = new EmailValidator();

        try
        {
            // Тест с корректным email
            validator.SetEmail("user@example.com");

            // Тест с некорректным email
            validator.SetEmail("invalid-email");
        }
        catch (InvalidEmailException ex)
        {
            Console.WriteLine($"Ошибка email: {ex.Message}");
        }
    }
}