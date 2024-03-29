# MarketUITest
## Руководство для запуска тестов  
### Установите и запустите WinAppDriver  

1. Загрузите установщик драйвера приложения Windows со страницы https://github.com/Microsoft/WinAppDriver/releases.
2. Установите драйвер
3. Включите [режим разработчика](https://docs.microsoft.com/en-us/windows/apps/get-started/enable-your-device-for-development) в настройках Windows 
4. Запустить `WinAppDriver.exe` из установочного каталога (например `C:\Program Files (x86)\Windows Application Driver`)
5. Переключить язык раскладки на английский (для правильной вставки)  

Будет поднят драйвер с IP адресом и портом `127.0.0.1:4723`.  
Затем следует скомпилировать приложение, для которого написаны тесты, в данном случае наше приложение [здесь](https://github.com/SaveliyShur/Market). Выкачайте код и поменяете
ссылку в коде тестов, чтобы тесты правильно наши и запустили скомпилированное приложение: 
```
public class UITests
    {
        private const string WindowsAppDriverUrl = "http://127.0.0.1:4723";
        private const string AppPath = @"D:\С#\MyWork\bin\Debug\MarketWorkBD.exe";
```
Следует изменить ссылку `AppPath`.  

6. Запуск тестов в Visual Studio -> Тест -> Запуск всех тестов

### Общение с сервером
В нашем приложении для которого написаны тесты присутствует TCP сервер, который может исполнять команды зашитые в него определенным образом, он поднимается по адресу 
IP адресом и портом `127.0.0.1:8888` и работает в отдельном потоке от основного UI приложения. Что позволяет нам напрямую удалять продукты, добавлять, проверять и прочее.
Используем его для создания контекста теста или очистки тестовых данных после его прохождения, при правильном запросе он вернет `True` в ответ на запрос при вызове метода 
`ReceiveMessage()`. Время ожидания ответа можем настраивать при создании объекта сервера `APIServer(int del)`.

Возможные методы описаны в том же классе, и содержат подробную документацию. В реальных проектах, возможно, рекомендуется делать иначе, данный проект был сделан в учебных целях.

### Немного о тестах  
Написано 3 теста, на вставку, удаление и изменения элемента при помощи графического интерфейса. В тесте на вставку, очищение данных после теста происходит при помощи запроса к серверу приложения. Все тесты написаны без использования паттерна `Page Object`, для быстрой читаемости и понимания принципа работы `WinAppDriver`.


-------
##### P.S.  
Если вы заметили какую-то ошибку напишите issue, если знаете как исправить откройте pull request.
Если данные тесты помогли вам просьба оценить проект, поставив Star.
