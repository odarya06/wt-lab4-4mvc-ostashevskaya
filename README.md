# WT-4. ASP.NET Core MVC — «Складской учёт»

Учебное веб-приложение для управления складскими позициями, разработанное в рамках
лабораторной работы **WT-4. Разработка веб-приложения на ASP.NET Core MVC и публикация на Render**.

## Репозиторий

```text
https://github.com/odarya06/wt-lab4-4mvc-ostashevskaya
```

## Опубликованное приложение

```text
https://wt-lab4-4mvc-ostashevskaya.onrender.com
```

Сервис развёрнут на Render как **Docker Web Service** на бесплатном тарифе.
Первый запрос после простоя может занять до минуты: free-инстанс засыпает
при отсутствии активности.

## Предметная область

**Складской учёт.** Приложение хранит карточки товаров: наименование, артикул, категорию,
поставщика, количество на складе, цену, описание, дату поступления и признак доступности
к отгрузке. По данным рассчитываются стоимость остатка и текстовый статус позиции.

База данных по условию лабораторной работы не используется: данные хранятся в памяти
приложения (`Data/ProductRepository.cs`) и сбрасываются при перезапуске.

## Возможности

- Главная страница со сводной статистикой по складу и последними поступлениями.
- Каталог товаров с поиском по названию/артикулу/поставщику, фильтрами по категории
  и состоянию остатка.
- Подробная карточка позиции.
- Добавление товара через форму с серверной валидацией.
- Редактирование и удаление позиций.
- Единый адаптивный интерфейс на Bootstrap 5: navbar, cards, badges, таблица, hover-эффекты.
- Обработка отсутствующих страниц и позиций (404).
- Поддержка переменной окружения `PORT` для публикации на Render.

## Соответствие уровням задания

| Блок | Что реализовано |
|------|-----------------|
| 1. MVC-проект | Проект ASP.NET Core MVC 8, собственная главная страница на Bootstrap |
| 2. Model | `Product` с 10 свойствами, 6 объектов в памяти |
| 3. Controller и routing | `ProductController`, `Index`, `Details` с 404, соглашение `{controller}/{action}/{id?}` |
| 4. Razor View | `Index.cshtml`: карточки, таблица, `@foreach`, Bootstrap-каталог |
| 5. GET/POST | `Create` GET/POST, валидация, `RedirectToAction`, сообщения об успехе |
| 6. Доп. функциональность | `Details`, `Edit`, `Delete`, единый визуальный стиль, адаптивная сетка |
| 7. Публикация | Git-репозиторий, `Dockerfile`, `render.yaml`, README, поддержка `PORT` |

## Технологии

- ASP.NET Core 8 MVC, C# 12
- Razor Views, Tag Helpers, Data Annotations
- Bootstrap 5.1, кастомный CSS
- Docker (multi-stage build)
- Render Web Service

## Структура проекта

```text
wt-lab4-4mvc-ostashevskaya/
├── Controllers/
│   ├── HomeController.cs
│   └── ProductController.cs
├── Data/
│   └── ProductRepository.cs
├── Helpers/
│   └── DisplayHelper.cs
├── Infrastructure/
│   └── InvariantDecimalModelBinder.cs
├── Models/
│   ├── HomeViewModel.cs
│   ├── Product.cs
│   └── ErrorViewModel.cs
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   ├── Privacy.cshtml
│   │   └── StatusCode.cshtml
│   ├── Product/
│   │   ├── Index.cshtml
│   │   ├── Details.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── _ProductForm.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       ├── _ValidationScriptsPartial.cshtml
│       └── Error.cshtml
├── wwwroot/
│   ├── css/site.css
│   ├── images/warehouse.svg
│   ├── js/site.js
│   └── lib/...
├── Dockerfile
├── render.yaml
├── Program.cs
├── README.md
└── wt-lab4-4mvc-ostashevskaya.csproj
```

## Маршруты

| Метод | URL | Назначение |
|-------|-----|------------|
| GET | `/` | Главная страница |
| GET | `/Product/Index` | Каталог товаров |
| GET | `/Product/Details/1` | Карточка позиции (404, если нет) |
| GET | `/Product/Create` | Форма добавления |
| POST | `/Product/Create` | Сохранение новой позиции |
| GET | `/Product/Edit/1` | Форма редактирования |
| POST | `/Product/Edit/1` | Сохранение изменений |
| POST | `/Product/Delete/1` | Удаление позиции |

Маршрутизация задаётся в `Program.cs`:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

Цепочка обработки запроса:

```text
URL → Routing → ProductController → Action → Model → Razor View → HTML
```

## Запуск локально

Требуется [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
dotnet restore
dotnet run
```

Приложение откроется по адресу из `Properties/launchSettings.json`
(например, `http://localhost:5000`).

Можно задать порт явно:

```bash
PORT=10000 dotnet run
```

и открыть `http://localhost:10000`.

## Запуск в Docker

```bash
docker build -t wt-lab4-4mvc-ostashevskaya .
docker run --rm -p 10000:10000 -e PORT=10000 wt-lab4-4mvc-ostashevskaya
```

Приложение будет доступно по адресу `http://localhost:10000`.

## Публикация на Render

1. Создайте публичный Git-репозиторий и отправьте проект в ветку `main`.
2. В Render выберите **New → Web Service** и подключите репозиторий.
3. Укажите:
   - **Language / Runtime:** Docker;
   - **Branch:** `main`;
   - **Dockerfile Path:** `./Dockerfile`;
   - **Instance Type:** Free.
4. Render сам передаст переменную `PORT`; приложение слушает `0.0.0.0:$PORT`.
5. Дождитесь окончания Build Logs и Deploy Logs и откройте публичный URL
   `https://wt-lab4-4mvc-ostashevskaya.onrender.com`.

Альтернативно можно создать сервис из Blueprint `render.yaml`.

Render автоматически запускает новый deployment после push в ветку `main`.

### Особенности бесплатного тарифа

Бесплатный Web Service переходит в режим ожидания после периода неактивности.
Первый запрос после паузы может выполняться дольше обычного.

## Хранение данных

`ProductRepository` использует статический `List<Product>` и блокировку для
потокобезопасного доступа. Изменения сохраняются только до перезапуска процесса.
В следующих лабораторных работах хранение планируется перенести в PostgreSQL.

## Материалы отчёта

- `docs/report.pdf` — отчёт по лабораторной работе;
- `docs/screenshots/` — скриншоты локального приложения, Render Dashboard,
  Build Logs, Deploy Logs и опубликованного приложения.

## Примечание

Проект создан как учебный. Имя проекта и пространство имён —
`wt-lab4-4mvc-ostashevskaya`; при необходимости их можно заменить на свои
(например, `wt-lab4-4mvc-ivanov`) командой переименования проекта и обновлением
пространств имён.
