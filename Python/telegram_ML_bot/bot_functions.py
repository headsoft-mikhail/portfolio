import random
import nltk
from bot_config import BOT_CONFIG
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from sklearn.svm import LinearSVC


def clear_phrase(phrase):
    phrase = phrase.lower()
    alphabet = '1234567890абвгдеёжзийклмнопрстуфхцчшщъыьэюя- '
    result = ''.join(symbol for symbol in phrase if symbol in alphabet)
    return result.strip()


def get_dialogues_dataset(filename):
    with open(filename, encoding='utf-8') as f:
        content = f.read()
    dialogues_str = content.split('\n\n')
    dialogues = [dialogue_str.split('\n')[:2] for dialogue_str in dialogues_str]
    dialogues_filtered = []
    questions = set()
    for dialogue in dialogues:
        if len(dialogue) != 2:
            continue
        question, answer = dialogue
        question = clear_phrase(question[2:])
        answer = answer[2:]
        if question not in questions and question != '':
            questions.add(question)
            dialogues_filtered.append([question, answer])
    # restructure dataset to a lot of mini-datasets with keys (key = every word in every question)
    dialogues_structured = {}
    for question, answer in dialogues_filtered:
        words = set(question.split(' '))
        for word in words:
            if word not in dialogues_structured:
                dialogues_structured[word] = []
            dialogues_structured[word].append([question, answer])
    # limit count of pairs with similar keyword in dataset. prefer to shortest questions
    key_volume = 1000
    dialogues_structured_cut = {}
    for word, pairs in dialogues_structured.items():
        pairs.sort(key=lambda pair: len(pair[0]))
        dialogues_structured_cut[word] = pairs[:key_volume]
    return dialogues_structured_cut


def machine_learning_train():
    # подготовка датасета для векторизации и обучения
    x_text = []  # набор строк, в котором будут перечислены все examples из всех intents
    y = []  # набор строк, в котором будут перечислены все intents, соответствующие элементам X_text
    for intent, intent_data in BOT_CONFIG['intents'].items():
        for example in intent_data['examples']:
            x_text.append(example)
            y.append(intent)
    # векторизация
    vectoriser = TfidfVectorizer(ngram_range=(3, 4), analyzer='char')  # задаем векторизатор
    x = vectoriser.fit_transform(x_text)  # преобразует examples в весовые векторы
    # классификация
    clf = LinearSVC()  # задается модель обучения
    clf.fit(x, y)  # обучение
    # валидация качества
    score_med = 0
    average_count = 20
    for i in range(average_count):
        x_train, x_test, y_train, y_test = train_test_split(x, y, test_size=0.3)
        clf = LinearSVC()
        clf.fit(x_train, y_train)
        score_med += clf.score(x_test, y_test) / average_count
    print(score_med)
    return clf, vectoriser


def classify_intent(replica):
    # ML
    replica = clear_phrase(replica)
    if replica == '':
        return
    intent = clf.predict(vectoriser.transform([replica]))[0]
    # check ML intent
    for example in BOT_CONFIG['intents'][intent]['examples']:
        example = clear_phrase(example)
        if example and nltk.edit_distance(example, replica) / len(example) <= 0.5:
            return intent


def get_answer_by_intent(intent):
    if intent in BOT_CONFIG['intents']:
        return random.choice(BOT_CONFIG['intents'][intent]['responses'])


def generate_answer(replica):
    levenstein_threshold = 0.2
    replica = clear_phrase(replica)
    words = set(replica.split(' '))
    mini_dataset = []
    for word in words:
        if word in dialogues_structured_cut:
            mini_dataset += dialogues_structured_cut[word]
    # TODO remove repeats from mini_dataset
    probable_variants = []  # probability,  question, answer
    for question, answer in mini_dataset:
        if abs(len(replica) - len(question)) / len(question) < levenstein_threshold:
            distance_weighted = nltk.edit_distance(question, replica) / len(question)
            if distance_weighted < levenstein_threshold:
                probable_variants.append([distance_weighted, question, answer])
    if probable_variants:
        return min(probable_variants, key=lambda variant: variant[0])[2]


def get_failure_phrase():
    return random.choice(BOT_CONFIG['failure_phrases'])


def bot(replica):
    # NLU
    intent = classify_intent(replica)
    # ANSWER GENERATION
    # select premade replica
    if intent:
        answer = get_answer_by_intent(intent)
        if answer:
            stats['intent'] += 1
            return answer
    # generative model
    answer = generate_answer(replica)
    if answer:
        stats['generate'] += 1
        return answer

    # misunderstand answer
    stats['failure'] += 1
    answer = get_failure_phrase()
    return answer


stats = {'intent': 0, 'generate': 0, 'failure': 0}
clf, vectoriser = machine_learning_train()
dialogues_structured_cut = get_dialogues_dataset('dialogues.txt')
