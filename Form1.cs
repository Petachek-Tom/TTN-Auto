using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Threading;
using System.IO;
using System.Text;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Sockets;
using System.Net;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;

namespace TTN_Auto
{
    public partial class Form1 : Form
    {
        private readonly string dbPath = "TTN.db"; // Путь к файлу базы данных
        private string connectionString; // Строка подключения, значение присваиваем при создании формы

        private CancellationTokenSource _cancellationTokenSource; // Токен отмены для управления задачами

        public Form1()
        {
            InitializeComponent();

            TTN_Table_switcher_1_rb.CheckedChanged += RadioButton_CheckedChanged;
            TTN_Table_switcher_2_rb.CheckedChanged += RadioButton_CheckedChanged;
            TTN_Table_switcher_3_rb.CheckedChanged += RadioButton_CheckedChanged;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Указываем путь к файлу лога
            string logFilePath = "log.txt";

            // Перенаправляем Console.Out на наш класс
            Console.SetOut(new ConsoleToFileLogger(logFilePath));

            Console.WriteLine($"Нашли файл лога.");

            Console.WriteLine($"Стартуем!");

            STATUS_label.Text = "Стартуем.";

            Console.WriteLine($"Пробуем получить FSRAR ID.");
            try
            {
                // Получаем FSRAR ID
                STATUS_label.Text = "Получаем FSRAR ID.";
                string ownerId = await FetchOwnerIdAsync("http://localhost:8080/api/info/list");
                FSRARID_label.Text = ownerId;
                STATUS_label.Text = "FSRAR ID получен!";
                Console.WriteLine($"Получили FSRAR ID - {ownerId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось получить FSRAR ID. {ex.Message}");
                MessageBox.Show($"Не удалось получить FSRAR ID. {ex.Message}");
                STATUS_label.Text = "Ошибка! Не готов к работе.";
            }

            Console.WriteLine($"Подключаемся к БД или создаем, если не существует.");
            try
            {
                // Создаем БД и таблицу
                Console.WriteLine($"Создаем БД.");
                STATUS_label.Text = "Создаем БД.";
                connectionString = $"Data Source={dbPath};Version=3;";
                await Task.Run(() => CreateDatabaseAndTable(connectionString));
                STATUS_label.Text = "БД создана.";
                Console.WriteLine($"БД найдена - {connectionString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось подключиться к БД, {ex.Message}");
                MessageBox.Show($"Не удалось подключиться к БД. {ex.Message}");
                STATUS_label.Text = "Ошибка! Не готов к работе.";
            }

            // Добавляем тестовую запись в таблицу
            //await Task.Run(() => AddTTN(connectionString, "TTN-TEST", DateTime.Now.ToString("yyyy-MM-dd"), "Unaccepted"));
            //STATUS_label.Text = "Тестовая запись добавлена.";

            // Загружаем данные в DataGridView
            Console.WriteLine($"Грузим данные из БД в таблицу.");
            await LoadDataIntoDataGridViewAsync();
            LoadFilteredData("All");
            CustomizeDataGridView();
            STATUS_label.Text = "Готов к работе!";
            Console.WriteLine($"Готовы к работе!");
        } // загрузка формы

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // Определяем, какой RadioButton выбран
            if (TTN_Table_switcher_1_rb.Checked)
            {
                LoadFilteredData("All"); // Все записи
            }
            else if (TTN_Table_switcher_2_rb.Checked)
            {
                LoadFilteredData("Unaccepted"); // Только "Unaccepted"
            }
            else if (TTN_Table_switcher_3_rb.Checked)
            {
                LoadFilteredData("Accepted"); // Только "Accepted"
            }
        } // RadioButton для фильтра

