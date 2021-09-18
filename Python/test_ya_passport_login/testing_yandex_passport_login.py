from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.common.exceptions import NoSuchElementException
import unittest
from tokens import PASSPORT_LOGIN, PASSPORT_PASSWORD
import time


def passport_login(login, password):
    driver = webdriver.Chrome()
    driver.get("https://passport.yandex.ru/auth?retpath=https%3A%2F%2Fpassport.yandex.ru%2Fprofile&noreturn=1")
    # input login
    elem = driver.find_element_by_name("login")
    elem.send_keys(login)
    time.sleep(1)
    elem.send_keys(Keys.RETURN)
    time.sleep(2)
    try:
        # exit if incorrect login
        elem = driver.find_element_by_class_name("Textinput-Hint_state_error")
        print(elem.text)
        driver.close()
        driver.quit()
        return None
    except NoSuchElementException:
        print("Login accepted")
    # input password
    elem = driver.find_element_by_name("passwd")
    elem.send_keys(password)
    time.sleep(1)
    elem.send_keys(Keys.RETURN)
    time.sleep(2)
    try:
        # exit if incorrect password
        elem = driver.find_element_by_class_name("Textinput-Hint_state_error")
        print(elem.text)
        driver.close()
        driver.quit()
        return None
    except NoSuchElementException:
        print("Password accepted")
    try:
        # input call/sms code
        driver.find_element_by_class_name("Button2").click()
        time.sleep(2)
        elem = driver.find_element_by_name("phoneCode")
        elem.send_keys(input("Wait for call or SMS and input code: "))
        time.sleep(1)
        driver.find_element_by_class_name("Button2").click()
    except NoSuchElementException:
        # if yandex doesn't require call/sms confirmation
        print("No call confirmation needed")
    time.sleep(5)
    try:
        account_name = driver.find_element_by_class_name("user-account__name").text
        driver.close()
        driver.quit()
        return account_name
    except NoSuchElementException:
        print("Call/SMS code check failed")
        return driver, None


class TestYandexPassportLogin(unittest.TestCase):
    def setUp(self):
        print('setUp')

    def tearDown(self):
        print('tearDown')

    def test_login_invalid_login(self):
        login = 'test_yandex_login'
        password = PASSPORT_PASSWORD
        self.assertEqual(passport_login(login, password), None)


    def test_login_invalid_password(self):
        login = PASSPORT_LOGIN
        password = '123456'
        self.assertEqual(passport_login(login, password), None)

    def test_login_valid(self):
        login = PASSPORT_LOGIN
        password = PASSPORT_PASSWORD
        self.assertEqual(passport_login(login, password), None)


