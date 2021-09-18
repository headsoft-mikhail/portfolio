import telebot
import random
import datetime

token = TELEGRAM_TOKEN

bot = telebot.TeleBot(token)

bot_states = [
    'creating_task',
    'removing_task',
    'show_date',
    'confirm_clearing',
    'free']
bot_state = bot_states[-1]


bot_speaker = {
    "тебя зовут": "Я - бот для списка задач. А как тебя зовут?",
    "лина": "Линочка - красотулька!",
    "миша": "Здорова, дружище!"}

schedule = {}


####################################################

@bot.message_handler(commands=["coin"])
def coin(message):
    bot.send_message(message.chat.id, random.choice(["орел", "решка"]))

####################################################

def show_help():
    result = [
        'Доступные команды:',
        '/help - вывести справку',
        '/add + дата + время + задача - добавить запись',
        '/addrand - добавить случайную задачу',
        '/remove + дата + время + задача - удалить запись',
        '/show + дата - показать дела на дату',
        '/showAll - показать весь список задач',
        '/clear - очистить список задач',
        '/coin - бросить монетку']
    return '\n'.join(result)


@bot.message_handler(commands=["help"])
def print_help(message):
    bot.send_message(message.chat.id, show_help())


###############################################################################

def tasks_func_show_task(date):
    result = ''
    if date in schedule:
        result += str(date) + ':'
        for time in schedule[date]:
            result += '\n   ' + str(time) + ':'
            for task in schedule[date][time]:
                result += '\n      ' + str(task)
        result += '\n'
    else:
        result = 'Нет записей на эту дату'
    return result


def show_task_checked(message):
    global bot_state
    if len(message.text.replace('/show', '').strip()) > 0:
        date = message.text.replace('/show', '').strip()
        bot.send_message(message.chat.id, tasks_func_show_task(date))
        bot_state = bot_states[-1]
    else:
        bot.send_message(message.chat.id, 'Некорректный ввод')
        bot.send_message(message.chat.id, 'Введите дату для отображения задач\n/exit - отменить')
        bot_state = bot_states[2]


@bot.message_handler(commands=["show"])
def show_task(message):
    show_task_checked(message)


###############################################################################

def tasks_func_show_all_tasks():
    result = ''
    if len(schedule) == 0:
        result = 'Нет записей'
    else:
        for date in schedule:
            result += str(date) + ':'
            for time in schedule[date]:
                result += '\n   ' + str(time) + ':'
                for task in schedule[date][time]:
                    result += '\n      ' + str(task)
            result += '\n'
    return result


@bot.message_handler(commands=["showAll"])
def show_all_tasks(message):
    bot.send_message(message.chat.id, tasks_func_show_all_tasks())


###############################################################################

def tasks_func_add_task(date, time, task):
    if len(date.replace(' ', '')) == 0 or len(time.replace(' ', '')) == 0 or len(task.replace(' ', '')) == 0:
        result = 'Введен пустой запрос'
        return result
    if date not in schedule:
        schedule[date] = {}
    if time not in schedule[date]:
        schedule[date][time] = [task]
    else:
        if task not in schedule[date][time]:
            schedule[date][time].append(task)
    result = 'Задача добавлена!'
    return result


def add_task_checked(message):
    global bot_state
    if message.text.replace('/add', '').strip().count(' ') >= 2:
        [date, time, task] = message.text.replace('/add', '').strip().split(' ', 2)
        bot.send_message(message.chat.id, tasks_func_add_task(date, time, task))
        bot_state = bot_states[-1]
    else:
        bot.send_message(message.chat.id, 'Некорректный ввод')
        bot.send_message(message.chat.id, 'Для добавления введите через пробел дату, время и задачу\n/exit - отменить')
        bot_state = bot_states[0]


@bot.message_handler(commands=["add"])
def add_task(message):
    add_task_checked(message)


###############################################################################

