﻿> __Project__
> > 
> > __EmailsForGetInfo__
> > > Поштові адреси на які буде розсилатися листи щодо змін на сайті (додано нову статтю, внесено зміни у існуючу...)
> > 
> > 
> > __Feedback__
> > > Інформація, що відображатиметься на сторінці адміністрації
> > 
> > __MailboxDefaults__
> > > Налаштування поштового клієнту
> > > 
> > > __RealEmail__ і __RealEmailPassword__ варто спробувати задати через environment variables => див. Tools/setenv.sh (не обов'язково і може щось не прпацювати тоді)
> > > 
> > > __AddressEmail__ => TODO: не працює, як потрібно - всеодно відображає реальний адрес (млже на сервері інакше буде)
> > > 
> > > __ServicePort__ => для GMAIL SMTP може приймати 2 значення: 
> > > > 1. `465` => __UseSSL__ = `true`
> > > > 2. `587` => __UseSSL__ = `false`
> > > 
> > > __MailSubjects__ => Текст у заголовках листів (довільний, на Ваш розсуд)
> > 
> > 
> > __Content__
> > > Адміністрування данними і їх можливими властивостями
> > > 
> > > __DataPath__
> > > > Відносні шляхи до директорій зарезервованих (потребує особливої уваги при налаштуванні)
> > > > 
> > > > 1. __ArticleDocsDir__ => місце збереження .doc/.docx статей
> > > > 2. __ArticleHtmlDir__ => місце збереження .htm конвертованих версій статей (можуть містити неточності)
> > > > 3. __ArticleImagesDir__ => місце збереження зображень для статей, що будуть підвантажуватися для .htm версій (обов'язково повинна буди статично папкою, звідки фронт безе усі інші зображення)
> > > > 4. __ConferenceImagesDir__ => місце збереження .doc/.docx статей
> > > 
> > > __UniqueAddress__ 
> > > > Спецільна адреса, що генерується для статей та користувачію автоматично довжиною у вказаному вами діапазоні, а для конференцій є можливість власноруч задати її
> > > > 
> > > > Перевага над *Guid* і *Primary Key*, що *Primary Key* неможливо змінювати, а *Guid* задовгий і виглядає не дуже охайно, тому я створив окреме поле для адрес, яку (поки теоретично) може бути можливіть змінювати
> > > > 
> > > > Приктад унікальної адреси: `aac4131b => /articles?x=aac4131b` || `~/DocsFiles/aac4131b.docs`
> > > 
> > > __Article__ і __Conference__
> > > > Організація обмежень відповідних значень
> > 
> __SelectedConnectionString__
> > Ключ з __ConnectionStrings__ за яким буде проходити підключення до БД
> 
> __ConnectionStrings__
> > Набір доступних строк підключення до БД (є можливість додавання нових і видалення існуючих)
> 
> __Security__
> > Система безпеки і збереження данних
> >
> > __Lockout__
> > Обмеження при системі входу користувача в систему
> > > 1. __LockoutTimeSpan__ => Довжину кулдауну користувача при провалі всіх спроб входу
> > > 2. __MaxFailedAccessAttempts__ => Кількість можливих помилок перед кулдауном  
> > 
> > __Password__
> > Обмеження можливих паролів для користувачів
> > > 1. __RequiredLength__ => Мінімальна довжина
> > > 2. __RequiredUniqueChars__ => Мінімальна кількість унікальних символів в паролі
> > > 3. __RequireNonAlphanumeric__ => @#$
> > > 4. __RequireLowercase__ => abc
> > > 5. __RequireUppercase__ => ABC
> > > 6. __RequireDigit__ => 123
> > > 7. __HasherIterationCount__ => Кількість ітерацій хешування паролю
> > 
> > __User__
> > Налаштування для користувачів
> > > 1. __CookieName__ => Ім'я сесійної кукі на пристрої користувача
> > > 2. __ExpireTimeSpanMinutes__ => Довжина життя сесійної кукі користувача
> > > 3. __SlidingExpiration__ => Можливіть продовження життя сесійної кукі при активності користувача
> > 
> > __SignIn__
> > > Налаштування системи входу в акаунт
> > > 1. __RequireConfirmedAccount__ => Входити у систему можуть лише користувачі з підтвердженою поштою
> 
> __Logging__
> > Рівень логування

