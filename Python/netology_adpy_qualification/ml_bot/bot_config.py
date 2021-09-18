SEARCH_PARAMETERS = [
    {'topic': 'age_from',
     'question': 'Укажите минимальный возраст кандидатов',
     'answer': 0,
     'answer_type': int},
    {'topic': 'age_to',
     'question': 'Укажите максимальный возраст кандидатов',
     'answer': 99,
     'answer_type': int},
    {'topic': 'music',
     'question': 'Искать совпадения в любимой музыке?',
     'answer': False,
     'answer_type': bool},
    {'topic': 'movies',
     'question': 'Искать совпадения в любимых фильмах?',
     'answer': False,
     'answer_type': bool},
    {'topic': 'interests',
     'question': 'Искать совпадения в интересах?',
     'answer': False,
     'answer_type': bool},
    {'topic': 'books',
     'question': 'Искать совпадения в любимых книгах?',
     'answer': False,
     'answer_type': bool}
]

USER_MISSING_DATA = [
    {'topic': 'age',
     'question': 'Укажите Ваш возраст:',
     'answer': 0,
     'answer_type': int},
    {'topic': 'sex',
     'question': 'Укажите Ваш пол (1-Ж, 2-М):',
     'answer': 0,
     'answer_type': int},
    {'topic': 'city',
     'question': 'Укажите город:',
     'answer': '',
     'answer_type': str},
    {'topic': 'music',
     'question': 'Ваша любимая музыка:',
     'answer': '',
     'answer_type': str},
    {'topic': 'movies',
     'question': 'Ваши любимые фильмы:',
     'answer': '',
     'answer_type': str},
    {'topic': 'interests',
     'question': 'Ваши интересы:',
     'answer': '',
     'answer_type': str},
    {'topic': 'books',
     'question': 'Ваши любимые книги:',
     'answer': '',
     'answer_type': str},
    {'topic': 'start',
     'question': 'Морально к поиску готовы?',
     'answer': '',
     'answer_type': str}
]