        private void LoadFilteredData(string filter)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Формируем SQL-запрос в зависимости от фильтра
                    string query;
                    if (filter == "All")
                    {
                        query = "SELECT * FROM TTN;";
                    }
                    else
                    {
                        query = "SELECT * FROM TTN WHERE Status = @Status;";
                    }

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        if (filter != "All")
                        {
                            command.Parameters.AddWithValue("@Status", filter);
                        }

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Преобразуем значения статуса
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string status = row["Status"].ToString();
                                row["Status"] = ConvertStatus(status);
                            }

                            // Привязываем DataTable к DataGridView
                            TTN_data_grid.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // Фильтруем данные

        private void EXIT_button_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Выходим. До новых встреч!");
            Application.Exit();
        } // Кнопка "Выход"

        private async void FSRARID_get_button_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Получаем FSRAR ID по кнопке.");
            try
            {
                Console.WriteLine($"Посылаем запрос.");
                string ownerId = await FetchOwnerIdAsync("http://localhost:8080/api/info/list");
                FSRARID_label.Text = ownerId;
                Console.WriteLine($"Получили FSRAR ID!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось получить FSRAR ID - {ex.Message}");
                if (ex.Message == "Произошла ошибка при отправке запроса.")
                {
                    MessageBox.Show($"Не удалось получить FSRAR ID. Вставьте ключ и запустите УТМ.");
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}");
                }
            }
        } // Кнопка "Получить FSRAR ID"

        private async Task<string> FetchOwnerIdAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse, options);

                    if (string.IsNullOrEmpty(apiResponse.OwnerId))
                    {
                        throw new Exception("Поле ownerId пустое или отсутствует. Перезапустите УТМ и повторите попытку.");
                    }

                    GOST_key_date_lable.Text = apiResponse.Gost.ExpireDate;
                    return apiResponse.OwnerId;
                }
                else
                {
                    throw new Exception($"Ошибка при запросе: {response.StatusCode}");
                }
            }
        } // Отправляем запрос к ЕГАИС, получаем ответ с инфой о ключе, используем

        private static void CreateDatabaseAndTable(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS TTN (
                Number INTEGER PRIMARY KEY AUTOINCREMENT,
                TTN_ID TEXT NOT NULL UNIQUE, -- Добавлено UNIQUE, чтоб не дублировались ТТН
                Date TEXT NOT NULL,
                Status TEXT NOT NULL
            );";
                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        } // Создаем БД и таблицу (если не существуют)

        private static void AddTTN(string connectionString, string ttnId, string date, string status)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = @"
            INSERT INTO TTN (TTN_ID, Date, Status)
            VALUES (@TTN_ID, @Date, @Status);";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@TTN_ID", ttnId);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Status", status);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        // Если возникает ошибка из-за UNIQUE constraint
                        if (ex.ErrorCode == 19) // SQLite error code for UNIQUE constraint violation
                        {
                            Console.WriteLine($"Запись с TTN_ID = {ttnId} уже существует.");
                        }
                        else
                        {
                            throw; // Перебрасываем исключение, если это другая ошибка
                        }
                    }
                }
            }
        } // добавляем запись в БД

        private async Task LoadDataIntoDataGridViewAsync()
        {
            try
            {
                DataTable dataTable = await Task.Run(() =>
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT * FROM TTN;";
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                });

                TTN_data_grid.DataSource = dataTable;
                CustomizeDataGridView();

                foreach (DataRow row in dataTable.Rows)
                {
                    row["Status"] = ConvertStatus(row["Status"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // грузим записи из БД в таблицу

        private string ConvertStatus(string status) // меняем в таблице нечитаемые статусы на читаемые
        {
            switch (status)
            {
                case "Unaccepted":
                    return "Не принята";
                case "Accepted":
                    return "Принята";
                default:
                    return status; // Возвращаем оригинальное значение, если оно неизвестно
            }
        }

        private void CustomizeDataGridView()
        {
            // Запрещаем редактирование данных
            TTN_data_grid.ReadOnly = true;

            // Отключаем возможность добавления новых строк
            TTN_data_grid.AllowUserToAddRows = false;

            // Убираем первый пустой столбец (заголовки строк)
            TTN_data_grid.RowHeadersVisible = false;

            // Запрещаем сортировку по столбцам (если не нужно)
            foreach (DataGridViewColumn column in TTN_data_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Resizable = DataGridViewTriState.False; // Запрещаем изменение ширины столбца
            }

            // Скрываем столбец Number (если он не нужен)
            TTN_data_grid.Columns["Number"].Visible = false;

            // Изменяем заголовки столбцов
            TTN_data_grid.Columns["TTN_ID"].HeaderText = "Идентификатор ТТН";
            TTN_data_grid.Columns["Date"].HeaderText = "Дата";
            TTN_data_grid.Columns["Status"].HeaderText = "Статус";

            // Устанавливаем фиксированную ширину таблицы
            TTN_data_grid.Width = 337; // Ширина таблицы 337 пикселей

            // Равномерное распределение ширины столбцов с учетом весов
            TTN_data_grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Настройка FillWeight для каждого столбца
            TTN_data_grid.Columns["TTN_ID"].FillWeight = 40; // 40% ширины
            TTN_data_grid.Columns["Date"].FillWeight = 30;   // 30% ширины
            TTN_data_grid.Columns["Status"].FillWeight = 30; // 30% ширины

            // Стилизация заголовков столбцов
            TTN_data_grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94); // Темно-синий фон
            TTN_data_grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Белый текст
            TTN_data_grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold); // Жирный шрифт
            TTN_data_grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Выравнивание по центру
            TTN_data_grid.EnableHeadersVisualStyles = false; // Включение кастомизации заголовков

            // Стилизация строк
            TTN_data_grid.RowsDefaultCellStyle.BackColor = Color.White; // Цвет фона четных строк
            TTN_data_grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 238, 238); // Цвет фона нечетных строк
            TTN_data_grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219); // Цвет выделения строки
            TTN_data_grid.DefaultCellStyle.SelectionForeColor = Color.White; // Цвет текста при выделении
            TTN_data_grid.GridColor = Color.LightGray; // Цвет сетки

            // Убираем границы ячеек
            TTN_data_grid.CellBorderStyle = DataGridViewCellBorderStyle.None;

            // Убираем рамку вокруг таблицы
            TTN_data_grid.BorderStyle = BorderStyle.None;

            // Настройка высоты строк
            TTN_data_grid.RowTemplate.Height = 30; // Высота строк

            //=============
        } // настраиваем внешний вид таблицы

        private async Task AutoTTN(CancellationToken cancellationToken)
        {
            Console.WriteLine("Запустили AutoTTN.");
            try
            {
                // Сразу вызываем AnswerTTN при старте
                AnswerTTN();

                // Запускаем бесконечный цикл
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Таймер для вызова AnswerTTN каждые 12 часов
                    DateTime nextAnswerTTN = DateTime.Now.AddHours(12);
                    while (DateTime.Now < nextAnswerTTN && !cancellationToken.IsCancellationRequested)
                    {
                        // Вызываем SearchTTN каждые 10 минут и подтверждаем накладные каждые 10 минут, меняем статусы подтвержденных в БД
                        await Task.Delay(TimeSpan.FromMinutes(10), cancellationToken);
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            await SearchTTNAsync();
                            await CheckAndDisplayUnacceptedTTNs();
                            //await ProcessFileAccepted();
                        }
                    }

                    // После 12 часов вызываем AnswerTTN
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        AnswerTTN();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Обработка отмены задачи
                Console.WriteLine("Остановили автоматику.");
                MessageBox.Show("Процесс остановлен.");
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }  // Метод AutoTTN, который управляет вызовом других методов

        private void AnswerTTN()
        {
            Console.WriteLine($"AnswerTTN запущен.");
            Console.WriteLine($"Готовимся сформировать файл запроса.");
            CreateXML("QUESTION_SEND"); // Формируем и отправляем файл
            Console.WriteLine("AnswerTTN выполнен.");
        } // Запрос необработанных

        private async Task SearchTTNAsync()
        {
            // Ваша логика для SearchTTN
            Console.WriteLine("SearchTTN выполнен.");

            await ProcessFilesAsync(); // Запускаем метод поиска ответа на запрос необработанных


        } // Ищем ответы

        private async Task ProcessFilesAsync() // Ответ на запрос необработанных - ищем подходящий файл
        {
            string folderPath = @"C:\UTM\transporter\xml\ws";
            Console.WriteLine($"Ищем ответы в {folderPath}");

            // Поиск файлов асинхронно
            Console.WriteLine($"Ищем файлы.");
            string[] files = await Task.Run(() =>
                Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories)
                    .Where(file => File.ReadAllText(file).Contains("ReplyNoAnswerTTN"))
                    .ToArray()
            );

            if (files.Length == 0)
            {
                Console.WriteLine("Файлы ответа на запрос необработанных пока не найдены.");
                return;
            }

            foreach (string file in files)
            {
                Console.WriteLine($"Нашли файлы, запускаем обработку.");
                await ProcessFileAsync(file); // Запускаем обработку найденных файлов
            }
        }

        private async Task ProcessFileAsync(string filePath) // Ответ на запрос необработанных - разбираем подходящие файлы
        {
            try
            {
                // Загрузка XML-документа асинхронно
                XDocument xmlDoc = await Task.Run(() => XDocument.Load(filePath));

                // Пространства имен из XML
                XNamespace ns = "http://fsrar.ru/WEGAIS/WB_DOC_SINGLE_01";
                XNamespace ttn = "http://fsrar.ru/WEGAIS/ReplyNoAnswerTTN";

                // Поиск всех элементов NoAnswer
                var noAnswerElements = xmlDoc.Descendants(ttn + "NoAnswer");

                foreach (var noAnswer in noAnswerElements)
                {
                    // Извлечение значений WbRegID и ttnDate
                    string wbRegID = noAnswer.Element(ttn + "WbRegID")?.Value;
                    string ttnDate = noAnswer.Element(ttn + "ttnDate")?.Value;

                    // Вносим результат в БД
                    await Task.Run(() => AddTTN(connectionString, wbRegID, ttnDate, "Unaccepted")); // добавляем запись в базу
                    await Task.Run(() => LoadDataIntoDataGridViewAsync()); // Обновляем таблицу
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке файла {filePath}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void TTN_accepting_start_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Нажали на кнопку запуска автоматики.");
            TTN_accepting_start.Enabled = false;
            TTN_accepting_stop.Enabled = true;

            if (_cancellationTokenSource != null)
            {
                MessageBox.Show("Процесс уже запущен.");
                return;
            }

            // Создаем новый токен отмены
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Запускаем метод AutoTTN
                await AutoTTN(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                // Обработка отмены
                MessageBox.Show("Процесс остановлен.");
            }
            finally
            {
                // Очищаем токен после завершения
                _cancellationTokenSource = null;
                TTN_accepting_start.Enabled = true;
                TTN_accepting_stop.Enabled = false;
            }
        } // Кнопка "Начать"

        private void TTN_accepting_stop_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                TTN_accepting_start.Enabled = true;
                TTN_accepting_stop.Enabled = false;
                // Отменяем задачу
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
                MessageBox.Show("Процесс остановлен.");
            }
            else
            {
                MessageBox.Show("Нет запущенных процессов.");
            }
        } // Кнопка "Остановить"

        private void CreateXML(string What, string TTN_ID = "TTN-0") // Создаем XML для подтверждения и запроса необработанных
        {

            DateTime Today = new DateTime();
            Today = DateTime.Now;
            string today_formatted = Today.ToString("yyyy-MM-dd");
            Random rnd = new Random();
            int TTN_Number_for_send_int = rnd.Next(1, 100);

            // Пространства имен
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace ns = "http://fsrar.ru/WEGAIS/WB_DOC_SINGLE_01";
            XNamespace wa = "http://fsrar.ru/WEGAIS/ActTTNSingle_v4";
            XNamespace ce = "http://fsrar.ru/WEGAIS/CommonV3";
            XNamespace qp = "http://fsrar.ru/WEGAIS/QueryParameters";


            if (What == "TTN_Accept")  // Создаем файл для подтверждения
            {
                string TTN_Number_for_send = TTN_Number_for_send_int.ToString();
                //=================================================================================================
                XDocument xdoc = new XDocument();

                XElement documents = new XElement(ns + "Documents",
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "ns", ns.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "wa", wa.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "ce", ce.NamespaceName),
                    new XElement(ns + "Owner",
                        new XElement(ns + "FSRAR_ID", FSRARID_label.Text)
                    ),
                    new XElement(ns + "Document",
                        new XElement(ns + "WayBillAct_v4",
                            new XElement(wa + "Header",
                                new XElement(wa + "IsAccept", "Accepted"),
                                new XElement(wa + "ACTNUMBER", TTN_Number_for_send),
                                new XElement(wa + "ActDate", today_formatted),
                                new XElement(wa + "WBRegId", TTN_ID),
                                new XElement(wa + "Note", "Ok!")
                                ),
                            new XElement(wa + "Content", "\n")
                            )
                        )
                );

                xdoc.Add(documents);
                //сохраняем документ

                xdoc.Save("pod.xml");
            } // Создаем файл подтверждения - 100%

            if (What == "QUESTION_SEND") // Создаем файл запроса
            {
                Console.WriteLine($"Создаем файл запроса.");
                XDocument xdoc = new XDocument();

                XElement documents = new XElement(ns + "Documents",
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "ns", ns.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "qp", qp.NamespaceName),
                    new XElement(ns + "Owner",
                        new XElement(ns + "FSRAR_ID", FSRARID_label.Text)
                    ),
                    new XElement(ns + "Document",
                        new XElement(ns + "QueryNATTN",
                            new XElement(qp + "Parameters",
                                new XElement(qp + "Parameter",
                                    new XElement(qp + "Name", "КОД"),
                                    new XElement(qp + "Value", FSRARID_label.Text)
                                            )
                                        )
                                     )
                                )
                );

                xdoc.Add(documents);
                //сохраняем документ

                xdoc.Save("unaccepted.xml");

                Console.WriteLine($"Создали файл запроса.");

                try
                {
                    Console.WriteLine($"Пробуем отправить файл запроса.");
                    MrSender("QUESTION_SEND");
                }
                catch (UriFormatException) // System.
                {
                    Console.WriteLine("Ошибка при отправке запроса.");
                }
            } // Создаем файл запроса - 100%
        }

        private void MrSender(string What) // Отсылаем подтверждение, запрашиваем необработанные
        {
            string sURL_Q = "http://localhost:8080/opt/in/QueryNATTN"; // Адрес для запросов необработанных
            string sURL_P = "http://localhost:8080/opt/in/WayBillAct_v4"; // Адрес для подтверждения

            if (What == "TTN_SEND")
            {
                /// - Этот код отправляет файл "Pod.xml" на указанный URL с помощью POST-запроса. Мы используем HttpClient для отправки запроса и MultipartFormDataContent для добавления файла к содержимому запроса. Код также проверяет статус ответа и выводит сообщение о результате попытки загрузки файла.
                HttpClient client = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                FileStream fileStream = new FileStream("Pod.xml", FileMode.Open);

                {
                    form.Add(new StreamContent(fileStream), "xml_file", "Pod.xml");
                    try
                    {
                        HttpResponseMessage response = client.PostAsync(sURL_P, form).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Файл подтверждения успешно отправлен в УТМ");
                        }
                        else
                        {
                            Console.WriteLine($"Не удалось отправить файл. Причина - " + response.StatusCode);
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"Не удалось отправить файл подтверждения!");
                    }
                }
                File.Delete("Pod.xml");
            } // Отправляем XML с подтверждением - 100%

            if (What == "QUESTION_SEND")
            {
                HttpClient client = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                FileStream fileStream = new FileStream("unaccepted.xml", FileMode.Open);

                {
                    form.Add(new StreamContent(fileStream), "xml_file", "unaccepted.xml");
                    try
                    {
                        HttpResponseMessage response = client.PostAsync(sURL_Q, form).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Запрос необработанных ТТН отправлен на {sURL_Q}.");
                        }
                        else
                        {

                            Console.WriteLine($"Не удалось отправить запрос на {sURL_Q}. Причина - " + response.StatusCode);
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"Не удалось запросить необработанные ТТН - нет связи с УТМ!");
                    }

                }
                File.Delete("unaccepted.xml");
            } // Отправляем запрос необработанных - 100%


        }

        private async Task CheckAndDisplayUnacceptedTTNs()
        {
            try
            {
                // Показываем индикатор загрузки или сообщение
                Console.WriteLine("Проверяем наличие записей.");

                // Выполняем запрос к базе данных в фоновом потоке
                List<string> unacceptedTtnIds = await Task.Run(() => FetchUnacceptedTTNIds());

                // Если записей нет, выводим сообщение
                if (unacceptedTtnIds.Count == 0)
                {
                    Console.WriteLine("Неподтвержденные ТТН не найдены.");
                    return;
                }

                // Выводим найденные TTN_ID на экран
                foreach (string ttnId in unacceptedTtnIds)
                {
                    Console.WriteLine($"Найдена неподтвержденная ТТН - {ttnId}");
                    Console.WriteLine($"Формируем акт.");
                    CreateXML("TTN_Accept", ttnId);
                    MrSender("TTN_SEND");
                }
                Console.WriteLine("Отработали список необработанных.");
                Console.WriteLine("Пробуем обновлять статусы через 5 минут после подтверждения.");
                await RunProcessFileAcceptedWithDelay();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        } // Заказываем список непринятых и циклом инициируем подтверждение

        private List<string> FetchUnacceptedTTNIds()
        {
            List<string> unacceptedTtnIds = new List<string>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // SQL-запрос для поиска записей со статусом "Unaccepted"
                string query = "SELECT TTN_ID FROM TTN WHERE Status = @Status;";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", "Unaccepted");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ttnId = reader["TTN_ID"].ToString();
                            unacceptedTtnIds.Add(ttnId); // Сохраняем в список
                        }
                    }
                }
            }

            return unacceptedTtnIds;
        } // Ищем в БД непринятые и суем в список

        private async Task RunProcessFileAcceptedWithDelay()
        {
            try
            {
                // Выводим сообщение о начале задержки
                Console.WriteLine("Делаем задержку в 5 минут для обновления статуса.");

                // Задержка в 5 минут (5 * 60 * 1000 миллисекунд)
                await Task.Delay(TimeSpan.FromMinutes(5));

                // Выполняем метод ProcessFileAccepted
                Console.WriteLine($"Выполняется ProcessFileAccepted...");
                await ProcessFileAccepted();

                // Обновляем статус после завершения
                Console.WriteLine($"ProcessFileAccepted завершен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        } // запускаем задержку в 5 минут и инициируем смену статуса.

        private async Task ProcessFileAccepted()
        {

            // Укажите путь к папке с файлами
            string folderPath = @"C:\UTM\transporter\xml\ws";

            // Получаем все файлы без расширения
            string[] files = Directory.GetFiles(folderPath, "*.", SearchOption.TopDirectoryOnly);

            // Обрабатываем каждый файл
            foreach (string filePath in files)
            {
                try
                {
                    // Загружаем файл как XML
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);

                    // Добавляем пространства имен
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    nsmgr.AddNamespace("ns", "http://fsrar.ru/WEGAIS/WB_DOC_SINGLE_01");
                    nsmgr.AddNamespace("tc", "http://fsrar.ru/WEGAIS/Ticket");

                    // Проверяем значение тега <tc:DocType>
                    XmlNode docTypeNode = xmlDoc.SelectSingleNode("//ns:Document/ns:Ticket/tc:DocType", nsmgr);
                    if (docTypeNode != null && docTypeNode.InnerText == "WAYBILL")
                    {
                        // Проверяем значение тега <tc:OperationResult>
                        XmlNode operationResultNode = xmlDoc.SelectSingleNode("//ns:Document/ns:Ticket/tc:OperationResult/tc:OperationResult", nsmgr);
                        if (operationResultNode != null && operationResultNode.InnerText == "Accepted")
                        {
                            // Получаем значение тега <tc:RegID>
                            XmlNode regIdNode = xmlDoc.SelectSingleNode("//ns:Document/ns:Ticket/tc:RegID", nsmgr);
                            if (regIdNode != null)
                            {
                                Console.WriteLine($"В файле {Path.GetFileName(filePath)} есть информация о подтверждении накладной: {regIdNode.InnerText}");
                                Console.WriteLine($"Пробуем внести изменения в базу.");
                                await UpdateTTNStatus(regIdNode.InnerText, "Accepted");
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок при чтении или парсинге файла
                    Console.WriteLine($"Ошибка обработки файла {filePath}: {ex.Message}");
                }
            }

            Console.WriteLine("Обработка файлов завершена.");
        } // Ищем квитанцию о том, что наладная подтверждена...

        private async Task<Task> UpdateTTNStatus(string ttnId, string newStatus)
        { // Убрать <Task>, если не работает
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для обновления статуса
                    string updateQuery = @"
                UPDATE TTN
                SET Status = @NewStatus
                WHERE TTN_ID = @TTN_ID;";

                    using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                    {
                        // Добавляем параметры для предотвращения SQL-инъекций
                        command.Parameters.AddWithValue("@NewStatus", newStatus);
                        command.Parameters.AddWithValue("@TTN_ID", ttnId);

                        // Выполняем запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Статус для TTN_ID {ttnId} успешно обновлен на '{newStatus}'.");
                            await Task.Run(() => LoadDataIntoDataGridViewAsync()); // Обновляем таблицу
                        }
                        else
                        {
                            Console.WriteLine($"Запись с TTN_ID {ttnId} не найдена.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            return Task.CompletedTask;
        }  // Обновляем статусы подтвержденных в БД
    }

    // Модель данных для десериализации JSON с инфой о ключе
    public class ApiResponse
    {
        public string Version { get; set; }
        public string Contour { get; set; }
        public string RsaError { get; set; }
        public string CheckInfo { get; set; }
        public string OwnerId { get; set; } // Это поле нас интересует
        public Database Db { get; set; }
        public Certificate Rsa { get; set; }
        public Certificate Gost { get; set; }
        public bool License { get; set; }
    }

    public class Database
    {
        public string CreateDate { get; set; }
        public string OwnerId { get; set; }
    }

    public class Certificate
    {
        public string CertType { get; set; }
        public string StartDate { get; set; }
        public string ExpireDate { get; set; }
        public string IsValid { get; set; }
        public string Issuer { get; set; }
    }

    //=====================================================

    public class ConsoleToFileLogger : TextWriter
    {
        private readonly TextWriter _consoleOut;
        private readonly StreamWriter _fileWriter;
        private readonly string _logFilePath;

        // Переменные для отслеживания повторяющихся сообщений
        private string _lastMessage = null;
        private int _repeatCount = 0;

        public ConsoleToFileLogger(string logFilePath)
        {
            _consoleOut = Console.Out;
            _logFilePath = logFilePath;

            // Очищаем устаревшие записи перед началом работы
            CleanupOldLogs();

            // Создаем StreamWriter для записи в файл
            _fileWriter = new StreamWriter(logFilePath, true, Encoding.UTF8) { AutoFlush = true };
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string value)
        {
            // Пишем в консоль
            _consoleOut.WriteLine(value);

            // Если сообщение совпадает с предыдущим, увеличиваем счетчик
            if (value == _lastMessage)
            {
                _repeatCount++;
            }
            else
            {
                // Если новое сообщение не совпадает с предыдущим, записываем предыдущее
                FlushRepeatedMessage();

                // Начинаем новый цикл
                _lastMessage = value;
                _repeatCount = 1;
            }
        }

        private void FlushRepeatedMessage()
        {
            if (_repeatCount > 0)
            {
                string messageToWrite = _repeatCount > 1
                    ? $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {_lastMessage} ({_repeatCount})"
                    : $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {_lastMessage}";

                _fileWriter.WriteLine(messageToWrite);
            }
        }

        private void CleanupOldLogs()
        {
            if (!File.Exists(_logFilePath))
                return;

            // Читаем все строки из файла
            var lines = File.ReadAllLines(_logFilePath);

            // Фильтруем строки, оставляя только те, которые моложе трех дней
            var recentLogs = lines.Where(line =>
            {
                // Извлекаем временную метку из строки
                if (line.Length >= 21 && DateTime.TryParseExact(
                    line.Substring(1, 19), // Вырезаем подстроку с датой
                    "yyyy-MM-dd HH:mm:ss",
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime logDate))
                {
                    return (DateTime.Now - logDate).TotalDays < 3;
                }
                return false; // Если строка не содержит корректную дату, игнорируем её
            }).ToArray();

            // Перезаписываем файл с оставшимися строками
            File.WriteAllLines(_logFilePath, recentLogs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Записываем последнее сообщение при завершении работы
                FlushRepeatedMessage();
                _fileWriter?.Dispose();
            }
            base.Dispose(disposing);
        }
    } // Пересылаем сообщения в консоль в логи
}