def tasks_func_remove_task(date, time, task):
    if date in schedule:
        if time in schedule[date]:
            if task in schedule[date][time]:
                schedule[date][time].remove(task)
                result = 'Задача удалена!'
                if len(schedule[date][time]) == 0:
                    del schedule[date][time]
                    if len(schedule[date]) == 0:
                        del schedule[date]
            else:
                result = 'Невозможно удалить: такой задачи не было поставлено'
        else:
            result = 'Невозможно удалить: на это время нет задач'
    else:
        result = 'Невозможно удалить: на эту дату нет задач'
    return result


def remove_task_checked(message):
    global bot_state
    if message.text.replace('/remove', '').strip().count(' ') >= 2:
        [date, time, task] = message.text.replace('/remove', '').strip().split(' ', 2)
        bot.send_message(message.chat.id, tasks_func_remove_task(date, time, task))
        bot_state = bot_states[-1]
    else:
        bot.send_message(message.chat.id, 'Некорректный ввод')
        bot.send_message(message.chat.id, 'Для удаления записи, введите через пробел дату и время задачи\n'
                                          '/exit - отменить')
        bot_state = bot_states[1]


@bot.message_handler(commands=["remove"])
def remove_task(message):
    remove_task_checked(message)


###############################################################################

random_task_randomTasksList = [
    'Посмотреть сериал',
    'Сделать домашнее задание',
    'Пораньше лечь спать',
    'Сделать зарядку',
    'Приготовить ужин',
    'Неистово писать код',
    'Позвонить маме',
    'Искупать кошку']


def random_task_getRandomTask():
    randomTask = random_task_randomTasksList[random.randint(0, len(random_task_randomTasksList) - 1)]
    return randomTask


def random_task_getRandomDate():
    # generate random date in the next week
    randomDate = datetime.date.today() + datetime.timedelta(random.randint(1, 7))
    return str(randomDate)


def random_task_getRandomTime():
    randomTime = datetime.time(random.randint(0, 23), random.randint(0, 59))
    return randomTime.isoformat('minutes')


def tasks_func_add_random_task():
    date = random_task_getRandomDate()
    time = random_task_getRandomTime()
    task = random_task_getRandomTask()
    return tasks_func_add_task(date, time, task), "Случайная задача:\n" + date + " " + time + " " + task


@bot.message_handler(commands=["addrand"])
def add_random_task(message):
    result, random_task = tasks_func_add_random_task()
    bot.send_message(message.chat.id, random_task)
    bot.send_message(message.chat.id, result)


###############################################################################

@bot.message_handler(commands=["exit"])
def exit_scenario(message):
    global bot_state
    bot_state = bot_states[-1]


###############################################################################

def confirm_clear(message):
    global bot_state
    if message.text.lower() == "y":
        schedule.clear()
        bot.send_message(message.chat.id, "Список дел очищен")
    else:
        bot.send_message(message.chat.id, "Очистка отменена")
    bot_state = bot_states[-1]


@bot.message_handler(commands=["clear"])
def clear_schedule(message):
    global bot_state
    bot_state = bot_states[3]
    bot.send_message(message.chat.id, "Введите 'Y' для подтверждения")


###############################################################################

@bot.message_handler(content_types=["text"])
def echo(message):
    print(message.text)
    global bot_state
    if bot_state == bot_states[0]:  # add task
        add_task_checked(message)
    elif bot_state == bot_states[1]:  # remove task
        remove_task_checked(message)
    elif bot_state == bot_states[2]:  # show task
        show_task_checked(message)
    elif bot_state == bot_states[3]:  # clear
        confirm_clear(message)
    elif bot_state == bot_states[-1]:  # free
        for question in bot_speaker.keys():
            if question in message.text.lower():
                bot.send_message(message.chat.id, bot_speaker[question])
                return
        bot.send_message(message.chat.id, message.text)


bot.polling(none_stop=True)