BOT_CONFIG = {
    'intents':
        {
            'hello':
                {'examples': ['старт', 'Хэй', 'Здрасте', 'Тут есть кто?', 'дароф', 'Здорово',
                              'Доброго времечка', 'Привет, бот', 'Здарова', 'Хеллоу', 'Хэллоу',
                              'Доброго времени суток', 'дорого утра', 'Здрасьте', 'йоу', 'Халоу', 'Здравствуйте',
                              'Здравствуй', 'Приветики', 'Салам алейкум', 'Здорово', 'Привет!', 'ку', 'Дратути',
                              'Салам', 'алоха', 'Привет', 'Хэлло', 'Доброй ночи', 'Хай', 'Доброго дня',
                              'Приветик', 'доброго вечера', 'Здоровенько', 'приветики', 'добрый вечер', 'хай',
                              'Добрый день', 'Здоровеньки булы', 'вечер в хату', 'Приветствую!', 'Салют', 'Здрасьте',
                              'вечер добрый', 'Хай', 'день добрый', 'Доброе утро', 'Здорова', 'Хэллоу', 'Салют',
                              'Дарова', 'Прив', 'Хай!'],
                 'responses': ['Приветствую. Чтобы начать поиск пары, отправьте "поиск" или "пара". '
                               '\nЕсли захотите посмотреть прошлые результаты, напишите "результат". '
                               '\nМогу бросить монетку или кубик, пошутить или рассказать интересный факт. '
                               '\nДля повтора - напишите "Ещё"']},
            'bye':
                {'examples': ['Всего доброго', 'бывай', 'Всего хорошего', 'Спокойной ночи', 'Спасибо за помощь',
                              'Пакеда', 'До скорой встречи', 'хорошего дня', 'Ариведерчи', 'Ауфидерзейн', 'Пока-пока',
                              'Бывай', 'Вопросов больше нет', 'пока', 'Я спать', 'До свидания', 'Поки', 'Досвидули',
                              'До встречи', 'Адьос', 'Споки', 'Спишемся', 'Досвидания', 'бай', 'Прощайте',
                              'всего доброго', 'покеда', 'Доброй ночи', 'Надоел', 'Счастливо', 'Всё, давай',
                              'Будь здоров', 'До связи', 'Пока', 'чао', 'конец разговора', 'До скорого',
                              'До свидания и спасибо', 'Все давай',
                              'прощай', 'Досвидание', 'Оревуар', 'Гудбай', 'Прощай', 'Увидимся',
                              'Аста ла виста', 'досвидос', 'увидимся', 'До скорой встречи', 'До завтра'],
                 'responses': ['Всего доброго.', 'Если что, я всегда здесь.', 'Хорошо, вы только пишите мне..',
                               'Очень жаль, занимательная была беседа, пока!', 'Увидимся!', 'Всего хорошего',
                               'До скорой встречи', 'Буду скучать.', 'Счастливо!',
                               'Если что, я всегда тут.', 'Возвращайтесь ещё', 'Приятного дня и до свидания',
                               'Пока, человек!', 'До свидания!', 'До свидания, дорогой товарищ!',
                               'Пакеда!', 'Я всегда здесь', 'Хорошего дня!', 'До связи', 'Оревуар',
                               'Так скоро уходите? До свидания!', 'Пока, пока']},
            'name':
                {'examples': ['Кто ты?', 'как тебя зовут?', 'Как тебя зовут', 'Как тебя звать?', 'Скажи своё имя',
                              'Как тебя можно называть?', 'Как к тебе обращаться?', 'Как мебя зовут?', 'Твоё имя',
                              'скажи свое имя', 'Как твое имя', 'Как зовут', 'Какое у тебя имя?', 'кликуху назови',
                              'Назови свое имя', 'Плиз назови своё имя', 'Скажи как тебя зовут', 'твоё имя',
                              'Как мне тебя называть', 'Скажи свое имя', 'Назови себя', 'У тебя есть имя?',
                              'Как к вам можно обращаться?', 'Как мне к тебе обращаться?', 'Ты кто', 'Как тебя зовут?',
                              'Представься', 'как звать', 'Имя', 'Представься?', 'как тебя по батьке',
                              'Как к тебе обращаться', 'Твое имя', 'Как звать?', 'Назовись', 'В паспорте как записан?',
                              'Как твоё имя'],
                 'responses': ['Я бот, у меня пока нет имени', 'Как хочешь так и называй...',
                               'У меня нет имени, зови меня как хочешь', 'Я ДЕСИПТИКОН. Шучу',
                               'Друзья называют меня – Мёрфи, но для вас я – Робокоп. Шучу']},
            'interesting_facts':
                {'examples': ['Расскажи что-нибудь интересное', 'Напиши что-нибудь интересно',
                              'Что интересного ты знаешь?', 'Скажи что-нибудь интересное', 'Расскажи факт',
                              'Что скажешь', 'Факт', ],
                 'responses': ['Колибри — единственная птица, которая может летать задом наперед.',
                               'Резиновый подлокотник эскалатора в метро двигается с другой скоростью для того, '
                               'чтобы пассажир не уснул на эскалаторе.',
                               'Изначально морковь была фиолетовой. Благодаря скрещиванию в конце 16 века в '
                               'Европе появилась ранее невиданная оранжевая морковь.',
                               'Между плитами пирамиды Хеопса невозможно просунуть лезвие.',
                               'Язык хамелеона вдвое длиннее его тела.',
                               'Сердце белого кита имеет размер автомобиля Фольксваген Жук.',
                               'В казино Лас-Вегаса нет часов. Это делается для того, '
                               'чтобы клиенты казино проводили в заведении больше времени.',
                               'Россия территориально больше, чем Плутон. Площадь поверхности '
                               'Плутона составляет в 16 650 000 кв. км, а России — 17 098 242 кв. км.',
                               'Тараканы появились за 120 млн лет до начала эры динозавров.',
                               'Более чем 70 % населения планеты никогда не слышали звонка телефона. '
                               'В Африке только один из 40 человек имеет телефон.',
                               'Точка типографского шрифта или рукописи весит примерно 0,00000013 грамма',
                               'Язык хамелеона вдвое длиннее его тела',
                               'По закону Великобритании от 1845 года если человек пытался совершиться '
                               'самоубийство он должен быть приговорен к смертной казни',
                               'Каждую секунду 1 % населения Земли мертвецки пьян',
                               'Если бы Земля вращалась в обратную сторону вокруг своей оси, то в году '
                               'было бы на двое суток меньше',
                               'Чаплин занял третье место на конкурсе двойников Чаплина']},
            'joke':
                {'examples': ['Шутка', 'Юмор', 'пошути', 'Ты знаешь шутки?', 'Расскажи анекдот', 'расскажи шутку',
                              'Рассмеши меня', 'Хочу шутку', 'Развесели', 'Анекдот', 'Хочу шутку', 'Расскажи шутку',
                              'Расскажи смешную историю', 'Развесели меня', 'Ты можешь пошутить?', 'Хочу анекдот',
                              'Ты можешь рассказать анекдот?', 'скажи что-нибудь смешное', 'прикол', 'Шуткани'],
                 'responses': ['Я бы с радостью, но ничего не приходит на ум, дайте подумать...',
                               'Кто рано встает, тот зевает весь день.',
                               'Закончился всемирный потоп. Ковчег причаливает к Арарату, и Ной начинает оттуда '
                               'выгружать клетки с медведями, собаками, тиграми, львами и другими животными. '
                               'А на горе армяне сидят и в нарды играют. Один другого в бок толкает и говорит:'
                               '\n- Ара, смотри, цирк приехал!',
                               'Пока компьютеры не умеют мыслить самостоятельно, им можно доверять.',
                               'Доброе слово и боту приятно', 'Кто рано встает, тот зевает весь день',
                               'Главная проблема шуток про git: у каждого своя версия.',
                               '— Чем отличается программист от политика?'
                               '\n— Программисту платят деньги за работающие программы.',
                               '-Как называется изобретённый на Кавказе жанр оперы и балета?\n -Сюита',
                               'Опытный разработчик всегда посмотрит направо и налево, '
                               'даже если переходит улицу с односторонним движением.',
                               'Робот-пылесос снабдили искусственным интеллектом. '
                               'Через пятнадцать минут работы он научился себя выключать',
                               'Жил—был программист и было у него два сына — Антон и Неантон.',
                               'Приходит Годзилла на поле боя с Конгом и говорит:\n- Дай манки',
                               'Уровень локдауна - перестал делить футболки на домашние и не домашние',
                               'Шёл медведь по лесу видит машина горит, сел в неё и сгорел',
                               'Главное достоинство искусственного интеллекта в том, что он умеет '
                               'терять деньги не хуже человека, но при этом не требует свою долю.',
                               'Взрослая жизнь — это когда отправляешь друзьям не смешные картинки, '
                               'а смешные вакансии.',
                               'Около трети россиян боятся потерять работу из-за искусственного интеллекта. '
                               'Зря боятся, никакой интеллект за 15 тыр работать не будет.',
                               'Вопрос армянского радио: Где база нард в Ереване?\nОтвет: Везде\nАР: Ответ правильный!',
                               'Колобок повесился',
                               'Разница между друзьями и врагами чисто анатомическая: первые в трудную '
                               'минуту подставляют плечо, вторые — ногу.',
                               'почему поросята и коровы на упаковках сосисок и колбас так радостно улыбаются? '
                               'Потому что знают — их там нет',
                               'Мюллер заходит в свой кабинет и видит Штирлица, роющегося у него в сейфе с '
                               'секретными документами.'
                               '\n- Что вы здесь делаете, Штирлиц? - спрашивает Мюллер.'
                               '\n- Трамвая жду - отвечает Штирлиц.'
                               '\n- Трамваи здесь не ходят.'
                               '\n- Они нигде не ходят, у трамваев нет ног, - парировал Штирлиц.'
                               '\nМюллер вышел из кабинета покурить и обдумать сложившуюся ситуацию. '
                               'А когда он вернулся, Штирлица не было в кабинете.'
                               '\n"Уехал" - подумал Мюллер',
                               'Встречаются два 60-летних кента, и один говорит другому :'
                               '\n- помнишь, нас в армии бромом кормили ?'
                               '\n- помню'
                               '\n- подействовал',
                               'Макароны прослужат дольше, если они по-флотски!',
                               'Существует два вида языков программирования: одни – все ругают, другими не пользуются.',
                               'Попали как-то в рай немец, итальянец и чат бот...', 'Маска тебе к лицу',
                               'Личные данные в сети какие-то жидкие. Постоянно утекают.',
                               'В магазин "все по 100 рублей" уже завезли доллары',
                               'Цивилизация - это, когда экстрим из повседневности превращается в развлечение',
                               'Чтобы найти с кем-то общий язык, иногда надо уметь вовремя прикусить свой',
                               'Плюсы и минусы быть учёным в средневековье:'
                               '\nПлюсы: ты учёный'
                               '\nМинусы: на костре печёный',
                               'Поймали инопланетяне русского, француза и немца, заперли каждого в замкнутую комнату '
                               '2 на 2 метра, дали каждому два титановых шарика и сказали:'
                               '\n- Кто за день придумает с этими шариками то, что нас удивит, '
                               'того отпустим, а остальнах на опыты отправим!'
                               '\nЧерез день заходят к французу! Тот стоит посреди комнаты и '
                               'виртуозно жонглирует шариками!'
                               '\n- Что ж, француз, ты нас удивил, '
                               'если те двое ничего не придумали, мы тебя отпустим!'
                               '\nЗаходят к немцу! Тот виртуозно жонглирует титановыми шариками при '
                               'этом отбивая чечетку!'
                               '\n- Ну, немец, удивил! Сейчас посмотрим, что там русский придумал и отпустим тебя!'
                               '\nИнопланетянин заходит к русскому! Через минуту выходит весь обалдевший и говорит:'
                               '\n- Нет, ребята, вы проиграли, он один шарик сломал, другой потерял!',
                               'Последний человек на Земле сидел в комнате. В дверь постучалась Елизавета II.',
                               'Иногда будильник помогает проснуться, но чаще всего - мешает спать',
                               'Если число 666 - зло. То, получается, корень всего зла в 25,8069?',
                               'Серотонин и дофамин - единственные вещи, которые действительно доставляют '
                               'тебе удовольствие.']},
            'coin':
                {'examples': ['брось монетку', 'монетка', 'подбрось монетку'],
                 'responses': ['Выпал орел', 'Выпала решка', 'Орел', 'Решка', 'Ой, укатилась']},
            'cube':
                {'examples': ['брось кубик', 'игральный кубик', 'кубик'],
                 'responses': ['1', '2', '3', '4', '5', '6']},
            'repeat':
                {'examples': ['еще', 'ещё', 'повтори', 'снова', 'повтори еще'],
                 'responses': ['Повтор']},
        },
    'failure_phrases':
        ['А вот это не совсем понятно.', 'Я ещё только учусь, спросите что-нибудь другое',
         'Сформулируйте иначе, я не понимаю', 'Не понимаю вас. Скажите по-другому.', 'Непонятно',
         'Простите, но я не понимаю Вас.', 'Я Вас не понимаю', 'Такого я ещё не умею..',
         'Непонятно. Перефразируйте, пожалуйста', 'Не понял вас',
         'Записал твой вопрос, сейчас спрошу у кого-нибудь. Не жди...', 'Слишком сложный вопрос для меня',
         'Не могу Вам ответить', 'Непонятно. Перефразируйте, пожалуйста', 'Попробуйте спросить иначе.',
         'Слишком сложный вопрос для меня.', 'Непонятно. Перефразируйте, пожалуйста',
         'Я еще только учусь. Спросите еще раз', 'Прошу прощения, я не понимаю']
}
